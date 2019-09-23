using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class TemplateCommand : Command
    {
        public TemplateCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Message.Text.ToLower().Contains("templat") && Account.Status == AccountStatus.Manage) res += 2;
            return res;
        }
        public override OutMessage Execute()
        {
            if (Message.Text == "Add template")
            {
                Account.CurrentTemplate = new Template();
                return new OutMessage(Account, "Select category for new template:", replyMarkup : Keyboards.Categories(Account.Categories, "AddTemplate"));
            }
            return Relieve();
        }
    }
}