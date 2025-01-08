
using Financia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financia.Infrastructure.DataAccess
{
    internal class FinanciaDBContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        public FinanciaDBContext(DbContextOptions options) : base(options){}
    }
}
