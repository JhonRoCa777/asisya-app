using Application.Ports.Output;
using AutoMapper;
using Domain.Entities.Category;
using Domain.Errors.Types;
using Domain.Externals;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository(
        AppDbContext Context,
        IMapper Mapper
    ) : ICategoryRepository
    {
        private readonly AppDbContext _Context = Context;
        private readonly IMapper _Mapper = Mapper;

        public async Task<Result<List<CategoryDTO>>> FindAllAsync()
        {
            var ModelList = await _Context.Categories
                                        .Where(e => e.DeletedAt == null)
                                        .OrderByDescending(e => e.CategoryId)
                                        .ToListAsync();

            return Result<List<CategoryDTO>>.Success(_Mapper.Map<List<CategoryDTO>>(ModelList));
        }

        public async Task<Result<CategoryDTO>> GetAsync(Guid EntityId)
        {
            var Result = await this.FindAsync(EntityId);

            if (!Result.IsSuccess)
                return Result<CategoryDTO>.Failure(Result.Error);

            return Result<CategoryDTO>.Success(_Mapper.Map<CategoryDTO>(Result.Data));
        }

        public async Task<Result<CategoryDTO>> CreateAsync(CategoryRequestDTO EntityRequest, Guid ResponsableId)
        {
            if (await ExistsByNameAsync(EntityRequest.CategoryName))
                return Result<CategoryDTO>.Failure(new CategoryExistsError());

            var Model = _Mapper.Map<CategoryModel>(EntityRequest);

            Model.CreatedAt = DateTime.UtcNow;
            Model.CreatedBy = ResponsableId;

            await _Context.Categories.AddAsync(Model);
            await _Context.SaveChangesAsync();

            return Result<CategoryDTO>.Success(_Mapper.Map<CategoryDTO>(Model));
        }

        public async Task<Result<CategoryDTO>> UpdateAsync(Guid EntityId, CategoryRequestDTO EntityRequest, Guid ResponsableId)
        {
            if (await ExistsByNameAsync(EntityRequest.CategoryName, EntityId))
                return Result<CategoryDTO>.Failure(new CategoryExistsError());

            var Result = await FindAsync(EntityId);
            if (!Result.IsSuccess)
                return Result<CategoryDTO>.Failure(Result.Error);

            var Model = Result.Data;
            _Mapper.Map(EntityRequest, Model);

            Model.UpdatedAt = DateTime.UtcNow;
            Model.UpdatedBy = ResponsableId;

            await _Context.SaveChangesAsync();

            return Result<CategoryDTO>.Success(_Mapper.Map<CategoryDTO>(Model));
        }

        public async Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId)
        {
            var Result = await FindAsync(EntityId);
            if (!Result.IsSuccess)
                return Result<bool>.Failure(Result.Error);

            var Model = Result.Data;

            Model.DeletedAt = DateTime.UtcNow;
            Model.DeletedBy = ResponsableId;

            await _Context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<bool> ExistsByNameAsync(string EntityName, Guid? EntityId = null)
        {
            return await _Context.Categories
                            .AnyAsync(e => e.CategoryName == EntityName && (!EntityId.HasValue || e.CategoryId != EntityId));
        }

        /************************************** CUSTOM **************************************/

        private async Task<Result<CategoryModel>> FindAsync(Guid entityId)
        {
            var model = await _Context.Categories.FirstOrDefaultAsync(e => e.CategoryId == entityId);

            if (model == null)
                return Result<CategoryModel>.Failure(new CategoryNotFoundError());

            if (model.DeletedAt.HasValue)
                return Result<CategoryModel>.Failure(new CategoryNotActiveError());

            return Result<CategoryModel>.Success(model);
        }

        //public async Task<bool> DeleteAsync(DTO DTO)
        //{
        //    var Entity = _Mapper.Map<Entity>(DTO);
        //    _Context.Entities.Remove(Entity);
        //    await _Context.SaveChangesAsync();
        //    return true;
        //}
    }
}
