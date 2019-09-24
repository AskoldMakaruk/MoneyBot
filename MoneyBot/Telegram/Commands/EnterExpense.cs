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
        public override OutMessage Execute(Message message, Account account)
        {
            var values = message.Text.TrySplit('-', ' ');
            var sum = values[1].ParseSum();
            if (sum != -1)
            {
                account.CurrentExpense.Description = values[0];
                account.CurrentExpense.Sum = sum;
                account.CurrentExpense.Date = DateTime.Now;
                account.Controller.AddExpense(account.CurrentExpense);
                account.Status = AccountStatus.Free;
                return new OutMessage(account, $"Success!", replyMarkup : Keyboards.MainKeyboard(account));

            }
            return new OutMessage(account, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
        public override OutMessage Relieve(Message message, Account account)
        {
            account.Status = AccountStatus.Free;
            return new OutMessage(account, $"You shall be freed", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}