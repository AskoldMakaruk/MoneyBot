using Microsoft.EntityFrameworkCore;
using MoneyBot.DB.Model;

namespace MoneyBot.DB
{
    public class TelegramContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ExpenseCategory> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
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
            modelBuilder.Entity<Account>().HasMany(p => p.Categories).WithOne(p => p.Account).IsRequired();

            modelBuilder.Entity<ExpenseCategory>().HasMany(p => p.Expenses).WithOne(p => p.Category).IsRequired();
            modelBuilder.Entity<ExpenseCategory>().HasIndex(p => p.Emoji).IsUnique();
            modelBuilder.Entity<Person>().HasIndex(p => p.Alias).IsUnique();
        }
    }
}