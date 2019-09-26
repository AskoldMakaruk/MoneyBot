using System;
using MoneyBot.DB.Model;

namespace MoneyBot.DB.Secondary
{
    public class AddRecord
    {
        public string Description { get; set; }
        public double Sum { get; set; }

        public DateTime Date { get; set; }
        public MoneyDirection Direction { get; set; }
        public RecordType RecordType { get; set; }

        public int FromId { get; set; }
    }
    public enum RecordType
    {
        Transaction,
        Expense
    }
}