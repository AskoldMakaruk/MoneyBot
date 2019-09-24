using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterTransactionCommand : Command
    {
        public EnterTransactionCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.EnterTransactionSum) res++;
            return res;
        }
        public override OutMessage Execute(Message message, Account account)
        {
            var values = message.Text.TrySplit('-', ' ');
            var sum = values[1].ParseSum();
            if (sum != -1)
            {
                account.CurrentTransaction.Description = values[0];
                account.CurrentTransaction.Sum = sum;
                account.CurrentTransaction.Date = DateTime.Now;
                account.Controller.AddTransaction(account.CurrentTransaction);
                account.Status = AccountStatus.Free;
                return new OutMessage(account, $"Success!", replyMarkup : Keyboards.MainKeyboard(account));
            }
            return new OutMessage(account, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
    }
}