using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class TemplateCommand : Command
    {
        public TemplateCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text.ToLower().Contains("templat") && account.Status == AccountStatus.Manage) res += 2;
            return res;
        }
        public override OutMessage Execute(Message message, Account account)
        {
            if (message.Text == "Add templates")
            {
                account.CurrentTemplate = new Template();
                return new OutMessage(account, "Select category for new template:", replyMarkup : Keyboards.Categories(account.Categories, "AddTemplate"));
            }
            return Relieve(message, account);
        }
    }
}