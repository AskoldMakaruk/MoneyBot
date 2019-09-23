using MoneyBot.DB.Model;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoneyBot.Telegram
{
    public class OutMessage
    {
        public OutMessage(Account account, string text, int replyToMessageId = 0, IReplyMarkup replyMarkup = null)
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
    }
}