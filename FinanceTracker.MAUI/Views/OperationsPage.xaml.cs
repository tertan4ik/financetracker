using FinanceTracker.Core.Models;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class OperationsPage : ContentPage
{
    public OperationsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Инициализация VM через DI
        if (BindingContext == null)
            BindingContext = App.Services.GetRequiredService<OperationsViewModel>();

        // 🔥 ОБНОВЛЯЕМ СПИСОК ОПЕРАЦИЙ ПРИ ВОЗВРАТЕ
        if (BindingContext is OperationsViewModel vm)
            vm.LoadData();
    }

    // ===== SWIPE: ИЗМЕНИТЬ =====
    private async void OnEditInvoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
            return;

        if (swipeItem.CommandParameter is not Operation operation)
            return;

        // Переход на экран редактирования операции
        await Navigation.PushAsync(new EditOperationPage(operation));
    }

    // ===== SWIPE: УДАЛИТЬ =====
    private async void OnDeleteInvoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
            return;

        if (swipeItem.CommandParameter is not Operation operation)
            return;

        if (BindingContext is not OperationsViewModel vm)
            return;

        bool confirm = await DisplayAlert(
            "Удаление",
            "Удалить операцию?",
            "Да",
            "Отмена");

        if (!confirm)
            return;

        vm.SelectedOperation = operation;
        vm.DeleteOperationCommand.Execute(null);
    }
    private async void OnAddOperationTapped(object sender, EventArgs e)
    {
        // null → режим добавления
        await Navigation.PushAsync(new EditOperationPage(null));
    }
}
