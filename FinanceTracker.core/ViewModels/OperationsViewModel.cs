using FinanceTracker.core.Enums;
using FinanceTracker.Core.Common;
using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class OperationsViewModel : BaseViewModel
{
    private readonly OperationService _operationService;
    private readonly CategoryService _categoryService;

    private const int UserId = 1;

    public ObservableCollection<Operation> Operations { get; } = new();
    public ObservableCollection<Category> Categories { get; } = new();
    public ObservableCollection<Category> FilteredCategories { get; } = new();



    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set { _amount = value; OnPropertyChanged(); Validate();
            AddOperationCommand.RaiseCanExecuteChanged(); 
        }
    }

    private DateTime _date = DateTime.Today;
    public DateTime Date
    {
        get => _date;
        set { _date = value; OnPropertyChanged(); Validate();
            AddOperationCommand.RaiseCanExecuteChanged();
        }
    }

    private string? _comment;
    public string? Comment
    {
        get => _comment;
        set { _comment = value; OnPropertyChanged(); }
    }

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set { _selectedCategory = value; OnPropertyChanged(); Validate();
            AddOperationCommand.RaiseCanExecuteChanged();
        }
    }

    private Operation? _selectedOperation;
    public Operation? SelectedOperation
    {
        get => _selectedOperation;
        set
        {
            _selectedOperation = value;
            OnPropertyChanged();

            if (value != null)
            {
                Amount = value.Amount;
                Date = value.Date;
                Comment = value.Comment;
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == value.CategoryId);
            }

            // 🔥 АКТИВАЦИЯ КНОПОК
            UpdateOperationCommand.RaiseCanExecuteChanged();
            DeleteOperationCommand.RaiseCanExecuteChanged();
        }
    }

    private OperationViewMode _currentMode = OperationViewMode.Expense;
    public OperationViewMode CurrentMode
    {
        get => _currentMode;
        set
        {
            if (_currentMode == value)
                return;

            _currentMode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(VisibleOperations));
        }
    }



    private CategoryType _selectedOperationType = CategoryType.Расход;
    public CategoryType SelectedOperationType
    {
        get => _selectedOperationType;
        set
        {
            if (_selectedOperationType == value)
                return;

            _selectedOperationType = value;
            OnPropertyChanged();

            SelectedCategory = null;

            // 🔥 КЛЮЧЕВО
            UpdateFilteredCategories();
        }
    }


    public IEnumerable<Operation> VisibleOperations =>
    CurrentMode switch
    {
        OperationViewMode.Income =>
            Operations.Where(o => o.Category.Type == CategoryType.Доход),

        OperationViewMode.Expense =>
            Operations.Where(o => o.Category.Type == CategoryType.Расход),

        _ => Operations
    };

    public Array OperationTypes => Enum.GetValues(typeof(CategoryType));

    private bool _isValid;
    public bool HasErrors => !IsValid;
    public bool IsValid
    {
        get => _isValid;
        private set
        {
            _isValid = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    private void Validate()
    {
        IsValid =
            Amount > 0 &&
            SelectedCategory != null &&
            Date != default;
    }
    public RelayCommand AddOperationCommand { get; }
    public RelayCommand UpdateOperationCommand { get; }
    public RelayCommand DeleteOperationCommand { get; }
    public RelayCommand SetIncomeModeCommand { get; }
    public RelayCommand SetExpenseModeCommand { get; }
    public RelayCommand SetAllModeCommand { get; }


    public OperationsViewModel(
        OperationService operationService,
        CategoryService categoryService)
    {
        _operationService = operationService;
        _categoryService = categoryService;

        AddOperationCommand = new RelayCommand(AddOperation,()=> IsValid);
        UpdateOperationCommand = new RelayCommand(UpdateOperation, () => SelectedOperation != null);
        DeleteOperationCommand = new RelayCommand(DeleteOperation, () => SelectedOperation != null);
        SetIncomeModeCommand = new RelayCommand(() => CurrentMode = OperationViewMode.Income);
        SetExpenseModeCommand = new RelayCommand(() => CurrentMode = OperationViewMode.Expense);
        SetAllModeCommand = new RelayCommand(() => CurrentMode = OperationViewMode.All);

        LoadData();
    }

    public void LoadData()
    {
        Categories.Clear();
        foreach (var category in _categoryService.GetCategories(UserId))
            Categories.Add(category);

        // 🔥 ОБЯЗАТЕЛЬНО
        UpdateFilteredCategories();
        Console.WriteLine("DATA IS LOADED");
        Operations.Clear();
        foreach (var operation in _operationService.GetOperations(UserId))
            Operations.Add(operation);
        UpdateFilteredCategories();
        RefreshVisibleOperations();
        Console.WriteLine("DATA IS LOADED");
    }



    private void AddOperation()
    {
        if (SelectedCategory == null)
            return;

        var operation = _operationService.CreateOperation(
            new CreateOperationRequest(
                Amount,
                Date,
                SelectedCategory.Id,
                UserId,
                Comment
            )
        );

        Operations.Insert(0, operation);

        // Очистка формы
        Amount = 0;
        Comment = null;
        Date = DateTime.Today;
        SelectedCategory = null;
        UpdateFilteredCategories();
    }


    private void UpdateOperation()
    {
        if (SelectedOperation == null || SelectedCategory == null)
            return;

        SelectedOperation.Amount = Amount;
        SelectedOperation.Date = Date;
        SelectedOperation.Comment = Comment;
        SelectedOperation.CategoryId = SelectedCategory.Id;

        _operationService.UpdateOperation(SelectedOperation);
        RefreshVisibleOperations();
    }

    private void DeleteOperation()
    {
        if (SelectedOperation == null)
            return;

        _operationService.DeleteOperation(SelectedOperation.Id);
        Operations.Remove(SelectedOperation);
        SelectedOperation = null;
        RefreshVisibleOperations();
    }

    private void UpdateFilteredCategories()
    {
        FilteredCategories.Clear();

        foreach (var category in Categories.Where(c => c.Type == SelectedOperationType))
            FilteredCategories.Add(category);
        Debug.WriteLine($"Filtered: {FilteredCategories.Count}");
        RefreshVisibleOperations();
    }

    private void RefreshVisibleOperations()
    {
        OnPropertyChanged(nameof(VisibleOperations));
    }

}
