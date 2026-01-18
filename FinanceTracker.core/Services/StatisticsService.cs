using FinanceTracker.core.Enums;
using FinanceTracker.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace FinanceTracker.Core.Services;

public class StatisticsService
{
    private readonly FinanceTrackerDbContext _db;
    public StatisticsService(FinanceTrackerDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Общая сумма доходов за период
    /// </summary>
    public decimal GetTotalIncome(int userId, DateTime from, DateTime to)
    {
        return _db.Operations
            .Where(o =>
                o.UserId == userId &&
                o.Type == OperationType.Income &&
                o.Date >= from &&
                o.Date <= to)
            .Sum(o => o.Amount);
    }

    /// <summary>
    /// Общая сумма расходов за период
    /// </summary>
    public decimal GetTotalExpense(int userId, DateTime from, DateTime to)
    {
        return _db.Operations
            .Where(o =>
                o.UserId == userId &&
                o.Type == OperationType.Expense &&
                o.Date >= from &&
                o.Date <= to)
            .Sum(o => o.Amount);
    }

    /// <summary>
    /// Баланс за период
    /// </summary>
    public decimal GetBalance(int userId, DateTime from, DateTime to)
    {
        return GetTotalIncome(userId, from, to)
             - GetTotalExpense(userId, from, to);
    }

    /// <summary>
    /// Расходы по категориям (для диаграмм)
    /// </summary>
    public IReadOnlyList<CategoryExpenseStat> GetExpensesByCategory(
        int userId,
        DateTime from,
        DateTime to)
    {
        return _db.Operations
            .Include(o => o.Category)
            .Where(o =>
                o.UserId == userId &&
                o.Type == OperationType.Expense &&
                o.Date >= from &&
                o.Date <= to)
            .GroupBy(o => o.Category.Name)
            .Select(g => new CategoryExpenseStat
            {
                CategoryName = g.Key,
                TotalAmount = g.Sum(o => o.Amount)
            })
            .ToList();
    }
    /// <summary>
    /// Доходы по категориям
    /// </summary>
    public IReadOnlyList<CategoryIncomeStat> GetIncomeByCategory(
        int userId,
        DateTime from,
        DateTime to)
    {
        return _db.Operations
            .Include(o => o.Category)
            .Where(o =>
                o.UserId == userId &&
                o.Type == OperationType.Income &&
                o.Date >= from &&
                o.Date <= to)
            .GroupBy(o => o.Category.Name)
            .Select(g => new CategoryIncomeStat
            {
                CategoryName = g.Key,
                TotalAmount = g.Sum(o => o.Amount)
            })
            .ToList();
    }


}

public class CategoryIncomeStat
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// DTO для статистики по категориям
/// </summary>
public class CategoryExpenseStat
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
