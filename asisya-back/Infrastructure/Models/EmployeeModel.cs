using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class EmployeeModel : BaseModel
    {
        [Key]
        public Guid EmployeeID { get; set; }

        [Required, MaxLength(20)]
        public string Document { get; set; } = null!;

        [Required]
        public DocumentTypeEnum DocumentType { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;


        [Required]
        public RoleEnum Role { get; set; }

        public string? Title { get; set; }

        public string? TitleOfCourtesy { get; set; }

        public DateOnly? BirthDate { get; set; }

        public DateOnly? HireDate { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? HomePhone { get; set; }

        public string? Extension { get; set; }

        public string? Photo { get; set; }

        public string? Notes { get; set; }

        public int? ReportsTo { get; set; }
    }
}
