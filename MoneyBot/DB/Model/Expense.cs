using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Expense
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        [ForeignKey("CategoryId")]
        public ExpenseCategory Category { get; set; }
    }
}