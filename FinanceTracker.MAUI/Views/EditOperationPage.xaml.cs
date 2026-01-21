using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class EditOperationPage : ContentPage
{
    public EditOperationPage(Operation operation)
    {
        InitializeComponent();

        BindingContext = new EditOperationViewModel(
            operation,
            App.Services.GetRequiredService<OperationService>(),
            App.Services.GetRequiredService<CategoryService>(),
            operation.UserId);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is EditOperationViewModel vm)
        {
            vm.SaveCommand.Execute(null);
            await Navigation.PopAsync();
        }
    }

}
