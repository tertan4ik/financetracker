using FinanceTracker.Core.Models;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class SavingGoalsPage : ContentPage
{
    public SavingGoalsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext == null)
            BindingContext = App.Services.GetRequiredService<SavingGoalsViewModel>();

        if (BindingContext is SavingGoalsViewModel vm)
            vm.LoadGoals();
    }

    private async void OnEditInvoked(object sender, EventArgs e)
    {

        if (sender is SwipeItem swipeItem &&
            swipeItem.CommandParameter is SavingGoal goal)
        {
            Console.WriteLine(goal.Name);
            await Navigation.PushAsync(new EditSavingGoalPage(goal));
        }
    }

    private async void OnDeleteInvoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem &&
            swipeItem.CommandParameter is SavingGoal goal &&
            BindingContext is SavingGoalsViewModel vm)
        {
            bool confirm = await DisplayAlert(
         "Удаление",
         "Удалить Цель?",
         "Да",
         "Отмена");

            if (!confirm)
                return;
            vm.SelectedGoal = goal;
            vm.DeleteGoalCommand.Execute(null);
        }
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
        Console.WriteLine("Add started");
        await Navigation.PushAsync(new EditSavingGoalPage(null));
      
    }
}
