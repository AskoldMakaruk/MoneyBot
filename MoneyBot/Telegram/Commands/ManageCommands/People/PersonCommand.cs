using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class PersonCommand : Command
    {
        public PersonCommand(Message message, Account Account) : base(message, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage && (Message.Text.Contains("person") || Message.Text.Contains("people"))) res += 2;
            return res;
        }
        public override OutMessage Execute()
        {
            if (Message.Text == "Add people")
            {
                Account.Status = AccountStatus.AddPeople;
                return new OutMessage(Account, $"Enter new people in format:\n" +
                    "[alias] [name]", replyMarkup : Keyboards.Cancel);
            }
            if (Message.Text == "Show people")
            {
                if (Account.People != null && Account.People.Count != 0)
                    return new OutMessage(Account, $"{string.Join("\n", Account.People.Select(c => $"{c.Alias} - {c.Name}"))}");
                else return new OutMessage(Account, $"You have no people.");
            }
            if (Message.Text == "Override people")
            {
                Account.Status = AccountStatus.OverridePeople;
                return new OutMessage(Account, "This will override your people and delete attached transactions.\nEnter new people in format:\n" +
                    "[alias] [name]", replyMarkup : Keyboards.Cancel);
            }
            return Relieve();
        }
    }
}