using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class ExspenseCategory
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }

        public override string ToString() => $"{Emoji} {Name}";
    }
}