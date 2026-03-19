using Domain.Entities.Category;
using Domain.Externals;

namespace Application.Ports.Output
{
    public interface ICategoryRepository
    {
        Task<Result<List<CategoryDTO>>> FindAllAsync();

        Task<Result<CategoryDTO>> GetAsync(Guid EntityId);

        Task<Result<CategoryDTO>> CreateAsync(CategoryRequestDTO EntityRequest, Guid ResponsableId);

        Task<Result<CategoryDTO>> UpdateAsync(Guid EntityId, CategoryRequestDTO EntityRequest, Guid ResponsableId);

        Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId);

        Task<bool> ExistsByNameAsync(string EntityName, Guid? EntityId = null);
    }
}
