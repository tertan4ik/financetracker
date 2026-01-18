using System.Windows;
using FinanceTracker.Core.Data;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.WPF.Views;

public partial class OperationWindow : Window
{
    public OperationWindow()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseSqlite("Data Source=finance_tracker.db")
            .Options;

        var db = new FinanceTrackerDbContext(options);

        var categoryService = new CategoryService(db);
        var operationService = new OperationService(db);

        DataContext = new OperationsViewModel(operationService, categoryService);
    }
}
