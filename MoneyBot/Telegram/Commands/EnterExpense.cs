using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterExpenseCommand : Command
    {
        public EnterExpenseCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.EnterExpenseSum) res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            var text = message.Text;

            var sum = -1.0;
            var description = "";

            var success = false;

            if (text.Contains('-'))
            {
                var values = message.Text.TrySplit('-');
                if (values.Length == 2)
                {
                    success = true;
                    description = values[0];
                    sum = values[1].ParseSum();
                }
            }
            else
            {
                success = double.TryParse(text.Trim(), out sum);
            }

            if (success && account.CurrentExpense != null)
            {
                account.CurrentExpense.Description = description;
                account.CurrentExpense.Sum = sum;

                account.CurrentExpense.Date = DateTime.Now;
                account.Controller.AddExpense(account.CurrentExpense);
                account.Status = AccountStatus.Free;
                return new Response(account, $"Success!", replyMarkup : Keyboards.MainKeyboard(account));

            }
            return new Response(account, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
        public override Response Relieve(Message message, Account account)
        {
            account.Status = AccountStatus.Free;
            return new Response(account, $"You shall be freed", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}