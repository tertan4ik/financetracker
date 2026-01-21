using System.Windows;
using FinanceTracker.Core.Data;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.WPF.Views;

public partial class CategoryWindow : Window
{
    public CategoryWindow()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseSqlite("Data Source=finance_tracker.db")
            .Options;

        var db = new FinanceTrackerDbContext(options);
        var categoryService = new CategoryService(db);

        const int DefaultUserId = 1;

        DataContext = new CategoriesViewModel(categoryService);
    }
}
