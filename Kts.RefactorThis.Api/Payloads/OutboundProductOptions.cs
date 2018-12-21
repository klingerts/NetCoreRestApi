using System.Collections.Generic;

namespace Kts.RefactorThis.Api.Payloads
{
    public class OutboundProductOptions
    {
        public IEnumerable<OutboundProductOption> Items { get; set; }
    }
}
