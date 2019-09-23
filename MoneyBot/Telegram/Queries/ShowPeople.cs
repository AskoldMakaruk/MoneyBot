using System;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class ShowPeopleQuery : Query
    {
        public ShowPeopleQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("ShowPeople");
        }
        public override OutMessage Execute()
        {
            Message.Data.TryParseId(out var id);

            var person = Account.People?.First(ct => ct.Id == id);

            if (person == null || person.Transactions == null)
            {
                return new OutMessage(Message.Id, "Everything is null");
            }
            Account.Status = AccountStatus.Free;
            var categoryDays = person.Transactions.GroupBy(e => e.Date.Date).Select(r => $"{r.Key.ToString("dd MMMM")}\n{string.Join("\n", r.Select(k => $"{k.Description}: {(k.Type == MoneyDirection.In?"+":"-")+k.Sum}"))}");

            string message = $"{person.Name}\n{string.Join("\n"+new string('-', 10)+"\n", categoryDays)}".Trim();
            if (Message.Message.Text != message)
                return new OutMessage(Account, message, replyMarkup : Keyboards.People(Account.People.ToArray(), "ShowPeople"))
                {
                    EditMessageId = Message.Message.MessageId
                };
            else return new OutMessage(Message.Id, null);
        }
    }
}