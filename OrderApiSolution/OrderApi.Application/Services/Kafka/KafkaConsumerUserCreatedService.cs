using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Application.Interfaces;
using System.Text.Json;


namespace OrderApi.Application.Services.Kafka
{
    public class KafkaConsumerUserCreatedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        public KafkaConsumerUserCreatedService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = "order-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe("user-created");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                if (consumeResult?.Message != null)
                {
                    Console.WriteLine($"[OrderService] Received: {consumeResult.Message.Value}");

                    // Parse payload
                    var userCreated = JsonSerializer.Deserialize<UserCreatedEvent>(consumeResult.Message.Value);

                    if (userCreated != null)
                    {
                        // Call service to handle
                        using var scope = _serviceProvider.CreateScope();
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                        await orderService.HandleUserCreatedAsync(userCreated);
                    }
                }
                await Task.Delay(1);
            }
        }
    }
}
public class UserCreatedEvent
{
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
