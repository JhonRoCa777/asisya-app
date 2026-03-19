namespace Domain.Entities.Category
{
    public record CategoryTo(
        Guid CategoryId,
        string CategoryName,
        string? Description,
        string? Picture,

        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy,
        DateTime? DeletedAt,
        Guid DeletedBy
    );

    public record CategoryDTO(
        Guid CategoryId,
        string CategoryName,
        string? Description,
        string? Picture
    );

    public record CategoryRequestDTO(
        string CategoryName,
        string? Description,
        string? Picture
    );

    public record CategoryPkDTO(
        Guid SupplierId,
        string CompanyName
    );
}
