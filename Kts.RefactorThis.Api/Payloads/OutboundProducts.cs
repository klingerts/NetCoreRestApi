using System.Collections.Generic;

namespace Kts.RefactorThis.Api.Payloads
{
    public class OutboundProducts
    {
        public IEnumerable<OutboundProduct> Items { get; set; }
    }
}
