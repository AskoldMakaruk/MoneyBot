using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterTemplateCommand : Command
    {
        public EnterTemplateCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterTemplate) res++;
            return res;
        }
        public override OutMessage Execute()
        {
            var values = Message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));

            var templates = values.Select(v => new Template()
            {
                Name = v[0],
                    Sum = v[1].ParseSum(),
                    Category = Account.CurrentTemplate.Category

            });
            Controller.AddTemplates(templates);
            Account.Status = AccountStatus.Free;
            return new OutMessage(Account, "Template added", replyMarkup : Keyboards.MainKeyboard(Account));
        }
    }
}