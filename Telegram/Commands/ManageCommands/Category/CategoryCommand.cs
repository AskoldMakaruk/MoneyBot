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
            if (Account.Status == AccountStatus.Manage) res++;
            if (Message.Text.Contains("categor")) res++;
            return res;
        }
        public override async void Execute()
        {
            if (Message.Text == "Add categories")
            {
                Account.Status = AccountStatus.AddCategories;
                await Client.SendTextMessageAsync(Account.ChatId, "Enter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", replyMarkup : Keyboards.Cancel);
                return;
            }
            if (Message.Text == "Show categories")
            {
                await Client.SendTextMessageAsync(Account.ChatId, $"{string.Join("\n", Account.Categories.Select(c => $"{c.Emoji} - {c.Type} - {c.Name}"))}");
                return;
            }
            Relieve();
        }
    }
}