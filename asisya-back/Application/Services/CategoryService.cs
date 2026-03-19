using Application.Ports.Input;
using Application.Ports.Output;
using Domain.Entities.Category;
using Domain.Externals;

namespace Application.Services
{
    public class CategoryService(ICategoryRepository Repo)
        : ICategoryService
    {
        private readonly ICategoryRepository _Repo = Repo;

        public async Task<Result<List<CategoryDTO>>> FindAllAsync()
            => await _Repo.FindAllAsync();

        public async Task<Result<CategoryDTO>> GetAsync(Guid EntityId)
            => await _Repo.GetAsync(EntityId);

        public async Task<Result<CategoryDTO>> CreateAsync(CategoryRequestDTO EntityRequest, Guid ResponsableId)
            => await _Repo.CreateAsync(EntityRequest, ResponsableId);

        public async Task<Result<CategoryDTO>> UpdateAsync(Guid EntityId, CategoryRequestDTO EntityRequest, Guid ResponsableId)
            => await _Repo.UpdateAsync(EntityId, EntityRequest, ResponsableId);

        public async Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId)
            => await _Repo.DeleteAsync(EntityId, ResponsableId);
    }
}
