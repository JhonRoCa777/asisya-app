using Domain.Entities.Supplier;
using Domain.Externals;

namespace Application.Ports.Input
{
    public interface ISupplierService
    {
        Task<Result<List<SupplierDTO>>> FindAllAsync();
    }
}
