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
            if (Account.Status == AccountStatus.AddCategories) res += 2;
            if (Message.Text != null) res++;
            return res;
        }
        public override async void Execute()
        {
            var values = Message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));

            var categories = values.Select(v => new ExpenseCategory()
            {
                Account = Account,
                    Emoji = v[0],
                    Name = v[1]
            });
            Controller.AddCategories(categories);
            Account.Status = AccountStatus.Free;
            await Client.SendTextMessageAsync(Account.ChatId, "Category added", replyMarkup : Keyboards.Main);
        }
    }
}