using Domain.Entities;
using Domain.Enums;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class EmployeeLoginValidator : AbstractValidator<EmployeeLoginDTO>
    {
        public EmployeeLoginValidator()
        {
            RuleFor(e => e.Document)
                .NotEmpty().WithMessage("Campo Obligatorio")
                .MinimumLength(6).WithMessage("Mínimo 6 Caracteres")
                .MaximumLength(10).WithMessage("Máximo 10 Caracteres");

            RuleFor(e => e.DocumentType)
                .NotEmpty().WithMessage("Campo Obligatorio")
                .Must(dt => Enum.TryParse<DocumentTypeEnum>(dt, ignoreCase: true, out _))
                .WithMessage("Tipo de Documento No Válido");
        }
    }
}
