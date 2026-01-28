using FinanceTracker.Core.Models;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class EditOperationPage : ContentPage
{
    public EditOperationPage(Operation? operation)
    {
        InitializeComponent();

        var vm = App.Services.GetRequiredService<OperationsViewModel>();
        BindingContext = vm;

        // null → добавление, не null → редактирование
        vm.SelectedOperation = operation;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is not OperationsViewModel vm)
            return;

        if (vm.SelectedOperation == null)
        {
            // ➕ добавление
            vm.AddOperationCommand.Execute(null);
        }
        else
        {
            // ✏️ редактирование
            vm.UpdateOperationCommand.Execute(null);
        }

        await Navigation.PopAsync();
    }
}
