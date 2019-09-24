using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class OverridePeopleCommand : Command
    {
        public OverridePeopleCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.OverridePeople) res++;
            return res;
        }
        public override OutMessage Execute(Message message, Account account)
        {
            var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
            var people = values.Select(v => new Person()
            {
                Account = account,
                    Alias = v[0],
                    Name = v[1]
            });
            //categories to be saved
            var saved = account.People.Where(c => people.FirstOrDefault(e => e.Name == c.Name && e.Alias == c.Alias) != null);

            //to save records about people that haven't changed
            account.People = saved.Union(people.Where(c => saved.FirstOrDefault(e => e.Name == c.Name && e.Alias == c.Alias) == null)).ToList();

            account.Status = AccountStatus.Free;
            return new OutMessage(account, "People added", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}