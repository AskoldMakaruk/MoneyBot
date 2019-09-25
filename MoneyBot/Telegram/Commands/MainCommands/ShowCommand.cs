using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class ShowCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text == "Show" && account.Status == AccountStatus.Free) res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            if (account.PeopleInitedAndNotEmpty() && account.PeopleInitedAndNotEmpty())
            {
                account.Status = AccountStatus.ChooseShow;
                return new Response(account, $"What you desire to see?", replyMarkup : Keyboards.MainShow);
            }
            else if (account.PeopleInitedAndNotEmpty())
            {
                return ShowCategoriesCommand.ToPeople(account);
            }
            else if (account.CategoriesInitedAndNotEmpty())
            {
                return ShowCategoriesCommand.ToCategory(account);
            }
            else
                return new Response(account, $"Add category or person first");
        }
    }
}