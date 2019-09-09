using MoneyBot.DB.Model;
using StickerMemeBot.Telegram.Commands;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ShowCategoriesCommand : Command
    {
        public ShowCategoriesCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Message.Text == "Show categories") res += 2;
            return res;
        }
        public override async void Execute()
        {
            var categories = Controller.GetCategories(Account.Id);
            await Client.SendTextMessageAsync(Account.ChatId, $"You have {categories.Length} categories.", replyMarkup : Keyboards.Categories(categories, "Show"));
        }
    }
}