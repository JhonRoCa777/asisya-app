using Application.Ports.Output;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors.Types;
using Domain.Externals;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository(
        AppDbContext Context,
        IMapper Mapper
    ) : IEmployeeRepository
    {
        private readonly AppDbContext _Context = Context;
        private readonly IMapper _Mapper = Mapper;

        public async Task<Result<EmployeeDTO>> FindByDocumentAsync(string Document, string DocumentType)
        {
            if (!Enum.TryParse<DocumentTypeEnum>(DocumentType, ignoreCase: true, out var docTypeEnum))
                return Result<EmployeeDTO>.Failure(new DocumentTypeNotFoundError());

            var Model = await _Context.Employees
                .Where(e => e.Document == Document)
                .Where(e => e.DocumentType == docTypeEnum)
                .FirstOrDefaultAsync();

            if (Model == null)
                return Result<EmployeeDTO>.Failure(new EmployeeNotFoundError());

            if (Model.DeletedAt != null)
                return Result<EmployeeDTO>.Failure(new EmployeeNotActiveError());

            return Result<EmployeeDTO>.Success(_Mapper.Map<EmployeeDTO>(Model));
        }
    }
}
