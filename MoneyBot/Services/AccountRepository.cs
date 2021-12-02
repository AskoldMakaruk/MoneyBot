using System.Linq;
using System.Threading.Tasks;
using BotFramework.Middleware;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Services
{
    public class AccountRepository : IUserRepository<User>
    {
        private readonly TelegramContext _context;

        public AccountRepository(TelegramContext context)
        {
            _context = context;
        }

        public User FromId(int id) => _context.Accounts.Find(id);

        public async Task<User> FromMessage(Message message)
        {
            var account = _context.Accounts.Include(a => a.Categories)
                .Include(a => a.Frens)
                .Include("Categories.Expenses")
                .Include("Categories.Templates")
                .Include("People.Transactions")
                .FirstOrDefault(a => a.ChatId == message.Chat.Id);

            if (account == null)
            {
                account = await CreateUser(message.From);
            }

            return account;
        }


        internal void RemoveAccount(User user)
        {
            _context.Accounts.Remove(user);
            _context.SaveChanges();
        }

        public async Task<User?> GetUser(long userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.ChatId == userId);
        }

        public async Task<User> CreateUser(global::Telegram.Bot.Types.User user)
        {
            var account = new User
            {
                ChatId = user.Id,
                Name = user.Username
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