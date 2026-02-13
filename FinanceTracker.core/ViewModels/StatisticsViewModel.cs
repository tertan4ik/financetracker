using FinanceTracker.Core.Common;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class StatisticsViewModel : BaseViewModel
{
    private readonly StatisticsService _statisticsService;
    private readonly OperationService _operationService;
    private const int UserId = 1;

    public ObservableCollection<CategoryIncomeStat> IncomeByCategory { get; } = new();
    public ObservableCollection<CategoryExpenseStat> ExpensesByCategory { get; } = new();
    public ObservableCollection<Operation> RecentOperations { get; } = new();

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

    private decimal _totalBalance; // общий баланс (все операции)
    public decimal TotalBalance
    {
        get => _totalBalance;
        set { _totalBalance = value; OnPropertyChanged(); }
    }

    private decimal _periodIncome;
    public decimal PeriodIncome
    {
        get => _periodIncome;
        set { _periodIncome = value; OnPropertyChanged(); }
    }

    private decimal _periodExpense;
    public decimal PeriodExpense
    {
        get => _periodExpense;
        set { _periodExpense = value; OnPropertyChanged(); }
    }

    private decimal _periodBalance;
    public decimal PeriodBalance
    {
        get => _periodBalance;
        set { _periodBalance = value; OnPropertyChanged(); }
    }

    public ICommand LoadStatisticsCommand { get; }

    public StatisticsViewModel(StatisticsService statisticsService, OperationService operationService)
    {
        _statisticsService = statisticsService;
        _operationService = operationService;
        LoadStatisticsCommand = new RelayCommand(LoadStatistics);
        LoadStatistics();
    }

    private void LoadStatistics()
    {
        // Общий баланс (все операции без фильтра по дате)
        TotalBalance = _statisticsService.GetTotalBalance(UserId);

        // Периодные показатели
        PeriodIncome = _statisticsService.GetTotalIncome(UserId, FromDate, ToDate);
        PeriodExpense = _statisticsService.GetTotalExpense(UserId, FromDate, ToDate);
        PeriodBalance = PeriodIncome - PeriodExpense;

        // Загружаем последние операции (за период)
        RecentOperations.Clear();
        var operations = _operationService.GetOperations(UserId)
            .Where(o => o.Date >= FromDate && o.Date <= ToDate)
            .OrderByDescending(o => o.Date)
            .Take(10);

        foreach (var op in operations)
            RecentOperations.Add(op);

        // Расходы по категориям
        ExpensesByCategory.Clear();
        var expenses = _statisticsService
            .GetExpensesByCategory(UserId, FromDate, ToDate)
            .OrderByDescending(x => x.TotalAmount);

        foreach (var stat in expenses)
        {
            stat.Percentage = PeriodExpense == 0 ? 0 : (double)(stat.TotalAmount / PeriodExpense);
            ExpensesByCategory.Add(stat);
        }

        // Доходы по категориям
        IncomeByCategory.Clear();
        var incomes = _statisticsService
            .GetIncomeByCategory(UserId, FromDate, ToDate)
            .OrderByDescending(x => x.TotalAmount);

        foreach (var stat in incomes)
        {
            stat.Percentage = PeriodIncome == 0 ? 0 : (double)(stat.TotalAmount / PeriodIncome);
            IncomeByCategory.Add(stat);
        }
    }
}