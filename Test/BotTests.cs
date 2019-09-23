using System.Threading.Tasks;
using MoneyBot.DB.Secondary;
using MoneyBot.Telegram;
using Telegram.Bot.Types;
using Xunit;

namespace Test
{
    public class BotTests
    {
        public BotTests()
        {

        }

        [Fact]
        public async void HandlingMessage()
        {
            var Bot = new Bot("823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA");
            Bot.Testing = true;

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Bot.OnMessageSent += (OutMessage mes) =>
            {
                Equals(mes.Text, "Welcome to MoneyBot.");
                tcs.SetResult(true);
            };

            Bot.HandleMessage(Create("/start"));

            await tcs.Task;
        }

        public static Message Create(string text) => new Message()
        {
            Chat = new Chat()
            {
            Id = -1,
            Username = "UnitTest"
            },
            Date = System.DateTime.Now,
            Text = text
        };
    }
}