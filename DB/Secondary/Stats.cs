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

        private ExpenseCategory[] _expenses => Categories.Where(e => e.Type == ExpenseType.Out).ToArray();
        private ExpenseCategory[] _incomes => Categories.Where(e => e.Type == ExpenseType.In).ToArray();

        public double Balance => Incomes - Expenses;

        public ExpenseCategory[] Categories { get; set; }
    }
}