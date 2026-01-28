using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class BudgetPage : ContentPage
{
    public BudgetPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("BUDGETAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        if (BindingContext == null)
            BindingContext = App.Services.GetRequiredService<BudgetViewModel>();

        if (BindingContext is BudgetViewModel vm)
            vm.LoadBudgets();
    }
}
