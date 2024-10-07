using ProductAPI.Infrastructure.DependencyInjection;
namespace ProductAPI.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // add Infrastrucute DI
            builder.Services.AddInfrastrucutreServices(builder.Configuration);


            var app = builder.Build();

            // use the infrastructure policy
            app.UseIfrastructurePolicy();
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
