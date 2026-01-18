using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class CategoriesPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext == null)
            BindingContext = App.Services.GetRequiredService<CategoriesViewModel>();
    }
}
