using Domain.Entities.Category;
using Domain.Entities.Supplier;

namespace Domain.Entities.Product
{
    public record ProductTo(
        Guid ProductId,
        string ProductName,
        int SupplierId,
        int CategoryId,
        decimal UnitPrice,
        short UnitsInStock,
        short UnitsOnOrder,
        short ReorderLevel,
        bool Discontinued,

        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy,
        DateTime? DeletedAt,
        Guid DeletedBy,

        SupplierTo? Supplier = null,
        CategoryTo? Category = null
    );

    public record ProductDTO(
        Guid ProductId,
        string ProductName,
        Guid SupplierId,
        Guid CategoryId,
        decimal UnitPrice,
        short UnitsInStock,
        short UnitsOnOrder
    );
}
