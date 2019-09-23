using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public abstract class Query
    {
        public Query(CallbackQuery message, Account account)
        {
            Message = message;
            Account = account;
        }
        public TelegramController Controller { get; set; }
        public CallbackQuery Message { get; }
        public Account Account { get; }

        public abstract OutMessage Execute();
        public abstract bool IsSuitable();
    }
}