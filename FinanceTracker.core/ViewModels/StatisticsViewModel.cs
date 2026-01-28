using FinanceTracker.Core.Common;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class StatisticsViewModel : BaseViewModel
{
    private readonly StatisticsService _statisticsService;
    private const int UserId = 1;

    public ObservableCollection<CategoryIncomeStat> IncomeByCategory { get; } = new();
    public ObservableCollection<CategoryExpenseStat> ExpensesByCategory { get; } = new();

    private DateTime _fromDate = DateTime.Today.AddDays(-30);
    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            _fromDate = value;
            OnPropertyChanged();
            LoadStatistics();
        }
    }

    private DateTime _toDate = DateTime.Today;
    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            _toDate = value;
            OnPropertyChanged();
            LoadStatistics();
        }
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

        // =========================
        // РАСХОДЫ (DESC)
        // =========================
        ExpensesByCategory.Clear();

        var expenses = _statisticsService
            .GetExpensesByCategory(UserId, FromDate, ToDate)
            .OrderByDescending(x => x.TotalAmount); // 🔥 СОРТИРОВКА

        foreach (var stat in expenses)
        {
            stat.Percentage =
                TotalExpense == 0 ? 0 : (double)(stat.TotalAmount / TotalExpense);

            ExpensesByCategory.Add(stat);
        }

        // =========================
        // ДОХОДЫ (DESC)
        // =========================
        IncomeByCategory.Clear();

        var incomes = _statisticsService
            .GetIncomeByCategory(UserId, FromDate, ToDate)
            .OrderByDescending(x => x.TotalAmount); // 🔥 СОРТИРОВКА

        foreach (var stat in incomes)
        {
            stat.Percentage =
                TotalIncome == 0 ? 0 : (double)(stat.TotalAmount / TotalIncome);

            IncomeByCategory.Add(stat);
        }
    }
}
