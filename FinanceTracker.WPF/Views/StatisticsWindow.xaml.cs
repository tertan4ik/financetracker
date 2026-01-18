using FinanceTracker.Core.Data;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FinanceTracker.WPF.Views;

public partial class StatisticsWindow : Window
{
    public StatisticsWindow()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseSqlite("Data Source=finance_tracker.db")
            .Options;

        var db = new FinanceTrackerDbContext(options);
        var statisticsService = new StatisticsService(db);

        DataContext = new StatisticsViewModel(statisticsService);
    }
}
