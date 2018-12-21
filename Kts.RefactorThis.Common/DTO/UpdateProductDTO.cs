using System;

namespace Kts.RefactorThis.DTO
{
    public class UpdateProductDTO : CreateProductDTO
    {
        public Guid Id { get; set; }
    }
}
