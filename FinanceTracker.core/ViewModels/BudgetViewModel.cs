using FinanceTracker.Core.Common;
using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class BudgetViewModel : BaseViewModel
{
    private readonly BudgetService _budgetService;
    private const int UserId = 1;

    public ObservableCollection<Budget> Budgets { get; } = new();
    public ObservableCollection<CategoryBudgetStat> CategoryLimits { get; }
    = new();

    private Budget? _selectedBudget;
    public Budget? SelectedBudget
    {
        get => _selectedBudget;
        set
        {
            _selectedBudget = value;
            OnPropertyChanged();

            if (_selectedBudget != null)
            {
                EditPeriodStart = _selectedBudget.PeriodStart;
                EditPeriodEnd = _selectedBudget.PeriodEnd;
                EditTotalLimit = _selectedBudget.TotalLimit;
            }

            Recalculate();
            DeleteBudgetCommand.RaiseCanExecuteChanged();
            SaveBudgetCommand.RaiseCanExecuteChanged();
        }
    }


    private CategoryBudgetStat? _selectedCategoryLimit;
    public CategoryBudgetStat? SelectedCategoryLimit
    {
        get => _selectedCategoryLimit;
        set
        {
            _selectedCategoryLimit = value;
            OnPropertyChanged();
            SaveCategoryLimitCommand.RaiseCanExecuteChanged();
        }
    }


    private decimal _totalExpenses;
    public decimal TotalExpenses
    {
        get => _totalExpenses;
        set { _totalExpenses = value; OnPropertyChanged(); }
    }

    private decimal _usagePercent;
    public decimal UsagePercent
    {
        get => _usagePercent;
        set { _usagePercent = value; OnPropertyChanged(); }
    }

    private bool _isExceeded;
    public bool IsExceeded
    {
        get => _isExceeded;
        set { _isExceeded = value; OnPropertyChanged(); }
    }



    private DateTime? _editPeriodStart;
    public DateTime? EditPeriodStart
    {
        get => _editPeriodStart;
        set { _editPeriodStart = value; OnPropertyChanged(); }
    }

    private DateTime? _editPeriodEnd;
    public DateTime? EditPeriodEnd
    {
        get => _editPeriodEnd;
        set { _editPeriodEnd = value; OnPropertyChanged(); }
    }

    private decimal _editTotalLimit;
    public decimal EditTotalLimit
    {
        get => _editTotalLimit;
        set { _editTotalLimit = value; OnPropertyChanged(); }
    }

    public DateTime? PeriodStart { get; set; } = DateTime.Today;
    public DateTime? PeriodEnd { get; set; } = DateTime.Today;
    public decimal TotalLimitInput { get; set; }

    public RelayCommand CreateBudgetCommand { get; }
    public RelayCommand DeleteBudgetCommand { get; }

    public RelayCommand SaveBudgetCommand { get; }
    public RelayCommand SaveCategoryLimitCommand { get; }

    public BudgetViewModel(BudgetService budgetService)
    {
        _budgetService = budgetService;
        CreateBudgetCommand = new RelayCommand(CreateBudget);
        DeleteBudgetCommand = new RelayCommand(DeleteBudget, () => SelectedBudget != null);
        SaveBudgetCommand = new RelayCommand(
          SaveBudget,
          () => SelectedBudget != null
        );


        SaveCategoryLimitCommand = new RelayCommand(
            SaveCategoryLimit,
            () => SelectedCategoryLimit != null && SelectedBudget != null
        );
        LoadBudgets();
    }

    private void LoadBudgets()
    {
        Budgets.Clear();
        foreach (var b in _budgetService.GetBudgets(UserId))
            Budgets.Add(b);

        SelectedBudget = Budgets.FirstOrDefault();
    }

    private void Recalculate()
    {
        if (SelectedBudget == null)
            return;

        TotalExpenses = _budgetService.GetTotalExpenses(SelectedBudget);
        UsagePercent = _budgetService.GetUsagePercent(SelectedBudget);
        IsExceeded = _budgetService.IsBudgetExceeded(SelectedBudget);

        CategoryLimits.Clear();
        foreach (var stat in _budgetService.GetCategoryBudgetStats(SelectedBudget))
            CategoryLimits.Add(stat);
    }


    private void CreateBudget()
    {
        if (PeriodStart == null || PeriodEnd == null)
            return;

        if (TotalLimitInput <= 0)
            return;

        _budgetService.CreateBudget(new CreateBudgetRequest(
            PeriodStart.Value,
            PeriodEnd.Value,
            TotalLimitInput,
            UserId
        ));

        LoadBudgets();
    }
    private void DeleteBudget()
    {
        if (SelectedBudget == null)
            return;

        _budgetService.DeleteBudget(SelectedBudget.Id);

        LoadBudgets();
    }

    private void SaveBudget()
    {
        if (SelectedBudget == null)
            return;

        if (EditPeriodStart == null || EditPeriodEnd == null)
            return;

        if (EditTotalLimit <= 0)
            return;

        SelectedBudget.PeriodStart = EditPeriodStart.Value;
        SelectedBudget.PeriodEnd = EditPeriodEnd.Value;
        SelectedBudget.TotalLimit = EditTotalLimit;

        _budgetService.UpdateBudget(SelectedBudget);

        LoadBudgets();
    }

    private void SaveCategoryLimit()
    {
        if (SelectedBudget == null || SelectedCategoryLimit == null)
            return;

        _budgetService.SetCategoryLimit(
            SelectedBudget.Id,
            SelectedCategoryLimit.CategoryId,
            SelectedCategoryLimit.LimitAmount
        );

        Recalculate();
    }



}
