using FinanceTracker.Core.Data;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ---------- DATABASE ----------
        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "finance_tracker.db"
        );

        builder.Services.AddDbContextFactory<FinanceTrackerDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });

        // ---------- SERVICES ----------
        builder.Services.AddTransient<CategoryService>();
        builder.Services.AddTransient<OperationService>();
        builder.Services.AddTransient<BudgetService>();
        builder.Services.AddTransient<SavingGoalService>();
        builder.Services.AddTransient<StatisticsService>();
        // ---------- VIEW MODELS ----------
        builder.Services.AddTransient<CategoriesViewModel>();
        builder.Services.AddTransient<OperationsViewModel>();
        builder.Services.AddTransient<BudgetViewModel>();
        builder.Services.AddTransient<SavingGoalsViewModel>();
        builder.Services.AddTransient<StatisticsViewModel>();

        return builder.Build();
    }
}
