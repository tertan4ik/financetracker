using FinanceTracker.Core.Data;
using FinanceTracker.WPF.Views;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FinanceTracker.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new CategoryWindow().Show();
            new OperationWindow().Show();
            new StatisticsWindow().Show();
            new BudgetWindow().Show();
            new SavingGoalsWindow().Show();
            var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
                .UseSqlite("Data Source=finance_tracker.db")
                .Options;

            using var db = new FinanceTrackerDbContext(options);

            db.Database.EnsureCreated();
        }

    }
}
