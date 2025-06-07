using Confluent.Kafka;
using InventoryService.Application.Commands;
using InventoryService.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static Confluent.Kafka.ConfigPropertyNames;

public class ProductSoldEventConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IMediator _mediator;
    private readonly ILogger<ProductSoldEventConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;


    public ProductSoldEventConsumer(
        IConsumer<string, string> consumer,
        IServiceProvider serviceProvider,
        IMediator mediator,
        ILogger<ProductSoldEventConsumer> logger
        )
    {
        _consumer = consumer;
        _mediator = mediator;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("KafkaProductSoldConsumer started...");

        _consumer.Subscribe("product-sold");
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(stoppingToken);

            var productSoldEvent = JsonConvert.DeserializeObject<ProductSoldEvent>(consumeResult.Message.Value);

            // Create a scope for resolving scoped dependencies
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Handle the event (e.g., update inventory quantity)
                await HandleProductSoldEvent(productSoldEvent, mediator);
            }
        }
        _consumer.Close();
    }

    private async Task HandleProductSoldEvent(ProductSoldEvent productSoldEvent, IMediator mediator)
    {
        var command = new UpdateInventoryCommand(productSoldEvent.ProductId, productSoldEvent.QuantitySold);

        await mediator.Send(command);
    }
}
