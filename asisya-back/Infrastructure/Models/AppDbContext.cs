using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<CategoryModel> Categories => Set<CategoryModel>();
        public DbSet<ProductModel> Products => Set<ProductModel>();
        public DbSet<EmployeeModel> Employees => Set<EmployeeModel>();
        public DbSet<SupplierModel> Suppliers => Set<SupplierModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SupplierModel>()
                .HasIndex(s => s.CompanyName).IsUnique();

            modelBuilder.Entity<CategoryModel>()
                .HasIndex(s => s.CategoryName).IsUnique();

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.Property(e => e.UnitPrice).HasColumnType("DECIMAL(18,2)");
                entity.HasIndex(e => e.ProductName).IsUnique();
            });                

            modelBuilder.Entity<EmployeeModel>(entity =>
            {
                entity.Property(e => e.DocumentType).HasConversion<string>();
                entity.Property(e => e.Role).HasConversion<string>();
                entity.HasIndex(e => new { e.Document, e.DocumentType }).IsUnique();
            });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamptz")
                    .IsRequired()
                    .HasDefaultValueSql("NOW()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("UpdatedAt")
                    .HasColumnType("timestamptz");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("DeletedAt")
                    .HasColumnType("timestamptz");
            }
        }
    }
}
