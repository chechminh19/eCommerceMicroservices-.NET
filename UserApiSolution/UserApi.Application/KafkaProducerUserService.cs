using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserApi.Application.Interfaces;

namespace UserApi.Application
{
    public class KafkaProducerUserService : IKafkaProducerUserService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "user-created";
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaProducerUserService> _logger;
        public KafkaProducerUserService(IConfiguration configuration, ILogger<KafkaProducerUserService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var bootstrapServers = _configuration["Kafka:BootstrapServers"];
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = "KafkaProducerUserService"
            };
            try
            {
                _producer = new ProducerBuilder<Null, string>(config).Build();
                _logger.LogInformation("Kafka producer initialized successfully for bootstrap servers: {BootstrapServers}", bootstrapServers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Kafka producer for bootstrap servers: {BootstrapServers}", bootstrapServers);
                throw;
            }
        }
        public async Task PublishUserCreatedEvent(int userId)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new { UserId = userId, CreatedAt = DateTime.Now });
                await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = payload });
                _logger.LogInformation("Published user-created event to Kafka: {Payload}", payload);
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError(ex, "Failed to publish user-created event to Kafka for user ID: {UserId}", userId);
                throw;
            }
        }
    }
}
