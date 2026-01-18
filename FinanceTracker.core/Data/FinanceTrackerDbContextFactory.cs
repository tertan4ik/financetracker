using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanceTracker.Core.Data;

public class FinanceTrackerDbContextFactory
    : IDesignTimeDbContextFactory<FinanceTrackerDbContext>
{
    public FinanceTrackerDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseSqlite("Data Source=finance_tracker.db")
            .Options;

        return new FinanceTrackerDbContext(options);
    }
}
