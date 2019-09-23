using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterTransactionCommand : Command
    {
        public EnterTransactionCommand(Message message, Account Account) : base(message, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterTransactionSum) res++;
            return res;
        }
        public override OutMessage Execute()
        {
            var values = Message.Text.TrySplit('-', ' ');
            var sum = values[1].ParseSum();
            if (sum != -1)
            {
                Account.CurrentTransaction.Description = values[0];
                Account.CurrentTransaction.Sum = sum;
                Account.CurrentTransaction.Date = DateTime.Now;
                Controller.AddTransaction(Account.CurrentTransaction);
                Account.Status = AccountStatus.Free;
                return new OutMessage(Account, $"Success!", replyMarkup : Keyboards.MainKeyboard(Account));
            }
            return new OutMessage(Account, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
    }
}