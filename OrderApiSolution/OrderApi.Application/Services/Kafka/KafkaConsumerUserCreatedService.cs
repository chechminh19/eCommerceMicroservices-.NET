using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderApi.Application.Interfaces;
using System.Text.Json;


namespace OrderApi.Application.Services.Kafka
{
    public class KafkaConsumerUserCreatedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly ILogger<KafkaConsumerUserCreatedService> _logger;
        public KafkaConsumerUserCreatedService(IServiceProvider serviceProvider, IConfiguration config, ILogger<KafkaConsumerUserCreatedService> logger)
        {
            _serviceProvider = serviceProvider;
            _config = config;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=== KAFKA CONSUMER STARTING ===");

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = "order-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.Plaintext,
                EnableAutoCommit = false,
                SessionTimeoutMs = 30000,
                HeartbeatIntervalMs = 10000
            };

            _logger.LogInformation($"Kafka config - BootstrapServers: {consumerConfig.BootstrapServers}");
            _logger.LogInformation($"Kafka config - GroupId: {consumerConfig.GroupId}");

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
                .SetErrorHandler((_, e) =>
                {
                    _logger.LogError($"=== KAFKA ERROR === {e.Reason}");
                })
                .SetLogHandler((_, message) =>
                {
                    _logger.LogInformation($"=== KAFKA LOG === {message.Message}");
                })
                .Build();

            try
            {
                _logger.LogInformation("=== SUBSCRIBING TO TOPIC: user-created ===");
                var userCreatedTopic = _config["Kafka:Topics:UserCreated"];
                _logger.LogInformation("=== SUBSCRIPTION SUCCESSFUL ===");

                _logger.LogInformation("=== STARTING CONSUME LOOP ===");
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogDebug("Waiting for message...");
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

                        if (consumeResult?.Message != null)
                        {
                            _logger.LogInformation($"=== MESSAGE RECEIVED === {consumeResult.Message.Value}");

                            var userCreated = JsonSerializer.Deserialize<UserCreatedEvent>(consumeResult.Message.Value);
                            if (userCreated != null)
                            {
                                using var scope = _serviceProvider.CreateScope();
                                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                                await orderService.HandleUserCreatedAsync(userCreated);

                                consumer.Commit(consumeResult);
                                _logger.LogInformation("=== MESSAGE PROCESSED SUCCESSFULLY ===");
                            }
                        }
                        else
                        {
                            _logger.LogDebug("No message received, continuing...");
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "=== CONSUME EXCEPTION ===");
                        await Task.Delay(5000, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "=== GENERAL EXCEPTION ===");
                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "=== FATAL ERROR IN KAFKA CONSUMER ===");
            }
            finally
            {
                consumer?.Close();
                _logger.LogInformation("=== KAFKA CONSUMER CLOSED ===");
            }
        }
    }
}
public class UserCreatedEvent
{
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
