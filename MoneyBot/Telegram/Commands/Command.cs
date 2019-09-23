using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public abstract class Command
    {
        public Command(Message message, Bot client, Account account)
        {
            Message = message;
            Client = client;
            Account = account;
        }
        public TelegramController Controller { get; set; }
        public Message Message { get; }
        public Bot Client { get; }
        public Account Account { get; }

        // 0 not at all
        // 1 just handle reply
        // 2 main condition is true
        // 3 role staff
        // 4 high priority
        public abstract int Suitability();
        public abstract OutMessage Execute();
        public virtual bool Canceled()
        {
            return Message.Text.ToLower().Equals("cancel") ||
                Message.Text.ToLower().Equals("/cancel");
        }
        public virtual OutMessage Relieve()
        {
            return new MainCommand(Message, Client, Account).Execute();
        }
    }
}