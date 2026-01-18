using FinanceTracker.Core.Common;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class StatisticsViewModel : BaseViewModel
{
    private readonly StatisticsService _statisticsService;
    public ObservableCollection<CategoryIncomeStat> IncomeByCategory { get; }
    = new();
    private const int UserId = 1;

    private DateTime _fromDate = DateTime.Today.AddDays(-30);
    public DateTime FromDate
    {
        get => _fromDate;
        set { _fromDate = value; OnPropertyChanged(); }
    }

    private DateTime _toDate = DateTime.Today;
    public DateTime ToDate
    {
        get => _toDate;
        set { _toDate = value; OnPropertyChanged(); }
    }

    private decimal _totalIncome;
    public decimal TotalIncome
    {
        get => _totalIncome;
        set { _totalIncome = value; OnPropertyChanged(); }
    }

    private decimal _totalExpense;
    public decimal TotalExpense
    {
        get => _totalExpense;
        set { _totalExpense = value; OnPropertyChanged(); }
    }

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set { _balance = value; OnPropertyChanged(); }
    }

    public ObservableCollection<CategoryExpenseStat> ExpensesByCategory { get; }
        = new();

    public ICommand LoadStatisticsCommand { get; }

    public StatisticsViewModel(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;

        LoadStatisticsCommand = new RelayCommand(LoadStatistics);

        LoadStatistics();
    }

    private void LoadStatistics()
    {
        TotalIncome = _statisticsService.GetTotalIncome(UserId, FromDate, ToDate);
        TotalExpense = _statisticsService.GetTotalExpense(UserId, FromDate, ToDate);
        Balance = _statisticsService.GetBalance(UserId, FromDate, ToDate);
        ExpensesByCategory.Clear();
        foreach (var stat in _statisticsService.GetExpensesByCategory(UserId, FromDate, ToDate))
            ExpensesByCategory.Add(stat);

        IncomeByCategory.Clear();
        foreach (var stat in _statisticsService.GetIncomeByCategory(UserId, FromDate, ToDate))
            IncomeByCategory.Add(stat);

    }
}
