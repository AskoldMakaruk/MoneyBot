using System.Linq;
using System.Text.RegularExpressions;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class AddCategoryCommand : Command
    {
        public AddCategoryCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.AddCategories) res++;
            return res;
        }
        public override async void Execute()
        {
            var regex = new Regex(@"\w{0,} - .{2,3} - \w{0,}");
            var values = Message.Text.Split('\n').Where(v => regex.IsMatch(v)).Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));

            var categories = values.Select(v => new ExpenseCategory()
            {
                Account = Account,
                    Emoji = v[0],
                    //TODO default type if one is missing
                    Type = v[1].ToLower().Contains("in") ? MoneyDirection.In : MoneyDirection.Out,
                    Name = v[2],

            });
            Controller.AddCategories(categories);
            Account.Status = AccountStatus.Free;
            var message = "";
            if (categories.Count() == 0)
            {
                message = "No categories were added.";
            }
            else if (categories.Count() == 1)
            {
                message = $"Category {categories.First().Name} was added.";
            }
            else
            {
                message = $"{categories.Count()} categories were added.";
            }
            await Client.SendTextMessageAsync(Account, message, replyMarkup : Keyboards.MainKeyboard(Account));
        }
    }
}