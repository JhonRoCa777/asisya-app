using Application.Ports.Input;
using Application.Ports.Output;
using Domain.Entities.Product;
using Domain.Externals;

namespace Application.Services
{
    public class ProductService(IProductRepository Repo)
        : IProductService
    {
        private readonly IProductRepository _Repo = Repo;

        public async Task<Result<PagedResult<ProductToIndex>>> FindAllAsync(
            string? nameFilter, int pageNumber, int pageSize, string orderBy, bool orderAsc
        ) => await _Repo.FindAllAsync(nameFilter, pageNumber, pageSize, orderBy, orderAsc);

        //public async Task<Result<CategoryDTO>> GetAsync(Guid EntityId)
        //    => await _Repo.GetAsync(EntityId);

        public async Task<Result<List<ProductDTO>>> CreateByRowsAsync(int EntityRows, Guid ResponsableId, int simulateErrors)
            => await _Repo.CreateByRowsAsync(EntityRows, ResponsableId, simulateErrors);

        //public async Task<Result<CategoryDTO>> UpdateAsync(Guid EntityId, CategoryRequestDTO EntityRequest, Guid ResponsableId)
        //    => await _Repo.UpdateAsync(EntityId, EntityRequest, ResponsableId);

        public async Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId)
            => await _Repo.DeleteAsync(EntityId, ResponsableId);
    }
}
