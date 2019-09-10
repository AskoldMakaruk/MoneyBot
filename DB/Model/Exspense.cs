using System;

namespace MoneyBot.DB.Model
{
    public class Exspense
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public DateTime Date { get; set; }
        public ExspenseCategory Category { get; set; }
        public Account Account { get; set; }
    }
}