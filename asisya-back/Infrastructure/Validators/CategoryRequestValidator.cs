using Domain.Entities.Category;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequestDTO>
    {
        public CategoryRequestValidator()
        {
            RuleFor(c => c.CategoryName)
                .NotEmpty().WithMessage("Campo Obligatorio")
                .MaximumLength(100).WithMessage("Máximo 100 caracteres");

            RuleFor(c => c.Description)
                .MaximumLength(500).WithMessage("Máximo 255 caracteres")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));

            RuleFor(c => c.Picture)
                .MaximumLength(255).WithMessage("Máximo 255 caracteres")
                .When(c => !string.IsNullOrWhiteSpace(c.Picture));
        }
    }
}