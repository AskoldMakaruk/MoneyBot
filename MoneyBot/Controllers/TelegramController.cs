using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;

namespace MoneyBot.Controllers
{
    public class TelegramController
    {
        public static bool First = true;

        private TelegramContext Context;
        public void Start()
        {
            Context = new TelegramContext();
            if (First)
            {
                //Context.Database.EnsureDeleted();
                First = false;
            }
            Context.Database.EnsureCreated();
        }
        internal void DeleteDb()
        {
            try
            {
                Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();
                // Context.Accounts.AddRange(Accounts.Values);
                Accounts = new Dictionary<long, Account>();
            }
            catch { }
        }
        #region Account

        public static Dictionary<long, Account> Accounts = new Dictionary<long, Account>();
        public Account FromId(int id)
        {

            var account = Accounts.Values.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                account = Context.Accounts.Find(id);
                Accounts.Add(account.ChatId, account);
            }
            return account;
        }

        public virtual Account FromMessage(Message message)
        {
            if (Accounts.ContainsKey(message.Chat.Id))
            {
                return Accounts[message.Chat.Id];
            }
            Account account = Context.Accounts.Include(a => a.Categories)
                .Include(a => a.People)
                .Include("Categories.Expenses")
                .Include("Categories.Templates")
                .Include("People.Transactions")
                .FirstOrDefault(a => a.ChatId == message.Chat.Id);

            if (account == null)
            {
                account = CreateAccount(message);
            }
            else
                account.Status = AccountStatus.Free;

            if (!Accounts.ContainsKey(account.ChatId))
                Accounts.Add(account.ChatId, account);

            return account;
        }

        public virtual Account FromMessage(Chat chat)
        {
            if (Accounts.ContainsKey(chat.Id))
            {
                return Accounts[chat.Id];
            }
            return Context.Accounts.FirstOrDefault(a => a.ChatId == chat.Id);
        }
        private Account CreateAccount(Message message)
        {
            var account = new Account
            {
                ChatId = message.Chat.Id,
                Name = message.Chat.Username,
                Status = AccountStatus.Start,
            };
            if (message.Chat.Username == null)
                account.Name = message.Chat.FirstName + " " + message.Chat.LastName;
            Context.Accounts.Add(account);
            Context.SaveChanges();
            return account;
        }

        internal void RemoveAccount(Account account)
        {
            Context.Accounts.Remove(account);
            Accounts.Remove(account.ChatId);
            SaveChanges();
        }

        #endregion

        #region Categories
        public virtual void AddCategories(IEnumerable<ExpenseCategory> categories)
        {
            Context.Categories.AddRange(categories);
            SaveChanges();
        }
        public ExpenseCategory[] GetCategories(int accountId)
        {
            return Context.Categories.Where(c => c.Account.Id == accountId).ToArray();
        }
        #endregion

        #region Expenses        
        public virtual void AddExpense(Expense expense)
        {
            Context.Expenses.Add(expense);
            SaveChanges();
        }
        public virtual void AddExpense(int templateId)
        {
            var template = Context.Templates.Include(q => q.Category).First(t => t.Id == templateId);
            Context.Expenses.Add(new Expense
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
            Context.Templates.AddRange(templates);
            SaveChanges();
        }

        internal Stats GetStats(int accountId)
        {
            return new Stats()
            {
                Categories = Context.Categories.Include(c => c.Expenses).Where(c => c.Account.Id == accountId).ToArray(),
                    People = Context.People.Include(c => c.Transactions).Where(c => c.Account.Id == accountId).ToArray(),
            };
        }
        internal void AddPeople(IEnumerable<Person> people)
        {
            Context.People.AddRange(people);
            SaveChanges();
        }
        internal void AddTransaction(Transaction transaction)
        {
            Context.Transactions.Add(transaction);
            SaveChanges();
        }

        public void SaveChanges() => Context.SaveChanges();
    }
}