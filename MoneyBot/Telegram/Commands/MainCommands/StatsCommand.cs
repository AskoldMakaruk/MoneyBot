using System;
using System.Linq;
using System.Text;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class StatsCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text == "Stats") res += 2;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            var stats = account.Controller.GetStats(account.Id);

            bool ownings = account.PeopleInitedAndNotEmpty();
            bool incomes = stats.TopIncomeCategories.Count() != 0;
            bool expenses = stats.TopExpenseCategories.Count() != 0;
            bool creditors = stats.TopCreditors.Count() != 0;
            bool debtors = stats.TopDeptors.Count() != 0;

            var builder = new StringBuilder();
            builder.AppendJoin('\n',
                $"{(DateTime.Now - new TimeSpan(30,0,0,0)).ToString("dd MMMM")} - {DateTime.Now.ToString("dd MMMM")}",
                $"Balance: {stats.Balance}\n\n"
            );
            if (stats.Owe != 0)
            {
                builder.AppendLine($"Ownings: {stats.Owe}");
            }
            if (incomes)
            {
                builder.AppendLine($"Incomes: {stats.Incomes}");
                builder.AppendJoin("\n", stats.TopIncomeCategories.Select(c => $"{c.Emoji}{c.Name} {c.Expenses.Sum(r => r.Sum)}"));
            }
            if (expenses)
            {
                builder.AppendLine($"Expenses: {stats.Expenses}");
                builder.AppendJoin("\n", stats.TopExpenseCategories.Select(c => $"{c.Emoji}{c.Name} {c.Expenses.Sum(r => r.Sum)}"));
            }
            if (creditors)
            {
                builder.AppendLine("\n\nYour top creditors:");
                builder.AppendJoin("\n", stats.TopCreditors.Select(d => $"{d.Name}: {d.CountSum()}"));
            }
            if (debtors)
            {
                builder.AppendLine("\n\nYour top debtors:");
                builder.AppendJoin("\n", stats.TopDeptors.Select(d => $"{d.Name}: {d.CountSum()}"));
            }

            return new Response(account, builder.ToString());
        }
    }
}