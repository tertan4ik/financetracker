using FinanceTracker.Core.Common;
using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class OperationsViewModel : BaseViewModel
{
    private readonly OperationService _operationService;
    private readonly CategoryService _categoryService;

    private const int UserId = 1;

    public ObservableCollection<Operation> Operations { get; } = new();
    public ObservableCollection<Category> Categories { get; } = new();

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set { _amount = value; OnPropertyChanged(); }
    }

    private DateTime _date = DateTime.Today;
    public DateTime Date
    {
        get => _date;
        set { _date = value; OnPropertyChanged(); }
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
        set { _selectedCategory = value; OnPropertyChanged(); }
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


    public RelayCommand AddOperationCommand { get; }
    public RelayCommand UpdateOperationCommand { get; }
    public RelayCommand DeleteOperationCommand { get; }

    public OperationsViewModel(
        OperationService operationService,
        CategoryService categoryService)
    {
        _operationService = operationService;
        _categoryService = categoryService;

        AddOperationCommand = new RelayCommand(AddOperation);
        UpdateOperationCommand = new RelayCommand(UpdateOperation, () => SelectedOperation != null);
        DeleteOperationCommand = new RelayCommand(DeleteOperation, () => SelectedOperation != null);

        LoadData();
    }

    private void LoadData()
    {
        Categories.Clear();
        foreach (var category in _categoryService.GetCategories(UserId))
            Categories.Add(category);

        Operations.Clear();
        foreach (var operation in _operationService.GetOperations(UserId))
            Operations.Add(operation);
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
        Amount = 0;
        Comment = null;
        Date = DateTime.Today;
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
        LoadData();
    }

    private void DeleteOperation()
    {
        if (SelectedOperation == null)
            return;

        _operationService.DeleteOperation(SelectedOperation.Id);
        Operations.Remove(SelectedOperation);
        SelectedOperation = null;
    }
}
