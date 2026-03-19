using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class SupplierModel : BaseModel
    {
        [Key]
        public Guid SupplierId { get; set; }

        [Required, MaxLength(100)]
        public string CompanyName { get; set; } = null!;

        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? HomePage { get; set; }

        public ICollection<ProductModel>? Products { get; set; }
    }
}
