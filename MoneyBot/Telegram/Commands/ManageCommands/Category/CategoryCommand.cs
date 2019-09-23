using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class CategoryCommand : Command
    {
        public CategoryCommand(Message message, Account Account) : base(message, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Manage && Message.Text.Contains("categor")) res += 2;
            return res;
        }
        public override OutMessage Execute()
        {
            if (Message.Text == "Add categories")
            {
                Account.Status = AccountStatus.AddCategories;
                return new OutMessage(Account, "Enter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", Keyboards.Cancel);
            }
            if (Message.Text == "Show categories")
            {
                if (Account.Categories != null && Account.Categories.Count != 0)
                    return new OutMessage(Account, $"{string.Join("\n", Account.Categories.Select(c => $"{c.Emoji} - {c.Type} - {c.Name}"))}");
                else return new OutMessage(Account, $"You have no categories.");
            }
            if (Message.Text == "Override categories")
            {
                Account.Status = AccountStatus.OverrideCategories;
                return new OutMessage(Account, "This will override your categories and delete attached expenses.\nEnter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", replyMarkup : Keyboards.Cancel);
            }
            return Relieve();
        }
    }
}