using System;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class ShowCategoryQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("ShowCategory");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            account.Status = AccountStatus.Free;

            message.Data.TryParseId(out var id);

            var category = account.Categories?.First(ct => ct.Id == id);
            if (category == null || category?.Expenses == null)
            {
                return new Response(message.Id, "Everything is null");
            }

            var categoryDays = category.Expenses.GroupBy(e => e.Date.Date)
                .Select(r => $"{r.Key.ToString("dd MMMM")}:\n{string.Join("\n", r.Select(k => k.Description.IsNullOrEmpty()?k.Sum.ToString(): $"{k.Description}: {k.Sum}"))}");

            string mes = $"{category.ToString()}\n{string.Join("\n"+new string('-', 10)+"\n", categoryDays)}".Trim();

            if (message.Message.Text != mes)
            {
                return new Response(account, message.Message.MessageId, mes, replyMarkup : Keyboards.ShowActiveCategories(account.Categories));
            }
            else return new Response(message.Id, null);
        }
    }
}