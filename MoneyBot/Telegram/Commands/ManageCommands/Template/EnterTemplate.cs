using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterTemplateCommand : Command
    {
        public EnterTemplateCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.EnterTemplate) res++;
            return res;
        }
        public override OutMessage Execute(Message message, Account account)
        {
            var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));

            var templates = values.Select(v => new Template()
            {
                Name = v[0],
                    Sum = v[1].ParseSum(),
                    Category = account.CurrentTemplate.Category

            });
            account.Controller.AddTemplates(templates);
            account.Status = AccountStatus.Free;
            return new OutMessage(account, "Template added", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}