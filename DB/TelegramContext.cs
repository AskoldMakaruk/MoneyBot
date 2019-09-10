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
            //modelBuilder.Entity<Account>().HasMany(p => p.Expenses);
            modelBuilder.Entity<Account>().HasMany(p => p.Categories).WithOne(p => p.Account).IsRequired();

            // modelBuilder.Entity<Expense>().HasOne(p => p.Category);
            // modelBuilder.Entity<Expense>().HasOne(p => p.Account);

            modelBuilder.Entity<ExpenseCategory>().HasMany(p => p.Expenses).WithOne(p => p.Category).IsRequired();

            //            modelBuilder.Entity<Template>().HasOne(p => p.Account);
            //      modelBuilder.Entity<Template>().HasOne(p => p.Category);

            //          modelBuilder.Entity<Transaction>().HasOne(p => p.Account);
            //        modelBuilder.Entity<Transaction>().HasOne(p => p.Person);
        }
    }
}