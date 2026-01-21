using FinanceTracker.Core.Models;
using FinanceTracker.Core.ViewModels;

namespace FinanceTracker.MAUI.Views;

public partial class CategoriesPage : ContentPage
{

    public CategoriesPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Content = new Label
        //{
        //    Text = "CODE-BEHIND WORKS",
        //    FontSize = 30,
        //    TextColor = Colors.Red
        //};


        if (BindingContext == null)
            BindingContext = App.Services.GetRequiredService<CategoriesViewModel>();
            if (BindingContext is CategoriesViewModel vm)
            vm.LoadCategories();
    }

    private void OnDeleteInvoked(object sender, EventArgs e)
    {
        Console.WriteLine("Button pressed");
        if (sender is not SwipeItem swipeItem)

            return;

        if (swipeItem.CommandParameter is not Category category)

        return;

        if (BindingContext is not CategoriesViewModel vm)
            return;

        // 🔥 ВОТ ЭТО КЛЮЧЕВО
        vm.SelectedCategory = category;

        if (vm.DeleteCategoryCommand.CanExecute(null))
            vm.DeleteCategoryCommand.Execute(null);
    }

    private void test(object sender,EventArgs e)
    {
        Console.WriteLine("BUTTON PRESSED");
    }

    private async void OnEditInvoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem)
            return;

        if (swipeItem.CommandParameter is not Category category)
            return;

        await Navigation.PushAsync(new EditCategoryPage(category));
    }


}
