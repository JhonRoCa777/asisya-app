namespace Infrastructure.Config
{
    public static class Builder_Controller
    {
        public static IServiceCollection AddControllerService(this IServiceCollection services)
        {
            // Agrega Controllers Ignorando Referencias Circulares
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            return services;
        }
    }
}
