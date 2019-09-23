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
        public OutMessage(Account account, int editMessageId, string text, IReplyMarkup replyMarkup = null)
        {
            Account = account;
            Text = text;
            ReplyMarkup = replyMarkup;
            EditMessageId = editMessageId;
        }
        public OutMessage(string answerToMessageId, string text, bool answerQuery = true)
        {
            AnswerToMessageId = answerToMessageId;
            Text = text;
            AnswerQuery = answerQuery;
        }
        public Account Account { get; set; }
        public string Text { get; set; }
        public int ReplyToMessageId { get; set; } = 0;
        public IReplyMarkup ReplyMarkup { get; set; }
        public int EditMessageId { get; set; } = 0;
        public bool AnswerQuery { get; set; } = false;
        public string AnswerToMessageId { get; set; }
    }
}