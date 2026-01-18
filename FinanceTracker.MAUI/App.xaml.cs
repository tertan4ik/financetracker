using FinanceTracker.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.PlatformConfiguration;
using FinanceTracker.MAUI.Views;

namespace FinanceTracker.MAUI;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        InitializeComponent();

        Services = IPlatformApplication.Current.Services;

        // 🔥 ВАЖНО: СОЗДАЁМ БАЗУ И ТАБЛИЦЫ
        using var scope = Services.CreateScope();
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FinanceTrackerDbContext>>();
        using var db = dbFactory.CreateDbContext();
        db.Database.EnsureCreated();

        //MainPage = new AppShell();
        MainPage = new CategoriesPage();
    }
}
