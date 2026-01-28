using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Data;
using FinanceTracker.Core.Models;
using FinanceTracker.core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Core.Services;

public class BudgetService
{
    private readonly FinanceTrackerDbContext _db;

    public BudgetService(FinanceTrackerDbContext db)
    {
        _db = db;
    }

    public Budget CreateBudget(CreateBudgetRequest request)
    {
        if (request.TotalLimit <= 0)
            throw new ArgumentException("Budget limit must be greater than zero");

        var budget = new Budget
        {
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            TotalLimit = request.TotalLimit,
            UserId = request.UserId
        };

        _db.Budgets.Add(budget);
        _db.SaveChanges();

        return budget;
    }

    /// <summary>
    ///  бюджеты пользователя
    /// </summary>
    public IReadOnlyList<Budget> GetBudgets(int userId)
    {
        return _db.Budgets
            .Include(b => b.CategoryLimits)
            .ThenInclude(l => l.Category)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Id)
            .ToList();
    }

    public void SetCategoryLimit(int budgetId, int categoryId, decimal limit)
    {
        var existing = _db.BudgetCategoryLimits
            .FirstOrDefault(l => l.BudgetId == budgetId && l.CategoryId == categoryId);

        if (existing != null)
        {
            existing.LimitAmount = limit;
        }
        else
        {
            _db.BudgetCategoryLimits.Add(new BudgetCategoryLimit
            {
                BudgetId = budgetId,
                CategoryId = categoryId,
                LimitAmount = limit
            });
        }

        _db.SaveChanges();
    }

    public decimal GetTotalExpenses(Budget budget)
    {
        return _db.Operations
            .Where(o =>
                o.UserId == budget.UserId &&
                o.Type == OperationType.Расход &&
                o.Date >= budget.PeriodStart &&
                o.Date <= budget.PeriodEnd)
            .Sum(o => o.Amount);
    }

    public decimal GetUsagePercent(Budget budget)
    {
        if (budget.TotalLimit == 0)
            return 0;

        return GetTotalExpenses(budget) / budget.TotalLimit * 100;
    }

    public bool IsBudgetExceeded(Budget budget)
    {
        return GetTotalExpenses(budget) > budget.TotalLimit;
    }

    /// <summary>
    /// Удаление бюджета
    /// </summary>
    public void DeleteBudget(int budgetId)
    {
        var budget = _db.Budgets
            .Include(b => b.CategoryLimits)
            .FirstOrDefault(b => b.Id == budgetId);

        if (budget == null)
            return;

        // сначала удаляем лимиты категорий
        _db.BudgetCategoryLimits.RemoveRange(budget.CategoryLimits);

        // затем сам бюджет
        _db.Budgets.Remove(budget);

        _db.SaveChanges();
    }

    /// <summary>
    /// Лимиты и фактические расходы по категориям для бюджета
    /// </summary>
    public IReadOnlyList<CategoryBudgetStat> GetCategoryBudgetStats(Budget budget)
    {
        var categories = _db.Categories
       .Where(c =>
           c.UserId == budget.UserId &&
           c.Type == CategoryType.Расход)
       .ToList();

        var expenses = _db.Operations
            .Where(o =>
                o.UserId == budget.UserId &&
                o.Type == OperationType.Расход &&
                o.Date >= budget.PeriodStart &&
                o.Date <= budget.PeriodEnd)
            .GroupBy(o => o.CategoryId)
            .Select(g => new
            {
                CategoryId = g.Key,
                Total = g.Sum(o => o.Amount)
            })
            .ToList();

        var limits = _db.BudgetCategoryLimits
            .Where(l => l.BudgetId == budget.Id)
            .ToList();

        return categories.Select(c =>
        {
            var spent = expenses.FirstOrDefault(e => e.CategoryId == c.Id)?.Total ?? 0;
            var limit = limits.FirstOrDefault(l => l.CategoryId == c.Id)?.LimitAmount ?? 0;

            return new CategoryBudgetStat
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                SpentAmount = spent,
                LimitAmount = limit
            };
        }).ToList();
    }

    /// <summary>
    /// Обновление бюджета
    /// </summary>
    public void UpdateBudget(Budget budget)
    {
        var existing = _db.Budgets.FirstOrDefault(b => b.Id == budget.Id);
        if (existing == null)
            return;

        existing.PeriodStart = budget.PeriodStart;
        existing.PeriodEnd = budget.PeriodEnd;
        existing.TotalLimit = budget.TotalLimit;

        _db.SaveChanges();
    }


    public IReadOnlyList<Category> GetExpenseCategories(int userId)
    {
        Console.WriteLine("Avaliblecat is done");
        return _db.Categories
            .Where(c =>
                c.UserId == userId &&
                c.Type == CategoryType.Расход)
            .ToList();
    }

}
