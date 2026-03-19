using Application.Ports.Output;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bogus;
using Domain.Entities.Product;
using Domain.Errors.Types;
using Domain.Externals;
using EFCore.BulkExtensions;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Algorithm;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class ProductRepository(
        AppDbContext Context,
        IDbContextFactory<AppDbContext> ContextFactory,
        IMapper Mapper
    ) : IProductRepository
    {
        private readonly AppDbContext _Context = Context;
        private readonly IDbContextFactory<AppDbContext> _ContextFactory = ContextFactory;
        private readonly IMapper _Mapper = Mapper;

        public async Task<Result<PagedResult<ProductToIndex>>> FindAllAsync(
            string? nameFilter, int pageNumber, int pageSize, string orderBy, bool orderAsc
        )
        {
            var query = _Context.Products
                .Where(p => string.IsNullOrWhiteSpace(nameFilter) || p.ProductName.Contains(nameFilter));

            query = orderBy switch
            {
                "ProductName" => orderAsc ? query.OrderBy(p => p.ProductName) : query.OrderByDescending(p => p.ProductName),
                _ => orderAsc ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt)
            };

            var total = await query.CountAsync();

            var modelList = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ProductToIndex>(_Mapper.ConfigurationProvider)
                .ToListAsync();

            return Result<PagedResult<ProductToIndex>>.Success(new PagedResult<ProductToIndex>
            {
                Items = modelList,
                TotalCount = total
            });
        }

        public async Task<Result<List<ProductDTO>>> CreateByRowsAsync(int EntityRows, Guid ResponsableId, int simulateErrors)
        {
            var categories = await _Context.Categories.ToListAsync();
            var suppliers = await _Context.Suppliers.ToListAsync();

            var errorIndices = new HashSet<int>();
            if (simulateErrors > 0)
            {
                var rnd = new Random();
                while (errorIndices.Count < simulateErrors)
                    errorIndices.Add(rnd.Next(EntityRows));
            }

            IEnumerable<ProductModel> GenerateProductsStream()
            {
                var faker = new Faker<ProductModel>()
                    .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                    .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).CategoryId)
                    .RuleFor(p => p.SupplierId, f => f.PickRandom(suppliers).SupplierId)
                    .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(5, 500)))
                    .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Int(0, 200))
                    .RuleFor(p => p.UnitsOnOrder, f => (short)f.Random.Int(0, 50))
                    .RuleFor(p => p.ReorderLevel, f => (short)f.Random.Int(5, 20))
                    .RuleFor(p => p.Discontinued, f => f.Random.Bool(0.1f));

                for (int i = 0; i < EntityRows; i++)
                {
                    var product = faker.Generate();

                    if (errorIndices.Contains(i))
                        product.ProductName = null;

                    yield return new ProductModel
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = product.ProductName,
                        SupplierId = product.SupplierId,
                        CategoryId = product.CategoryId,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock,
                        UnitsOnOrder = product.UnitsOnOrder,
                        ReorderLevel = product.ReorderLevel,
                        Discontinued = product.Discontinued,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = ResponsableId,
                        UpdatedAt = DateTime.UtcNow,
                        UpdatedBy = ResponsableId,
                        DeletedAt = null,
                        DeletedBy = Guid.Empty
                    };
                }
            }

            // Acumular batches
            var batches = new List<List<ProductModel>>();
            var batch = new List<ProductModel>();

            foreach (var product in GenerateProductsStream())
            {
                batch.Add(product);

                if (batch.Count == 5000)
                {
                    batches.Add(batch.ToList());
                    batch.Clear();
                }
            }

            if (batch.Count != 0)
                batches.Add(batch);

            // Procesar en paralelo
            var failedProducts = new ConcurrentBag<ProductModel>();

            var sw = Stopwatch.StartNew();

            await Task.WhenAll(
                batches.Select(b => ProcessBatchDivideAndConquerAsync(b, failedProducts))
            );

            sw.Stop();
            Console.WriteLine($"[CreateByRowsAsync] Tiempo total: {sw.Elapsed.TotalSeconds:F2}s — Batches: {batches.Count} — Filas: {EntityRows} — Errores: {simulateErrors}");

            return Result<List<ProductDTO>>.Success(
                _Mapper.Map<List<ProductDTO>>(failedProducts.ToList())
            );
        }

        private async Task ProcessBatchDivideAndConquerAsync(List<ProductModel> batch, ConcurrentBag<ProductModel> failedProducts)
        {
            batch = [.. batch
                    .GroupBy(p => p.ProductName)
                    .Select(g => g.First())];

            if (batch.Count == 0) return;

            if (batch.Count == 1)
            {
                await using var context = await _ContextFactory.CreateDbContextAsync();
                try
                {
                    await context.BulkInsertOrUpdateAsync(batch, options =>
                    {
                        options.UpdateByProperties = [nameof(ProductModel.ProductName)];
                        options.EnableStreaming = true;
                    });
                }
                catch
                {
                    failedProducts.Add(batch[0]);
                }
                return;
            }

            try
            {
                await using var context = await _ContextFactory.CreateDbContextAsync();
                await context.BulkInsertOrUpdateAsync(batch, options =>
                {
                    options.UpdateByProperties = [nameof(ProductModel.ProductName)];
                    options.EnableStreaming = true;
                });
            }
            catch
            {
                int mid = batch.Count / 2;
                await ProcessBatchDivideAndConquerAsync(batch.Take(mid).ToList(), failedProducts);
                await ProcessBatchDivideAndConquerAsync(batch.Skip(mid).ToList(), failedProducts);
            }
        }

        //public async Task<Result<bool>> CreateByRowsAsync(int EntityRows, Guid ResponsableId)
        //{
        //    var categories = await _Context.Categories.ToListAsync();
        //    var suppliers = await _Context.Suppliers.ToListAsync();

        //    IEnumerable<ProductModel> GenerateProductsStream()
        //    {
        //        var faker = new Faker<ProductModel>()
        //            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
        //            .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).CategoryId)
        //            .RuleFor(p => p.SupplierId, f => f.PickRandom(suppliers).SupplierId)
        //            .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(5, 500)))
        //            .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Int(0, 200))
        //            .RuleFor(p => p.UnitsOnOrder, f => (short)f.Random.Int(0, 50))
        //            .RuleFor(p => p.ReorderLevel, f => (short)f.Random.Int(5, 20))
        //            .RuleFor(p => p.Discontinued, f => f.Random.Bool(0.1f));

        //        for (int i = 0; i < EntityRows; i++)
        //        {
        //            var product = faker.Generate();

        //            yield return new ProductModel
        //            {
        //                ProductId = Guid.NewGuid(),
        //                ProductName = product.ProductName,
        //                SupplierId = product.SupplierId,
        //                CategoryId = product.CategoryId,
        //                UnitPrice = product.UnitPrice,
        //                UnitsInStock = product.UnitsInStock,
        //                UnitsOnOrder = product.UnitsOnOrder,
        //                ReorderLevel = product.ReorderLevel,
        //                Discontinued = product.Discontinued,
        //                CreatedAt = DateTime.UtcNow,
        //                CreatedBy = ResponsableId,
        //                UpdatedAt = DateTime.UtcNow,
        //                UpdatedBy = ResponsableId,
        //                DeletedAt = null,
        //                DeletedBy = Guid.Empty
        //            };
        //        }
        //    }

        //    await _Context.BulkInsertOrUpdateAsync(GenerateProductsStream(), options =>
        //    {
        //        options.UpdateByProperties = [nameof(ProductModel.ProductName)];
        //        options.BatchSize = 5000;
        //        options.EnableStreaming = true;
        //    });

        //    return Result<bool>.Success(true);
        //}

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

        /************************************** CUSTOM **************************************/

        private async Task<Result<ProductModel>> FindAsync(Guid entityId)
        {
            var Model = await _Context.Products.FirstOrDefaultAsync(e => e.ProductId == entityId);

            if (Model == null)
                return Result<ProductModel>.Failure(new CategoryNotFoundError());

            if (Model.DeletedAt.HasValue)
                return Result<ProductModel>.Failure(new CategoryNotActiveError());

            return Result<ProductModel>.Success(Model);
        }
    }
}
