using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Exspense
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("CategoryId")]
        public ExspenseCategory Category { get; set; }
    }
}