using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class PersonCommand : Command
    {
        public PersonCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage && (Message.Text.Contains("person") || Message.Text.Contains("people"))) res += 2;
            return res;
        }
        public override async void Execute()
        {
            if (Message.Text == "Add people")
            {
                Account.Status = AccountStatus.AddPeople;
                await Client.SendTextMessageAsync(Account.ChatId, $"Enter new people in format:\n" +
                    "[alias] [name]", replyMarkup : Keyboards.Cancel);
                return;
            }
            if (Message.Text == "Show people")
            {
                if (Account.People != null && Account.People.Count != 0)
                    await Client.SendTextMessageAsync(Account.ChatId, $"{string.Join("\n", Account.People.Select(c => $"{c.Alias} - {c.Name}"))}");
                else await Client.SendTextMessageAsync(Account.ChatId, $"You have no people.");
                return;
            }
            if (Message.Text == "Override people")
            {
                Account.Status = AccountStatus.OverridePeople;
                await Client.SendTextMessageAsync(Account.ChatId, "This will override your people and delete attached transactions.\nEnter new people in format:\n" +
                    "[alias] [name]", replyMarkup : Keyboards.Cancel);
                return;
            }
            Relieve();
        }
    }
}