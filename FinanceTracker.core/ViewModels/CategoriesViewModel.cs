using FinanceTracker.Core.Common;
using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.core.Enums;
using System.Collections.ObjectModel;

namespace FinanceTracker.Core.ViewModels;

public class CategoriesViewModel : BaseViewModel
{
    private readonly CategoryService _categoryService;
    private readonly int _userId;

    public ObservableCollection<Category> Categories { get; } = new();

    public IEnumerable<Category> VisibleCategories =>
        CurrentMode == CategoryViewMode.Income
            ? Categories.Where(c => c.Type == CategoryType.Доход)
            : Categories.Where(c => c.Type == CategoryType.Расход);

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); Validate(); AddCategoryCommand.RaiseCanExecuteChanged();  }
    }

    private CategoryType _selectedCategoryType;
    public CategoryType SelectedCategoryType
    {
        get => _selectedCategoryType;
        set { _selectedCategoryType = value; OnPropertyChanged(); }
    }

    public Array CategoryTypes => Enum.GetValues(typeof(CategoryType));

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            OnPropertyChanged();

            if (value != null)
            {
                Name = value.Name;
                SelectedCategoryType = value.Type;
            }

            UpdateCategoryCommand.RaiseCanExecuteChanged();
            DeleteCategoryCommand.RaiseCanExecuteChanged();
        }
    }
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
        IsValid = !string.IsNullOrWhiteSpace(Name);
    }


    public RelayCommand AddCategoryCommand { get; }
    public RelayCommand UpdateCategoryCommand { get; }
    public RelayCommand DeleteCategoryCommand { get; }
    public RelayCommand SetIncomeModeCommand { get; }
    public RelayCommand SetExpenseModeCommand { get; }

    public CategoriesViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;
        _userId = CurrentUser.Id;

        SelectedCategoryType = CategoryType.Расход;

        AddCategoryCommand = new RelayCommand(AddCategory, () => IsValid);
        UpdateCategoryCommand = new RelayCommand(UpdateCategory, () => SelectedCategory != null);
        DeleteCategoryCommand = new RelayCommand(DeleteCategory, () => SelectedCategory != null);
        SetIncomeModeCommand = new RelayCommand(() => CurrentMode = CategoryViewMode.Income);
        SetExpenseModeCommand = new RelayCommand(() => CurrentMode = CategoryViewMode.Expense);

        LoadCategories();
    }

    public void LoadCategories()
    {
        Categories.Clear();
        foreach (var category in _categoryService.GetCategories(_userId))
            Categories.Add(category);

        OnPropertyChanged(nameof(VisibleCategories));
    }

    private void AddCategory()
    {
        var category = _categoryService.CreateCategory(
            new CreateCategoryRequest(Name, SelectedCategoryType, _userId));

        Categories.Add(category);
        Name = string.Empty;
        LoadCategories();
    }

    private void UpdateCategory()
    {
        if (SelectedCategory == null)
            return;

        SelectedCategory.Name = Name;
        SelectedCategory.Type = SelectedCategoryType;

        _categoryService.UpdateCategory(SelectedCategory);
        LoadCategories();
    }

    private void DeleteCategory()
    {
        Console.WriteLine("BUTTON PRESSED");
        if (SelectedCategory == null)
            return;

        _categoryService.DeleteCategory(SelectedCategory.Id);
        Categories.Remove(SelectedCategory);
        SelectedCategory = null;
        LoadCategories();
    }

    private CategoryViewMode _currentMode = CategoryViewMode.Expense;
    public CategoryViewMode CurrentMode
    {
        get => _currentMode;
        set
        {
            if (_currentMode == value)
                return;

            _currentMode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(VisibleCategories));
        }
    }
}
