using Application.Ports.Output;
using AutoMapper;
using Domain.Entities.Supplier;
using Domain.Externals;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SupplierRepository(
        AppDbContext Context,
        IMapper Mapper
    ) : ISupplierRepository
    {
        private readonly AppDbContext _Context = Context;
        private readonly IMapper _Mapper = Mapper;

        public async Task<Result<List<SupplierDTO>>> FindAllAsync()
        {
            var ModelList = await _Context.Suppliers
                                        .Where(e => e.DeletedAt == null)
                                        .OrderByDescending(e => e.SupplierId)
                                        .ToListAsync();

            return Result<List<SupplierDTO>>.Success(_Mapper.Map<List<SupplierDTO>>(ModelList));
        }
    }
}
