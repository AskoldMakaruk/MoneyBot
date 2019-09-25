using System.Collections.Generic;
using System.Linq;
using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using MoneyBot.Telegram.Commands;
using Moq;
using Telegram.Bot.Types;
using Xunit;

namespace Test
{
    public class BotTests
    {
        public BotTests() { }

        [Theory]
        [InlineData("asdasdasd",                            0)]
        [InlineData("🍖- Out -  Food",                      1)]
        [InlineData("🍖- Out -  Food\n🍰  - Out -  Treats", 2)]
        public void AddCategory(string message, int count)
        {
            var mes  = Create(message);
            var mock = new Mock<TelegramController>();
            mock.Setup(c => c.FromMessage(mes)).Returns(new Account());
            mock.Setup(c => c.AddCategories(null)).Callback((IEnumerable<ExpenseCategory> input) =>
            {
                Assert.Equal(input.Count(), count);
            });

            var account = mock.Object.FromMessage(mes);
            account.Controller = mock.Object;

            var cmd    = new AddCategoryCommand();
            var outmes = cmd.Execute(mes, account);
        }

        [Theory]
        [InlineData("Apples - 14.88", 14.88)]
        [InlineData("Яблоки - 14.88", 14.88)]
        public void AddExpense(string message, double sum)
        {
            var mes  = Create(message);
            var mock = new Mock<TelegramController>();
            mock.Setup(c => c.AddExpense(null)).Callback((Expense input) => { Assert.Equal(input.Sum, sum); });

            var account = new Account {CurrentExpense = new Expense(), Controller = mock.Object};

            var cmd    = new EnterExpenseCommand();
            var outmes = cmd.Execute(mes, account);
        }

        public static Message Create(string text) => new Message()
        {
            Chat = new Chat()
            {
                Id       = -1,
                Username = "UnitTest"
            },
            Date = System.DateTime.Now,
            Text = text
        };
    }
}