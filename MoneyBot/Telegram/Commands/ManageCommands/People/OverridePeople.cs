using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class OverridePeopleCommand : Command
    {
        public OverridePeopleCommand(Message message, Account Account) : base(message, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.OverridePeople) res++;
            return res;
        }
        public override OutMessage Execute()
        {
            var values = Message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
            var people = values.Select(v => new Person()
            {
                Account = Account,
                    Alias = v[0],
                    Name = v[1]
            });
            //categories to be saved
            var saved = Account.People.Where(c => people.FirstOrDefault(e => e.Name == c.Name && e.Alias == c.Alias) != null);

            //to save records about people that haven't changed
            Account.People = saved.Union(people.Where(c => saved.FirstOrDefault(e => e.Name == c.Name && e.Alias == c.Alias) == null)).ToList();

            Account.Status = AccountStatus.Free;
            return new OutMessage(Account, "People added", replyMarkup : Keyboards.MainKeyboard(Account));
        }
    }
}