using Application.Ports.Input;
using Application.Ports.Output;
using Domain.Entities.Supplier;
using Domain.Externals;

namespace Application.Services
{
    public class SupplierService(ISupplierRepository Repo)
        : ISupplierService
    {
        private readonly ISupplierRepository _Repo = Repo;

        public async Task<Result<List<SupplierDTO>>> FindAllAsync()
            => await _Repo.FindAllAsync();
    }
}
