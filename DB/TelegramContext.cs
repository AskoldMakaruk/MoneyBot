using Microsoft.EntityFrameworkCore;
using MoneyBot.DB.Model;

namespace MoneyBot.DB
{
    public class TelegramContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ExspenseCategory> Categories { get; set; }
        public DbSet<Exspense> Exspenses { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasMany(p => p.Exspenses);
            modelBuilder.Entity<Account>().HasMany(p => p.Categories);
            modelBuilder.Entity<Exspense>().HasOne(p => p.Category);
        }
    }
}