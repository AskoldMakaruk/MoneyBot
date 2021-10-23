using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram
{
    public static class SendExtensions
    {
        public static async Task SelectRecordType(this IClient client, Account account, RecordType category, Message message = null)
        {
            account.CurrentRecord = new AddRecord { RecordType = category };

            if (message != null)
            {
                await client.EditMessageText(message.MessageId, $"Choose one:", replyMarkup: Keyboards.CategoryTypes(account, "RecordType"));
            }
            else
            {
                await client.SendTextMessage($"Choose one:", replyMarkup: Keyboards.CategoryTypes(account, "RecordType"));
            }
        }

        public static async Task ToCategory(this IClient client, Account account)
        {
            await client.SendTextMessage($"Select one to see info.", replyMarkup: Keyboards.ShowActiveCategories(account.Categories));
        }

        public static async Task ToPeople(this IClient client, Account account)
        {
            await client.SendTextMessage($"Select one to see info.", replyMarkup: Keyboards.People(account.People, "ShowPeople"));
        }
    }
}