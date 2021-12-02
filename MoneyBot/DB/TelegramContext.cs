using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;

namespace MoneyBot.DB
{
    public class TelegramContext : DbContext
    {
        public DbSet<User> Accounts { get; set; }
        public DbSet<ExpenseCategory> Categories { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        // public DbSet<Fund> Funds { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public TelegramContext()
        {
            Start();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database1.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.Frens)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<ExpenseCategory>(e =>
            {
                e.HasOne<User>().WithMany(a => a.Categories).HasForeignKey(a => a.UserId);
                e.HasMany(p => p.Expenses).WithOne(p => p.Category).IsRequired();
                e.HasIndex(p => new { p.Emoji, p.UserId }).IsUnique();
            });

            modelBuilder.Entity<Person>(builder => { builder.HasIndex(p => p.Alias).IsUnique(); });
        }

        public static bool First = true;


        public void Start()
        {
            if (First)
            {
                First = false;
            }

            Database.EnsureCreated();
        }

        internal void DeleteDb()
        {
            try
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
            catch
            {
            }
        }

        #region Categories

        public virtual void AddCategories(IEnumerable<ExpenseCategory> categories)
        {
            Categories.AddRange(categories);
            SaveChanges();
        }

        public ExpenseCategory[] GetCategories(int accountId)
        {
            return Categories.Where(c => c.User.Id == accountId).ToArray();
        }

        #endregion

        #region Expenses

        public virtual void AddExpense(Expense expense)
        {
            Expenses.Add(expense);
            SaveChanges();
        }

        public virtual void AddExpense(int templateId)
        {
            var template = Templates.Include(q => q.Category).First(t => t.Id == templateId);
            Expenses.Add(new Expense
            {
                Description = template.Name,
                Category = template.Category,
                Date = DateTime.Now,
                Sum = template.Sum
            });
            SaveChanges();
        }

        #endregion

        internal void AddTemplates(IEnumerable<Template> templates)
        {
            Templates.AddRange(templates);
            SaveChanges();
        }

        internal Stats GetStats(int accountId)
        {
            return new Stats()
            {
                Categories = Categories.Include(c => c.Expenses).Where(c => c.User.Id == accountId).ToArray(),
                People = People.Include(c => c.Transactions).Where(c => c.User.Id == accountId).ToArray(),
            };
        }


        internal void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
            SaveChanges();
        }

        public void SaveChanges() => SaveChanges();
    }
}