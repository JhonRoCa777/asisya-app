using Application.Services;
using Domain.Entities.Category;
using FluentAssertions;
using Infrastructure.Repositories;

namespace Test
{
    public class CategoryServiceIntegrationTests
    {
        [Fact]
        public async Task CRUD_Category_Should_Work()
        {
            // Crear DbContext en memoria
            await using var context = DbContextHelper.CreateContext();

            // Crear Mapper
            var mapper = MapperHelper.CreateMapper();

            // Crear repositorio real
            var repo = new CategoryRepository(context, mapper);

            // Servicio
            var service = new CategoryService(repo);

            var responsableId = Guid.NewGuid();

            // ---------------- CREATE ----------------
            var createRequest = new CategoryRequestDTO(
                "Electronics",
                "Electronic devices",
                null
            );

            var created = await service.CreateAsync(createRequest, responsableId);

            created.IsSuccess.Should().BeTrue();
            created.Data.CategoryName.Should().Be("Electronics");
            created.Data.Description.Should().Be("Electronic devices");

            var categoryId = created.Data.CategoryId;

            // ---------------- READ ----------------
            var found = await service.GetAsync(categoryId);
            found.IsSuccess.Should().BeTrue();
            found.Data.CategoryName.Should().Be("Electronics");

            // ---------------- UPDATE ----------------
            var updateRequest = new CategoryRequestDTO
            (
                "Tech",
                "Tech gadgets",
                null
            );
            var updated = await service.UpdateAsync(categoryId, updateRequest, responsableId);

            updated.IsSuccess.Should().BeTrue();
            updated.Data.CategoryName.Should().Be("Tech");
            updated.Data.Description.Should().Be("Tech gadgets");
            //updated.Data.Picture.Should().Be("tech.png");

            // ---------------- GET ALL ----------------
            var all = await service.FindAllAsync();
            all.IsSuccess.Should().BeTrue();
            all.Data.Count.Should().Be(1);
            all.Data[0].CategoryName.Should().Be("Tech");

            // ---------------- DELETE ----------------
            var deleted = await service.DeleteAsync(categoryId, responsableId);
            deleted.IsSuccess.Should().BeTrue();
            deleted.Data.Should().BeTrue();

            // ---------------- VERIFY DELETE ----------------
            var afterDelete = await service.GetAsync(categoryId);
            afterDelete.IsSuccess.Should().BeFalse();
        }
    }
}