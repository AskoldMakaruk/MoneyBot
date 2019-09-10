using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterExspenseSumCommand : Command
    {
        public EnterExspenseSumCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterExspenseSum) res += 2;
            if (Message.Text != null) res++;
            return res;
        }
        public override async void Execute()
        {

        }
    }
}