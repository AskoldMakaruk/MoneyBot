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
            if (Account.Status == AccountStatus.Manage) res++;
            if (Message.Text.Contains("person") || Message.Text.Contains("people")) res++;
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
                await Client.SendTextMessageAsync(Account.ChatId, $"{string.Join("\n", Account.People.Select(c => $"{c.Alias} - {c.Name}"))}");
                return;
            }
            Relieve();
        }
    }
}