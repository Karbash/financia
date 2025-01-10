
using Financia.Domain.Repositories;
using Financia.Domain.Repositories.Expenses;
using Financia.Domain.Repositories.User;
using Financia.Domain.Security.Cryptography;
using Financia.Infrastructure.DataAccess;
using Financia.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financia.Infrastructure
{
    public static class DependenceInjectionExtension
    {
       public static void AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        { 
            AddDbContext(services, configuration);
            AddRepositories(services);

            services.AddScoped<IPasswordEncrypter, Security.BCrypt>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            var version = new Version(8, 0, 4);
            var serverVersion = new MySqlServerVersion(version);

            services.AddDbContext<FinanciaDBContext>(config => config.UseMySql(connectionString, serverVersion));
        }
    }
}
