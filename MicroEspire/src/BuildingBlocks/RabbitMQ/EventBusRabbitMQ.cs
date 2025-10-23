using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RabbitMQ
{
    public class EventBusRabbitMQ(IPublishEndpoint publishEndpoint, ILogger<EventBusRabbitMQ> logger) : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<EventBusRabbitMQ> _logger = logger;
        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IntegrationEvent
        {
            try
            {
                _logger.LogInformation("Publishing integration event: {EventId} - {EventType}", @event.Id, typeof(T).Name);
                await _publishEndpoint.Publish(@event, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event: {EventId}", @event.Id);
                throw;
            }
        }
    }
}
