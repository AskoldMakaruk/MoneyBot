using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class CategoryCommand : Command
    {
        public CategoryCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage && Message.Text.Contains("categor")) res += 2;
            return res;
        }
        public override async void Execute()
        {
            if (Message.Text == "Add categories")
            {
                Account.Status = AccountStatus.AddCategories;
                await Client.SendTextMessageAsync(Account, "Enter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", replyMarkup : Keyboards.Cancel);
                return;
            }
            if (Message.Text == "Show categories")
            {
                if (Account.Categories != null && Account.Categories.Count != 0)
                    await Client.SendTextMessageAsync(Account, $"{string.Join("\n", Account.Categories.Select(c => $"{c.Emoji} - {c.Type} - {c.Name}"))}");
                else await Client.SendTextMessageAsync(Account, $"You have no categories.");
                return;
            }
            if (Message.Text == "Override categories")
            {
                Account.Status = AccountStatus.OverrideCategories;
                await Client.SendTextMessageAsync(Account, "This will override your categories and delete attached expenses.\nEnter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", replyMarkup : Keyboards.Cancel);
                return;
            }
            Relieve();
        }
    }
}