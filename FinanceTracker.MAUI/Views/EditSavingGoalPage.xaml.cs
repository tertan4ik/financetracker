using FinanceTracker.Core.Models;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class EditSavingGoalPage : ContentPage
{
    public EditSavingGoalPage(SavingGoal? goal)
    {
        InitializeComponent();

        var vm = App.Services.GetRequiredService<SavingGoalsViewModel>();
        BindingContext = vm;

        // просто прокидываем цель
        vm.SetSelectedGoal(goal);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {

        if (BindingContext is SavingGoalsViewModel vm)
        {
            vm.SaveGoalCommand.Execute(null);
            Console.WriteLine("save command executed");
            await Navigation.PopAsync();
        }
        else
        {
            Console.WriteLine("save command not executed");
            await Navigation.PopAsync();
        }


    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
