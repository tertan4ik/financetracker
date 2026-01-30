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

    public void CreateGoal(
        string name,
        decimal targetAmount,
        decimal currentAmount,
        DateTime deadline,
        int userId)
    {
        var goal = new SavingGoal
        {
            Name = name,
            TargetAmount = targetAmount,
            CurrentAmount = currentAmount,
            Deadline = deadline,
            UserId = userId
        };

        _db.SavingGoals.Add(goal);
        _db.SaveChanges();
    }



    public void UpdateGoal(SavingGoal updatedGoal)
    {
        var goal = _db.SavingGoals.FirstOrDefault(g => g.Id == updatedGoal.Id);
        if (goal == null)
            return;

        goal.Name = updatedGoal.Name;
        goal.TargetAmount = updatedGoal.TargetAmount;
        goal.CurrentAmount = updatedGoal.CurrentAmount;
        goal.Deadline = updatedGoal.Deadline;

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
