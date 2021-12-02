using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Services.Extensioins;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram
{
    public static class SendExtensions
    {
        public static async Task SelectRecordType(this IClient client, User user, RecordType category, Message message = null)
        {
            // user.CurrentRecord = new AddRecord { RecordType = category };

            if (message != null)
            {
                await client.EditMessageText(message.MessageId, $"Choose one:", replyMarkup: Keyboards.CategoryTypes(user, "RecordType"));
            }
            else
            {
                await client.SendTextMessage($"Choose one:", replyMarkup: Keyboards.CategoryTypes(user, "RecordType"));
            }
        }

        public static async Task ToCategory(this IClient client, User user)
        {
            await client.SendTextMessage($"Select one to see info.", replyMarkup: Keyboards.ShowActiveCategories(user.Categories));
        }

        public static async Task ToPeople(this IClient client, User user)
        {
            // await client.SendTextMessage($"Select one to see info.", replyMarkup: Keyboards.People(user.People, "ShowPeople"));
        }
    }
}