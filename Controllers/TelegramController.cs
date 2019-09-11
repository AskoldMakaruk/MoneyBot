using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB;
using MoneyBot.DB.Model;
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

        public Account FromMessage(Message message)
        {
            if (Accounts.ContainsKey(message.Chat.Id))
            {
                return Accounts[message.Chat.Id];
            }
            Account account = Context.Accounts.Include(a => a.Categories)
                .Include(a => a.People)
                .Include("Categories.Expenses")
                .Include("Categories.Templates")
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
        public Account FromMessage(Chat chat)
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

        #endregion

        #region Categories
        public void AddCategories(IEnumerable<ExpenseCategory> categories)
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
        public void AddExpense(Expense expense)
        {
            Context.Expenses.Add(expense);
            SaveChanges();
        }
        internal void AddExpense(int templateId)
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

        public void SaveChanges() => Context.SaveChanges();
    }
}