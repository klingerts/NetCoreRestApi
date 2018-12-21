namespace Kts.RefactorThis.Api.Payloads
{
    public class InboundProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
    }
}
