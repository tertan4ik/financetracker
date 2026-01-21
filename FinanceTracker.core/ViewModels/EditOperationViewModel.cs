using FinanceTracker.Core.Common;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.core.Enums;
using System.Collections.ObjectModel;

namespace FinanceTracker.Core.ViewModels;

public class EditOperationViewModel : BaseViewModel
{
    private readonly OperationService _operationService;
    private readonly CategoryService _categoryService;

    public Operation Operation { get; }

    public ObservableCollection<Category> Categories { get; } = new();
    public ObservableCollection<Category> FilteredCategories { get; } = new();

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set { _amount = value; OnPropertyChanged(); }
    }

    private DateTime _date;
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

    private CategoryType _selectedOperationType;
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
            UpdateFilteredCategories();
        }
    }

    public Array OperationTypes => Enum.GetValues(typeof(CategoryType));

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set { _selectedCategory = value; OnPropertyChanged(); }
    }

    public RelayCommand SaveCommand { get; }

    public EditOperationViewModel(
        Operation operation,
        OperationService operationService,
        CategoryService categoryService,
        int userId)
    {
        Operation = operation;
        _operationService = operationService;
        _categoryService = categoryService;

        Amount = operation.Amount;
        Date = operation.Date;
        Comment = operation.Comment;

        foreach (var c in _categoryService.GetCategories(userId))
            Categories.Add(c);

        SelectedOperationType = operation.Category.Type;
        SelectedCategory = Categories.FirstOrDefault(c => c.Id == operation.CategoryId);

        SaveCommand = new RelayCommand(Save);
    }

    private void UpdateFilteredCategories()
    {
        FilteredCategories.Clear();

        foreach (var c in Categories.Where(c => c.Type == SelectedOperationType))
            FilteredCategories.Add(c);
    }

    private void Save()
    {
        if (SelectedCategory == null)
            return;

        Operation.Amount = Amount;
        Operation.Date = Date;
        Operation.Comment = Comment;
        Operation.CategoryId = SelectedCategory.Id;

        _operationService.UpdateOperation(Operation);
    }
}
