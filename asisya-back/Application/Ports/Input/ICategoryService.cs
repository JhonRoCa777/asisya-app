using Domain.Entities.Category;
using Domain.Externals;

namespace Application.Ports.Input
{
    public interface ICategoryService
    {
        Task<Result<List<CategoryDTO>>> FindAllAsync();

        Task<Result<CategoryDTO>> GetAsync(Guid EntityId);

        Task<Result<CategoryDTO>> CreateAsync(CategoryRequestDTO EntityRequest, Guid ResponsableId);

        Task<Result<CategoryDTO>> UpdateAsync(Guid EntityId, CategoryRequestDTO EntityRequest, Guid ResponsableId);

        Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId);
    }
}
