using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class MangeCommand : Command
    {
        public MangeCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage) res++;
            return res;
        }
        public override void Execute()
        {
            Relieve();
        }
    }
}