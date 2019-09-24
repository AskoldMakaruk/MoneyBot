using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class MangeCommand : Command
    {
        public MangeCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.Manage) res++;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            return Relieve(message, account);
        }
    }
}