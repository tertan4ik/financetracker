using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<StatisticsViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StatisticsViewModel vm)
        {
            vm.LoadStatisticsCommand.Execute(null);
        }
    }
}
