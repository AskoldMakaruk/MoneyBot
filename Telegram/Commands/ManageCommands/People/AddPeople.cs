using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class AddPeopleCommand : Command
    {
        public AddPeopleCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.AddPeople) res++;
            return res;
        }
        public override async void Execute()
        {
            var values = Message.Text.Split("\n").Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
            var people = values.Select(v => new Person()
            {
                Account = Account,
                    Alias = v[0],
                    Name = v[1]
            });
            Controller.AddPeople(people);
            Account.Status = AccountStatus.Free;
            await Client.SendTextMessageAsync(Account.ChatId, "People added", replyMarkup : Keyboards.Main);
        }
    }
}