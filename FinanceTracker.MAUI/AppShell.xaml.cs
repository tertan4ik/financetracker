using FinanceTracker.MAUI.Views;

namespace FinanceTracker.MAUI


{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditCategoryPage), typeof(EditCategoryPage));
        }

    }
}
