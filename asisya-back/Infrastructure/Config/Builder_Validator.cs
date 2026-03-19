using FluentValidation;
using Infrastructure.Validators;

namespace Infrastructure.Config
{
    public static class Builder_Validator
    {
        public static IServiceCollection AddValidatorService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<EmployeeLoginValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryRequestValidator>();
            return services;
        }
    }
}
