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
    public class AccountRepository
    {
        private readonly TelegramContext _context;

        public AccountRepository(TelegramContext context)
        {
            _context = context;
        }

        public Account FromId(int id) => _context.Accounts.Find(id);

        public virtual Account FromMessage(Chat chat) => _context.Accounts.FirstOrDefault(a => a.ChatId == chat.Id);

        public virtual Account FromMessage(Message message)
        {
            var account = _context.Accounts.Include(a => a.Categories)
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
            {
                account.Status = AccountStatus.Free;
            }

            return account;
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
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account;
        }

        internal void RemoveAccount(Account account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }
    }
}