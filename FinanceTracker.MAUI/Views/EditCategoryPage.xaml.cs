using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class EditCategoryPage : ContentPage
{
    public EditCategoryPage(Category category)
    {
        InitializeComponent();

        BindingContext = new EditCategoryViewModel(
            category,
            App.Services.GetRequiredService<CategoryService>()
        );
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is EditCategoryViewModel vm)
        {
            vm.SaveCommand.Execute(null);
            await Navigation.PopAsync(); // ✅ UI слой, всё законно
        }
    }

}