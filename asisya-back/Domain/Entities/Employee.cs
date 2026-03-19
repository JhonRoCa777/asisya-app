using Domain.Enums;

namespace Domain.Entities
{
    public record Employee(
        Guid EmployeeID,
        string Document,
        DocumentTypeEnum DocumentType,
        string LastName,
        string FirstName,
        RoleEnum Role,
        string? Title,
        string? TitleOfCourtesy,
        DateOnly? BirthDate,
        DateOnly? HireDate,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? HomePhone,
        string? Extension,
        string? Photo,
        string? Notes,
        int? ReportsTo,

        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy,
        DateTime? DeletedAt,
        Guid DeletedBy
    );

    public record EmployeeDTO(
        Guid EmployeeID,
        string Document,
        DocumentTypeEnum DocumentType,
        string LastName,
        string FirstName,
        RoleEnum Role,
        string? Title,
        string? TitleOfCourtesy,
        DateOnly? BirthDate,
        DateOnly? HireDate,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? HomePhone,
        string? Extension,
        string? Photo,
        string? Notes,
        int? ReportsTo
    );

    public record EmployeeLoginDTO(
        string Document,
        string DocumentType
    );
}
