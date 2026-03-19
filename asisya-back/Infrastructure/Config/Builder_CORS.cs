namespace Infrastructure.Config
{
    public static class Builder_CORS
    {
        public static IServiceCollection AddCORSService(this IServiceCollection services, IConfiguration configuration)
        {
            var frontUrls = configuration["FrontApp:Url"]!.Split('|', StringSplitOptions.RemoveEmptyEntries);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFront",
                    policy => policy.WithOrigins(frontUrls)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });

            return services;
        }
    }
}
