using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class ProductModel : BaseModel
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; } = null!;

        [ForeignKey("Supplier")]
        public Guid? SupplierId { get; set; }

        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }

        public short UnitsOnOrder { get; set; }

        public short ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public SupplierModel? Supplier { get; set; }

        public CategoryModel? Category { get; set; }
    }
}
