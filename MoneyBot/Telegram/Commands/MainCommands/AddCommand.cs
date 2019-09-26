using MoneyBot.DB.Model;
using MoneyBot.Telegram.Queries;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class AddCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text == "Add" && account.Status == AccountStatus.Free) res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            //todo fast templates and change text on buttons
            var keys = Keyboards.AddType(account);
            if (account.PeopleInited() && account.CategoriesInited())
            {
                return new Response(account, $"This is about", replyMarkup : keys);
            }
            else if (account.PeopleInited())
            {
                return AddTypeQuery.SelectRecordType(account, DB.Secondary.RecordType.Transaction);
            }
            else if (account.CategoriesInited())
            {
                return AddTypeQuery.SelectRecordType(account, DB.Secondary.RecordType.Expense);
            }
            else
                return new Response(account, $"Add category or person first");
        }
    }
}