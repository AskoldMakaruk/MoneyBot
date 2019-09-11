using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Fund
    {
        public int Id { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Name { get; set; }
        public double TotalGoal { get; set; }
        public int MonthsDuration { get; set; }
    }
}