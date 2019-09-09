using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class ExspenseCategory
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
    }
}