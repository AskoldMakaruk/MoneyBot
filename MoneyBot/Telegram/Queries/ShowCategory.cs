using System;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class ShowCategoryQuery : Query
    {
        public ShowCategoryQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("ShowCategory");
        }
        public override OutMessage Execute()
        {
            Message.Data.TryParseId(out var id);

            var category = Account.Categories?.First(ct => ct.Id == id);

            if (category == null || category?.Expenses == null)
            {
                return new OutMessage(Message.Id, "Everything is null");
            }

            var categoryDays = category.Expenses.GroupBy(e => e.Date.Date).Select(r => $"{r.Key.ToString("dd MMMM")}\n{string.Join("\n", r.Select(k => $"{k.Description}: {k.Sum}"))}");

            Account.Status = AccountStatus.Free;

            string message = $"{category.ToString()}\n{string.Join(new string('-', 10)+"\n", categoryDays)}".Trim();
            if (Message.Message.Text != message)
                return new OutMessage(Account, Message.Message.MessageId, message, replyMarkup : Keyboards.Categories(Account.Categories.ToArray(), "Show"));
            else return new OutMessage(Message.Id, null);
        }
    }
}