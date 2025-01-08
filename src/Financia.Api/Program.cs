
using Financia.Api.Filters;
using Financia.Api.Middleware;
using Financia.Application;
using Financia.Infrastructure;

namespace Financia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRouting(option => option.LowercaseUrls = true);
            builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<CultureMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
