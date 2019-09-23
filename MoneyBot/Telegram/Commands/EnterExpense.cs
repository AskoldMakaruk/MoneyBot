using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterExpenseCommand : Command
    {
        public EnterExpenseCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterExpenseSum) res += 2;
            return res;
        }
        public override OutMessage Execute()
        {
            var values = Message.Text.TrySplit('-', ' ');
            var sum = values[1].ParseSum();
            if (sum != -1)
            {
                Account.CurrentExpense.Description = values[0];
                Account.CurrentExpense.Sum = sum;
                Account.CurrentExpense.Date = DateTime.Now;
                Controller.AddExpense(Account.CurrentExpense);
                Account.Status = AccountStatus.Free;
                return new OutMessage(Account, $"Success!", replyMarkup : Keyboards.MainKeyboard(Account));

            }
            return new OutMessage(Account, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
        public override OutMessage Relieve()
        {
            Account.Status = AccountStatus.Free;
            return new OutMessage(Account, $"You shall be freed", replyMarkup : Keyboards.MainKeyboard(Account));
        }
    }
}