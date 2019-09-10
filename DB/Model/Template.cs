using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Template
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public ExpenseCategory Category { get; set; }
    }
}