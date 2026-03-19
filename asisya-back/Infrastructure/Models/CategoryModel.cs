using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    [Table("Categories")]
    public class CategoryModel : BaseModel
    {
        [Key]
        public Guid CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public string? Picture { get; set; }

        public ICollection<ProductModel>? Products { get; set; }
    }
}
