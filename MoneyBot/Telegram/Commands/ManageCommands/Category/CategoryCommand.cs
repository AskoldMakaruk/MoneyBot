using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class CategoryCommand : Command
    {
        public CategoryCommand() : base() { }

        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.Manage && message.Text.Contains("categor")) res += 2;
            return res;
        }
        public override OutMessage Execute(Message message, Account account)
        {
            if (message.Text == "Add categories")
            {
                account.Status = AccountStatus.AddCategories;
                return new OutMessage(account, "Enter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", Keyboards.Cancel);
            }
            if (message.Text == "Show categories")
            {
                if (account.Categories != null && account.Categories.Count != 0)
                    return new OutMessage(account, $"{string.Join("\n", account.Categories.Select(c => $"{c.Emoji} - {c.Type} - {c.Name}"))}");
                else return new OutMessage(account, $"You have no categories.");
            }
            if (message.Text == "Override categories")
            {
                account.Status = AccountStatus.OverrideCategories;
                return new OutMessage(account, "This will override your categories and delete attached expenses.\nEnter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", replyMarkup : Keyboards.Cancel);
            }
            return Relieve(message, account);
        }
    }
}