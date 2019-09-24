using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public abstract class Query
    {
        public TelegramController Controller { get; set; }

        public abstract Response Execute(CallbackQuery message, Account account);
        public abstract bool IsSuitable(CallbackQuery message, Account account);
    }
}