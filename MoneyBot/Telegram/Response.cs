using MoneyBot.DB.Model;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoneyBot.Telegram
{
    public class Response
    {
        public Response(User user, string text, IReplyMarkup replyMarkup = null, int replyToMessageId = 0)
        {
            User = user;
            Text = text;
            ReplyToMessageId = replyToMessageId;
            ReplyMarkup = replyMarkup;
        }
        public Response(User user, int editMessageId, string text, IReplyMarkup replyMarkup = null)
        {
            User = user;
            Text = text;
            ReplyMarkup = replyMarkup;
            EditMessageId = editMessageId;
        }
        public Response(string answerToMessageId, string text, bool answerQuery = true)
        {
            AnswerToMessageId = answerToMessageId;
            Text = text;
            AnswerQuery = answerQuery;
        }
        public User User { get; set; }
        public string Text { get; set; }
        public int ReplyToMessageId { get; set; } = 0;
        public IReplyMarkup ReplyMarkup { get; set; }
        public int EditMessageId { get; set; } = 0;
        public bool AnswerQuery { get; set; } = false;
        public string AnswerToMessageId { get; set; }
    }
}