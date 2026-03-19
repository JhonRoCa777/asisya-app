using Domain.Entities.Product;
using Domain.Externals;

namespace Application.Ports.Output
{
    public interface IProductRepository
    {
        Task<Result<PagedResult<ProductToIndex>>> FindAllAsync(
            string? nameFilter, int pageNumber, int pageSize, string orderBy, bool orderAsc
        );

        Task<Result<List<ProductDTO>>> CreateByRowsAsync(int EntityRows, Guid ResponsableId, int simulateErrors);

        Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId);
    }
}
