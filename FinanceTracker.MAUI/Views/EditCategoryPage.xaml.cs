using FinanceTracker.Core.Models;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class EditCategoryPage : ContentPage
{
    public EditCategoryPage(Category? category)
    {
        InitializeComponent();

        var vm = App.Services.GetRequiredService<CategoriesViewModel>();
        BindingContext = vm;

        // 🔑 если category == null → добавление
        // 🔑 если category != null → редактирование
        vm.SelectedCategory = category;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is not CategoriesViewModel vm)
            return;

        if (vm.SelectedCategory == null)
        {
            // ➕ добавление
            if (vm.AddCategoryCommand.CanExecute(null))
                vm.AddCategoryCommand.Execute(null);
        }
        else
        {
            // ✏️ редактирование
            if (vm.UpdateCategoryCommand.CanExecute(null))
                vm.UpdateCategoryCommand.Execute(null);
        }

        await Navigation.PopAsync();
    }


}
