using MoneyBot.DB.Model;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoneyBot.Telegram
{
    public class OutMessage
    {
        public OutMessage(Account account, string text, IReplyMarkup replyMarkup = null, int replyToMessageId = 0)
        {
            Account = account;
            Text = text;
            ReplyToMessageId = replyToMessageId;
            ReplyMarkup = replyMarkup;
        }
        public Account Account { get; set; }
        public string Text { get; set; }
        public int ReplyToMessageId { get; set; } = 0;
        public IReplyMarkup ReplyMarkup { get; set; }
        public int EditMessageId { get; set; } = 0;
    }
}