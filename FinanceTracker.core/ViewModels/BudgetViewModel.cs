using FinanceTracker.Core.Common;
using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;

namespace FinanceTracker.Core.ViewModels;

public class BudgetViewModel : BaseViewModel
{
    private readonly BudgetService _budgetService;
    private const int UserId = 1;

    /* =========================
     * Collections
     * ========================= */

    public ObservableCollection<Budget> Budgets { get; } = new();
    public ObservableCollection<CategoryBudgetStat> CategoryLimits { get; } = new();
    public ObservableCollection<Category> AvailableCategoriesForLimit { get; } = new();

    /* =========================
     * Selected budget
     * ========================= */

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
            CreateCategoryLimitCommand.RaiseCanExecuteChanged();
            SaveCategoryLimitsCommand.RaiseCanExecuteChanged();
        }
    }

    /* =========================
     * Create category limit
     * ========================= */

    private Category? _selectedCategoryForLimit;
    public Category? SelectedCategoryForLimit
    {
        get => _selectedCategoryForLimit;
        set
        {
            _selectedCategoryForLimit = value;
            OnPropertyChanged();
            CreateCategoryLimitCommand.RaiseCanExecuteChanged();
        }
    }

    private decimal _newCategoryLimitAmount;
    public decimal NewCategoryLimitAmount
    {
        get => _newCategoryLimitAmount;
        set
        {
            _newCategoryLimitAmount = value;
            OnPropertyChanged();
            CreateCategoryLimitCommand.RaiseCanExecuteChanged();
        }
    }

    /* =========================
     * Statistics
     * ========================= */

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

    /* =========================
     * Edit budget
     * ========================= */

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

    /* =========================
     * Create budget
     * ========================= */

    public DateTime? PeriodStart { get; set; } = DateTime.Today;
    public DateTime? PeriodEnd { get; set; } = DateTime.Today;
    public decimal TotalLimitInput { get; set; }

    /* =========================
     * Commands
     * ========================= */

    public RelayCommand CreateBudgetCommand { get; }
    public RelayCommand DeleteBudgetCommand { get; }
    public RelayCommand SaveBudgetCommand { get; }
    public RelayCommand CreateCategoryLimitCommand { get; }
    public RelayCommand SaveCategoryLimitsCommand { get; }

    /* =========================
     * ctor
     * ========================= */

    public BudgetViewModel(BudgetService budgetService)
    {
        _budgetService = budgetService;

        CreateBudgetCommand = new RelayCommand(CreateBudget);

        DeleteBudgetCommand = new RelayCommand(
            DeleteBudget,
            () => SelectedBudget != null
        );

        SaveBudgetCommand = new RelayCommand(
            SaveBudget,
            () => SelectedBudget != null
        );

        CreateCategoryLimitCommand = new RelayCommand(
            CreateCategoryLimit,
            () => SelectedBudget != null
               && SelectedCategoryForLimit != null
               && NewCategoryLimitAmount > 0
        );

        SaveCategoryLimitsCommand = new RelayCommand(
            SaveCategoryLimits,
            () => SelectedBudget != null && CategoryLimits.Any()
        );

        LoadBudgets();
    }

    /* =========================
     * Load / recalc
     * ========================= */

    public void LoadBudgets()
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

        // показываем ТОЛЬКО категории с лимитами
        CategoryLimits.Clear();
        foreach (var stat in _budgetService
            .GetCategoryBudgetStats(SelectedBudget)
            .Where(s => s.LimitAmount > 0))
        {
            CategoryLimits.Add(stat);
        }

        UpdateAvailableCategoriesForLimit();
    }

    private void UpdateAvailableCategoriesForLimit()
    {
        AvailableCategoriesForLimit.Clear();

        if (SelectedBudget == null)
            return;

        var categories = _budgetService
            .GetExpenseCategories(UserId)
            .Where(c => !CategoryLimits.Any(l => l.CategoryId == c.Id));

        foreach (var c in categories)
            AvailableCategoriesForLimit.Add(c);
    }

    /* =========================
     * Budget CRUD
     * ========================= */

    private void CreateBudget()
    {
        if (PeriodStart == null || PeriodEnd == null || TotalLimitInput <= 0)
            return;

        _budgetService.CreateBudget(new CreateBudgetRequest(
            PeriodStart.Value,
            PeriodEnd.Value,
            TotalLimitInput,
            UserId));

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
        if (SelectedBudget == null ||
            EditPeriodStart == null ||
            EditPeriodEnd == null ||
            EditTotalLimit <= 0)
            return;

        SelectedBudget.PeriodStart = EditPeriodStart.Value;
        SelectedBudget.PeriodEnd = EditPeriodEnd.Value;
        SelectedBudget.TotalLimit = EditTotalLimit;

        _budgetService.UpdateBudget(SelectedBudget);
        LoadBudgets();
    }

    /* =========================
     * Category limits
     * ========================= */

    private void CreateCategoryLimit()
    {
        if (SelectedBudget == null || SelectedCategoryForLimit == null)
            return;

        _budgetService.SetCategoryLimit(
            SelectedBudget.Id,
            SelectedCategoryForLimit.Id,
            NewCategoryLimitAmount);

        SelectedCategoryForLimit = null;
        NewCategoryLimitAmount = 0;

        Recalculate();
    }

    private void SaveCategoryLimits()
    {
        Console.WriteLine("Command is work");
        if (SelectedBudget == null)
            return;

        foreach (var limit in CategoryLimits)
        {
            if (limit.LimitAmount <= 0)
                continue;

            _budgetService.SetCategoryLimit(
                SelectedBudget.Id,
                limit.CategoryId,
                limit.LimitAmount);
        }

        Recalculate();
    }
}
