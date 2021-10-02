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
        
        public static bool First = true;

        private TelegramContext _context;

        public void Start()
        {
            _context = new TelegramContext();
            if (First)
            {
                First = false;
            }

            _context.Database.EnsureCreated();
        }

        internal void DeleteDb()
        {
            try
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }
            catch
            {
            }
        }

        #region Categories

        public virtual void AddCategories(IEnumerable<ExpenseCategory> categories)
        {
            _context.Categories.AddRange(categories);
            SaveChanges();
        }

        public ExpenseCategory[] GetCategories(int accountId)
        {
            return _context.Categories.Where(c => c.Account.Id == accountId).ToArray();
        }

        #endregion

        #region Expenses

        public virtual void AddExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            SaveChanges();
        }

        public virtual void AddExpense(int templateId)
        {
            var template = _context.Templates.Include(q => q.Category).First(t => t.Id == templateId);
            _context.Expenses.Add(new Expense
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
            _context.Templates.AddRange(templates);
            SaveChanges();
        }

        internal Stats GetStats(int accountId)
        {
            return new Stats()
            {
                Categories = _context.Categories.Include(c => c.Expenses).Where(c => c.Account.Id == accountId).ToArray(),
                People = _context.People.Include(c => c.Transactions).Where(c => c.Account.Id == accountId).ToArray(),
            };
        }

        internal void AddPeople(IEnumerable<Person> people)
        {
            _context.People.AddRange(people);
            SaveChanges();
        }

        internal void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            SaveChanges();
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}