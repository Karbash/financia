
using Financia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financia.Infrastructure.DataAccess
{
    internal class FinanciaDBContext : DbContext
    {
        public FinanciaDBContext(DbContextOptions options) : base(options){}
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tag>().ToTable("Tags");
        }
    }
}
