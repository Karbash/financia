using Financia.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Financia.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public static async Task MigrateDatabase(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<FinanciaDBContext>();

            // Aplica as migrações pendentes no banco de dados
            await dbContext.Database.MigrateAsync();
        }
    }
}
