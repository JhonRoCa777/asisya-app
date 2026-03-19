using Application.Ports.Input;
using Application.Ports.Output;
using Application.Services;
using Domain.Externals;
using Infrastructure.Repositories;

namespace Infrastructure.Config
{
    public static class Builder_DependencyInjection
    {
        public static IServiceCollection AddDependencyInjectionService(this IServiceCollection services, IConfiguration configuration)
        {
            // Utils
            services.AddSingleton(configuration.GetSection("JwtSettings").Get<JwtSettings>()!);
            services.AddSingleton<IJwtRepository, JwtRepository>();

            // Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();

            // Repository
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
