using System.Linq;
using System.Threading.Tasks;
using BotFramework.Middleware;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Services
{
    public class AccountRepository : IUserRepository<Account>
    {
        private readonly TelegramContext _context;

        public AccountRepository(TelegramContext context)
        {
            _context = context;
        }

        public Account FromId(int id) => _context.Accounts.Find(id);

        public async Task<Account> FromMessage(Message message)
        {
            var account = _context.Accounts.Include(a => a.Categories)
                .Include(a => a.People)
                .Include("Categories.Expenses")
                .Include("Categories.Templates")
                .Include("People.Transactions")
                .FirstOrDefault(a => a.ChatId == message.Chat.Id);

            if (account == null)
            {
                account = await CreateUser(message.From);
            }
            else
            {
                account.Status = AccountStatus.Free;
            }

            return account;
        }


        internal void RemoveAccount(Account account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public async Task<Account?> GetUser(long userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.ChatId == userId);
        }

        public async Task<Account> CreateUser(User user)
        {
            var account = new Account
            {
                ChatId = user.Id,
                Name = user.Username,
                Status = AccountStatus.Start,
            };
            if (user.Username == null)
            {
                account.Name = user.FirstName + " " + user.LastName;
            }

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }
    }
}