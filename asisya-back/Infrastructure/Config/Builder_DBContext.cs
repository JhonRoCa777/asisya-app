using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config
{
    public static class Builder_DBContext
    {
        public static void AddDBContextService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextFactory<AppDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
        }
    }
}
