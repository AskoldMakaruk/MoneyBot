using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class OverrideCategoriesCommand : Command
    {
        public OverrideCategoriesCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.OverrideCategories) res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));

            var categories = values.Select(v => new ExpenseCategory()
            {
                Account = account,
                    Emoji = v[0],
                    //TODO default type if one is missing
                    Type = v[1].ToLower().Contains("in") ? MoneyDirection.In : MoneyDirection.Out,
                    Name = v[2],

            });
            //categories to be saved
            var saved = account.Categories.Where(c => categories.FirstOrDefault(e => e.Name == c.Name && e.Emoji == c.Emoji) != null);
            //to save records about categories that haven't changed
            account.Categories = saved.Union(categories.Where(c => saved.FirstOrDefault(e => e.Name == c.Name && e.Emoji == c.Emoji) == null)).ToList();

            account.Controller.SaveChanges();
            account.Status = AccountStatus.Free;
            return new Response(account, "Categories overrided", Keyboards.MainKeyboard(account));
        }
    }
}