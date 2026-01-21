using FinanceTracker.Core.Data;
using FinanceTracker.Core.Models;
using FinanceTracker.MAUI.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.PlatformConfiguration;

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
        db.Database.EnsureCreated();

        // 🔥 SEED: создаём пользователя, если таблица пустая
        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Id = 1,
                Name = "Default User"
            });

            db.SaveChanges();
        }

        MainPage = new AppShell();
        //MainPage = new AppShell();
        //MainPage = new CategoriesPage();
    }
}
