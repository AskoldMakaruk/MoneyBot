namespace MoneyBot.DB.Model
{
    public class Fund
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public double TotalGoal { get; set; }
        public int LenghtMonths { get; set; }
    }
}