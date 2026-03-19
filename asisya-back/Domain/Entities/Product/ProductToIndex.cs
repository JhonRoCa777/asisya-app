using Domain.Entities.Category;
using Domain.Entities.Supplier;

namespace Domain.Entities.Product
{
    public class ProductToIndex
    {
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public SupplierToRef Supplier { get; set; } = new();
        public CategoryToRef Category { get; set; } = new();
    }
}
