using System;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class ShowPeopleQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("ShowPeople");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            message.Data.TryParseId(out var id);

            var person = account.People?.First(ct => ct.Id == id);

            if (person == null || person.Transactions == null)
            {
                return new Response(message.Id, "Everything is null");
            }
            account.Status = AccountStatus.Free;
            var categoryDays = person.Transactions.GroupBy(e => e.Date.Date).Select(r => $"{r.Key.ToString("dd MMMM")}\n{string.Join("\n", r.Select(k => $"{k.Description}: {(k.Type == MoneyDirection.In?"+":"-")+k.Sum}"))}");

            string mes = $"{person.Name}\n{string.Join("\n"+new string('-', 10)+"\n", categoryDays)}".Trim();
            if (message.Message.Text != mes)
                return new Response(account, mes, replyMarkup : Keyboards.People(account.People.ToArray(), "ShowPeople"))
                {
                    EditMessageId = message.Message.MessageId
                };
            else return new Response(message.Id, null);
        }
    }
}