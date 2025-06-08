namespace Product_service.Event
{
    public class ProductSoldEvent
    {
        public Guid ProductId { get; set; }
        public int QuantitySold { get; set; }
        public DateTime SoldAt{ get; set; }
    }
}
