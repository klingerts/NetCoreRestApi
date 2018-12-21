using System;

namespace Kts.RefactorThis.Api.Payloads
{
    public class OutboundProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
    }
}
