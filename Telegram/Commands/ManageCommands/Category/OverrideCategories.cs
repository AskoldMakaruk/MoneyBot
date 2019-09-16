using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class OverrideCategoriesCommand : Command
    {
        public OverrideCategoriesCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.OverrideCategories) res++;
            return res;
        }
        public override async void Execute()
        {
            var values = Message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));

            var categories = values.Select(v => new ExpenseCategory()
            {
                Account = Account,
                    Emoji = v[0],
                    //todo default type if one is missing
                    Type = v[1].ToLower().Contains("in") ? MoneyDirection.In : MoneyDirection.Out,
                    Name = v[2],

            });
            //categories to be saved
            var a = Account.Categories.Where(c => categories.FirstOrDefault(e => e.Name == c.Name && e.Emoji == c.Emoji) != null);

            var s = a.Union(categories.Where(c => a.FirstOrDefault(e => e.Name == c.Name && e.Emoji == c.Emoji) == null)).ToList();
            Account.Categories = s;
            Controller.SaveChanges();
            Account.Status = AccountStatus.Free;
            await Client.SendTextMessageAsync(Account.ChatId, "Categories overrided", replyMarkup : Keyboards.Main);
        }
    }
}