using Confluent.Kafka;
using Product_service.Event;
using System.Text.Json;
using System.Threading.Tasks;

public class ProductEventProducer
{
    private readonly IProducer<string, string> _producer;

    public ProductEventProducer(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task<ProductSoldEvent> PublishProductSoldEvent(ProductSoldEvent productSoldEvent)
    {
        var message = JsonSerializer.Serialize(productSoldEvent);
        var kafkaMessage = new Message<string, string>
        {
            Key = productSoldEvent.ProductId.ToString(),
            Value = message
        };

        // Produce message to Kafka topic
        await _producer.ProduceAsync("product-sold", kafkaMessage);
        Console.WriteLine("Event produces", kafkaMessage);
        return productSoldEvent;
    }
}
