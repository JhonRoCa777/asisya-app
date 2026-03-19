using Domain.Entities;
using Domain.Externals;

namespace Application.Ports.Output
{
    public interface IEmployeeRepository
    {
        Task<Result<EmployeeDTO>> FindByDocumentAsync(string Document, string DocumentType);
    }
}
