using System;

namespace Kts.RefactorThis.DTO
{
    public class CreateProductOptionDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}