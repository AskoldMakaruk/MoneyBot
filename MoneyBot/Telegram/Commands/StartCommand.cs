using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class StartCommand : Command
    {
        public StartCommand() : base() { }

        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.Start) res += 2;
            if (message.Text.StartsWith("/start")) res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            account.Status = AccountStatus.Free;
            return new Response(account, "Welcome to MoneyBot.", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}