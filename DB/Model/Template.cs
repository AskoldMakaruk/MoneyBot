namespace MoneyBot.DB.Model
{
    public class Template
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public ExpenseCategory Category { get; set; }
        public double Sum { get; set; }
        public string Name { get; set; }
    }
}