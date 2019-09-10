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
            if (Account.Status == AccountStatus.AddCategory) res += 2;
            if (Message.Text != null) res++;
            return res;
        }
        public override async void Execute()
        {
            var text = Message.Text;
            var emoji = text.Substring(0, text.IndexOf('-') - 1);
            var name = text.Substring(text.IndexOf('-') + 1);
            var category = new ExspenseCategory()
            {
                Account = Account,
                Emoji = emoji,
                Name = name
            };
            Controller.AddCategory(category);
            Account.Status = AccountStatus.Free;
            await Client.SendTextMessageAsync(Account.ChatId, "Category added", replyMarkup : Keyboards.Main);
        }
    }
}