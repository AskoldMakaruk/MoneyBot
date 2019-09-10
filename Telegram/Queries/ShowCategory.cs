using System;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class ShowCategoryQuery : Query
    {
        public ShowCategoryQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("Show");
        }
        public override async void Execute()
        {
            var idStr = Message.Data.Substring(Message.Data.LastIndexOf(" ") + 1);
            int.TryParse(idStr, out var id);

            var category = Account.Categories?.First(ct => ct.Id == id);

            if (category == null || category?.Expenses == null)
            {
                try { await Client.AnswerCallbackQueryAsync(Message.Id, "Everything is null"); }
                catch { }
                return;
            }

            var categoryDays = category.Expenses.GroupBy(e => e.Date.Date).Select(r => $"{r.Key.ToString("dd MMMM")}\n{string.Join('\n', r.Select(k => $"{k.Description}: {k.Sum}"))}");

            string message = $"{category.ToString()}\n{string.Join(new string('-', 10)+"\n", categoryDays)}";

            await Client.SendTextMessageAsync(Account.ChatId, message, replyMarkup : Keyboards.Main);
            Account.Status = AccountStatus.Free;
        }
    }
}