using FinanceTracker.Core.Data;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FinanceTracker.WPF.Views;

public partial class SavingGoalsWindow : Window
{
    public SavingGoalsWindow()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseSqlite("Data Source=finance_tracker.db")
            .Options;

        var db = new FinanceTrackerDbContext(options);
        var service = new SavingGoalService(db);

        DataContext = new SavingGoalsViewModel(service);
    }
}
