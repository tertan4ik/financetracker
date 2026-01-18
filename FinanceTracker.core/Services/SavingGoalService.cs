using FinanceTracker.Core.Data;
using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Core.Services;

public class SavingGoalService
{
    private readonly FinanceTrackerDbContext _db;

    public SavingGoalService(FinanceTrackerDbContext db)
    {
        _db = db;
    }

    public IReadOnlyList<SavingGoal> GetGoals(int userId)
    {
        return _db.SavingGoals
            .Where(g => g.UserId == userId)
            .AsNoTracking()
            .ToList();
    }

    public SavingGoal CreateGoal(string name, decimal targetAmount, int userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if (targetAmount <= 0)
            throw new ArgumentException("Target amount must be greater than zero");

        var goal = new SavingGoal
        {
            Name = name.Trim(),
            TargetAmount = targetAmount,
            CurrentAmount = 0,
            UserId = userId
        };

        _db.SavingGoals.Add(goal);
        _db.SaveChanges();

        return goal;
    }

    public void UpdateGoal(SavingGoal goal)
    {
        var existing = _db.SavingGoals.FirstOrDefault(g => g.Id == goal.Id);
        if (existing == null)
            return;

        existing.Name = goal.Name.Trim();
        existing.TargetAmount = goal.TargetAmount;
        existing.CurrentAmount = goal.CurrentAmount;

        _db.SaveChanges();
    }

    public void DeleteGoal(int goalId)
    {
        var goal = _db.SavingGoals.FirstOrDefault(g => g.Id == goalId);
        if (goal == null)
            return;

        _db.SavingGoals.Remove(goal);
        _db.SaveChanges();
    }
}
