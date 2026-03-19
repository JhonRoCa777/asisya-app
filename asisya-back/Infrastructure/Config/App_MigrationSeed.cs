using Bogus;
using Domain.Enums;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config
{
    public static class App_MigrationSeed
    {
        public static WebApplication UseMigrationSeed (this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                // MIGRATIONS
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();

                // SEEDERS
                var firstEmployeeId = Guid.NewGuid();
                // EMPLOYEES
                if (!db.Employees.Any())
                {
                    var employees = new List<EmployeeModel>
                    {
                        new() {
                            EmployeeID = firstEmployeeId,
                            Document = "1005772426",
                            DocumentType = DocumentTypeEnum.CC,
                            FirstName = "Jhonatan",
                            LastName = "Romero",
                            Role = RoleEnum.ADMIN,
                            CreatedBy = firstEmployeeId
                        },
                        new() {
                            EmployeeID = Guid.NewGuid(),
                            Document = "1234567890",
                            DocumentType = DocumentTypeEnum.CC,
                            FirstName = "Stiven",
                            LastName = "Campuzano",
                            Role = RoleEnum.USER,
                            CreatedBy = firstEmployeeId
                        },
                        new() {
                            EmployeeID = Guid.NewGuid(),
                            Document = "9876543210",
                            DocumentType = DocumentTypeEnum.CC,
                            FirstName = "Pepito",
                            LastName = "Perez",
                            Role = RoleEnum.USER,
                            CreatedBy = firstEmployeeId,
                            DeletedBy = firstEmployeeId,
                            DeletedAt = DateTime.UtcNow
                        }
                    };

                    db.Employees.AddRange(employees);
                    db.SaveChanges();
                }

                // CATEGORIES
                if (!db.Categories.Any())
                {
                    var categoryNames = new HashSet<string>();
                    var categories = new List<CategoryModel>();

                    while (categories.Count < 5)
                    {
                        var name = new Faker().Commerce.Categories(1).First();
                        if (categoryNames.Add(name))
                        {
                            categories.Add(new CategoryModel
                            {
                                CategoryId = Guid.NewGuid(),
                                CategoryName = name,
                                Description = new Faker().Commerce.ProductDescription(),
                                Picture = new Faker().Image.PicsumUrl(),
                                CreatedBy = firstEmployeeId
                            });
                        }
                    }

                    db.Categories.AddRange(categories);
                    db.SaveChanges();
                }

                // SUPPLIERS
                if (!db.Suppliers.Any())
                {
                    var companyNames = new HashSet<string>();
                    var suppliers = new List<SupplierModel>();

                    while (suppliers.Count < 5)
                    {
                        var name = new Faker().Company.CompanyName();
                        if (companyNames.Add(name))
                        {
                            suppliers.Add(new SupplierModel
                            {
                                SupplierId = Guid.NewGuid(),
                                CompanyName = name,
                                CreatedBy = firstEmployeeId
                            });
                        }
                    }

                    db.Suppliers.AddRange(suppliers);
                    db.SaveChanges();
                }

                // PRODUCTS
                if (!db.Products.Any())
                {
                    var categories = db.Categories.ToList();
                    var suppliers = db.Suppliers.ToList();
                    var faker = new Faker();

                    foreach (var category in categories)
                    {
                        var productNames = new HashSet<string>();
                        var categoryProducts = new List<ProductModel>();

                        var productFaker = new Faker<ProductModel>()
                            .RuleFor(p => p.ProductId, f => Guid.NewGuid())
                            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                            .RuleFor(p => p.SupplierId, f => f.PickRandom(suppliers).SupplierId)
                            .RuleFor(p => p.CategoryId, category.CategoryId)
                            .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(5, 500)))
                            .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Int(0, 200))
                            .RuleFor(p => p.UnitsOnOrder, f => (short)f.Random.Int(0, 50))
                            .RuleFor(p => p.ReorderLevel, f => (short)f.Random.Int(5, 20))
                            .RuleFor(p => p.Discontinued, f => f.Random.Bool(0.1f))
                            .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
                            .RuleFor(p => p.CreatedBy, Guid.NewGuid())
                            .RuleFor(p => p.UpdatedAt, DateTime.UtcNow)
                            .RuleFor(p => p.UpdatedBy, Guid.NewGuid())
                            .RuleFor(p => p.DeletedAt, (DateTime?)null)
                            .RuleFor(p => p.DeletedBy, Guid.Empty);

                        while (categoryProducts.Count < 10)
                        {
                            var product = productFaker.Generate();
                            if (productNames.Add(product.ProductName))
                            {
                                product.CreatedBy = firstEmployeeId;
                                categoryProducts.Add(product);
                            }
                        }

                        db.Products.AddRange(categoryProducts);
                    }

                    db.SaveChanges();
                }
            }

            return app;
        }
    }
}
