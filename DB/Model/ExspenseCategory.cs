using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class ExspenseCategory
    {
        public int Id { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public List<Exspense> Exspenses { get; set; }
        public override string ToString() => $"{Emoji} {Name}";
    }
}