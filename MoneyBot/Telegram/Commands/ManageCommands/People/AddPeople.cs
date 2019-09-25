using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class AddPeopleCommand : Command
    {
        public AddPeopleCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.AddPeople) res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
            var people = values.Select(v => new Person()
            {
                Account = account,
                    Alias = v[0],
                    Name = v[1]
            });
            account.Controller.AddPeople(people);
            account.Status = AccountStatus.Free;
            return new Response(account, "People added", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}