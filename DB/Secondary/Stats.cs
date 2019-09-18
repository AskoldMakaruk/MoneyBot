using System;
using System.Linq;
using MoneyBot.DB.Model;
namespace MoneyBot.DB.Secondary
{
    public class Stats
    {
        public double Incomes => _incomes
            .SelectMany(c => c.Expenses)
            .Where(e => DateTime.Now - e.Date < new TimeSpan(30, 0, 0, 0))
            .Sum(e => e.Sum);
        public double Expenses => _expenses
            .SelectMany(c => c.Expenses)
            .Where(e => DateTime.Now - e.Date < new TimeSpan(30, 0, 0, 0))
            .Sum(e => e.Sum);

        public double Owe => People
            .Sum(p => p.CountSum());

        public ExpenseCategory[] TopExpenseCategories => _expenses
            .Where(c => c.Expenses.Count != 0)
            .OrderByDescending(e => e.Expenses.Sum(r => r.Sum))
            .Take(4)
            .ToArray();
        public ExpenseCategory[] TopIncomeCategories => _incomes
            .Where(c => c.Expenses.Count != 0)
            .OrderByDescending(e => e.Expenses.Sum(r => r.Sum))
            .Take(4)
            .ToArray();
        //people that you gave money
        public Person[] TopDeptors => People
            .Where(p => p.CountSum() < 0)
            .OrderBy(p => p.CountSum()).Take(5).ToArray();
        //people that gave you money
        public Person[] TopCreditors => People
            .Where(p => p.CountSum() > 0)
            .OrderByDescending(p => p.CountSum()).Take(5).ToArray();

        private ExpenseCategory[] _expenses => Categories.Where(e => e.Type == MoneyDirection.Out).ToArray();
        private ExpenseCategory[] _incomes => Categories.Where(e => e.Type == MoneyDirection.In).ToArray();

        public double Balance => Incomes - Expenses;

        public ExpenseCategory[] Categories { get; set; }

        public Person[] People { get; set; }
    }
}