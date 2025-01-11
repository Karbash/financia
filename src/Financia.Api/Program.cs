using Financia.Api.Filters;
using Financia.Api.Middleware;
using Financia.Application;
using Financia.Infrastructure;
using Financia.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

            builder.Services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddAuthentication( config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme =  JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer( config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = new TimeSpan(0),
                    IssuerSigningKey = 
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey")!))
                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<CultureMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            // Chamada para MigrateDatabase
            MigrateDatabase(app).GetAwaiter().GetResult();

            app.Run();
        }

        // Método para aplicar a migração de banco de dados
        static async Task MigrateDatabase(WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                // Garantir que o DatabaseMigration tem um método assíncrono
                await DatabaseMigration.MigrateDatabase(scope.ServiceProvider);
            }
        }
    }
}
