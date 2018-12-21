using System;

namespace Kts.RefactorThis.DTO
{
    public class UpdateProductOptionDTO : CreateProductOptionDTO
    {
        // OptionId
        public Guid OptionId { get; set; }
    }
}
