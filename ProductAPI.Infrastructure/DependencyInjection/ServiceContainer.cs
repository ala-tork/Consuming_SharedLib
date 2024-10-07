using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.Interfaces;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;


namespace ProductAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastrucutreServices(this IServiceCollection services,IConfiguration config)
        {
            // Add Database Connectivity && Authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            // services DI
            services.AddScoped<IProduct, ProductRepository>();
            return services;

        }

        public static IApplicationBuilder UseIfrastructurePolicy(this IApplicationBuilder app)
        {
            // register Global middleware :
            // Global exception : handels exceptions errors
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
