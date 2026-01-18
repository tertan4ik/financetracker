using Microsoft.EntityFrameworkCore;
using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Data;

public class FinanceTrackerDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<BudgetCategoryLimit> BudgetCategoryLimits => Set<BudgetCategoryLimit>();
    public DbSet<SavingGoal> SavingGoals => Set<SavingGoal>();

    public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category → User
        modelBuilder.Entity<Category>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Operation → User
        modelBuilder.Entity<Operation>()
            .HasOne(o => o.User)
            .WithMany(u => u.Operations)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Operation → Category
        modelBuilder.Entity<Operation>()
            .HasOne(o => o.Category)
            .WithMany(c => c.Operations)
            .HasForeignKey(o => o.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Budget → User
        modelBuilder.Entity<Budget>()
            .HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // BudgetCategoryLimit → Budget
        modelBuilder.Entity<BudgetCategoryLimit>()
            .HasOne(l => l.Budget)
            .WithMany(b => b.CategoryLimits)
            .HasForeignKey(l => l.BudgetId);

        // BudgetCategoryLimit → Category
        modelBuilder.Entity<BudgetCategoryLimit>()
            .HasOne(l => l.Category)
            .WithMany(c => c.BudgetCategoryLimits)
            .HasForeignKey(l => l.CategoryId);

        // SavingGoal → User
        modelBuilder.Entity<SavingGoal>()
            .HasOne(g => g.User)
            .WithMany(u => u.SavingGoals)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
