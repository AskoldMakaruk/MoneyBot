using MoneyBot.DB.Model;
using StickerMemeBot.Telegram.Commands;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class CategoryCommand : Command
    {
        public CategoryCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage) res++;
            if (Message.Text.Contains("categor")) res++;
            return res;
        }
        public override async void Execute()
        {
            if (Message.Text == "Add category")
            {
                Account.Status = AccountStatus.AddCategory;
                await Client.SendTextMessageAsync(Account.ChatId, "Enter new category in format:\n[emoji] - [category name]");
                return;
            }
            if (Message.Text == "Edit category")
            {
                Account.Status = AccountStatus.EditCategory;
                await Client.SendTextMessageAsync(Account.ChatId, "Select category to edit:", replyMarkup : Keyboards.Categories(Controller.GetCategories(Account.Id), "Edit"));

                return;
            }
        }
    }
}