using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class EditSavingGoalPage : ContentPage
{
    public EditSavingGoalPage(SavingGoal goal)
    {
        InitializeComponent();

        BindingContext = new EditSavingGoalViewModel(
            goal,
            App.Services.GetRequiredService<SavingGoalService>());
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is EditSavingGoalViewModel vm)
        {
            vm.SaveCommand.Execute(null);
            await Navigation.PopAsync();
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
