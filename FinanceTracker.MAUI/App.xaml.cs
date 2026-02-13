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
        try
        {
            // Глобальный обработчик непойманных исключений
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ CRASH (AppDomain): {ex}");
                    Console.WriteLine($"❌ CRASH (AppDomain): {ex}");

                    // Показываем alert на Android
                    if (Application.Current?.MainPage != null)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await Application.Current.MainPage.DisplayAlert(
                                    "Ошибка",
                                    $"Произошла ошибка: {ex.Message}",
                                    "OK");
                            }
                            catch { }
                        });
                    }
                }
            };

            // Для MAUI UI поток
          

            // Для задач (Task)
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine($"❌ Task CRASH: {args.Exception}");
                Console.WriteLine($"❌ Task CRASH: {args.Exception}");
                args.SetObserved();
            };

            InitializeComponent();

            Services = IPlatformApplication.Current.Services;

            // 🔥 ВАЖНО: СОЗДАЁМ БАЗУ И ТАБЛИЦЫ
            try
            {
                using var scope = Services.CreateScope();
                var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FinanceTrackerDbContext>>();
                using var db = dbFactory.CreateDbContext();
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ DB ERROR: {ex}");
                Console.WriteLine($"❌ DB ERROR: {ex}");
            }

            MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ APP INIT ERROR: {ex}");
            Console.WriteLine($"❌ APP INIT ERROR: {ex}");

            // Создаем минимальный MainPage чтобы приложение не упало
            MainPage = new ContentPage
            {
                Content = new Label
                {
                    Text = $"Ошибка запуска: {ex.Message}",
                    TextColor = Colors.Red,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
        }
    }
}