using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class MangeCommand : Command
    {
        public MangeCommand(Message message, Account Account) : base(message, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage) res++;
            return res;
        }
        public override OutMessage Execute()
        {
            return Relieve();
        }
    }
}