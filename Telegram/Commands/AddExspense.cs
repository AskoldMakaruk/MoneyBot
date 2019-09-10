using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class AddExspenseCommand : Command
    {
        public AddExspenseCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Message.Text == "Add exspense") res += 2;
            if (Account.Status == AccountStatus.Free) res++;
            return res;
        }
        public override async void Execute()
        {
            var categories = Controller.GetCategories(Account.Id);
            var keyboard = Keyboards.Categories(categories, "AddExspense");
            await Client.SendTextMessageAsync(Account.ChatId, $"Select exspense category:", replyMarkup : keyboard);
        }
    }
}