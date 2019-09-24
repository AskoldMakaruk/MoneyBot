using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class PersonCommand : Command
    {
        public PersonCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.Manage && (message.Text.Contains("person") || message.Text.Contains("people"))) res += 2;
            return res;
        }
        public override OutMessage Execute(Message message, Account account)
        {
            if (message.Text == "Add people")
            {
                account.Status = AccountStatus.AddPeople;
                return new OutMessage(account, $"Enter new people in format:\n" +
                    "[alias] [name]", replyMarkup : Keyboards.Cancel);
            }
            if (message.Text == "Show people")
            {
                if (account.People != null && account.People.Count != 0)
                    return new OutMessage(account, $"{string.Join("\n", account.People.Select(c => $"{c.Alias} - {c.Name}"))}");
                else return new OutMessage(account, $"You have no people.");
            }
            if (message.Text == "Override people")
            {
                account.Status = AccountStatus.OverridePeople;
                return new OutMessage(account, "This will override your people and delete attached transactions.\nEnter new people in format:\n" +
                    "[alias] [name]", replyMarkup : Keyboards.Cancel);
            }
            return Relieve(message, account);
        }
    }
}