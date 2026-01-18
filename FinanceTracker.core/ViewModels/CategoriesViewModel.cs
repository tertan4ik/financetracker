using FinanceTracker.Core.Common;
using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.core.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceTracker.Core.ViewModels;

public class CategoriesViewModel : BaseViewModel
{
    private readonly CategoryService _categoryService;
    private readonly int _userId;

    public ObservableCollection<Category> Categories { get; } = new();

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
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

            // 🔥 ВОТ ЭТИ СТРОКИ ВКЛЮЧАЮТ КНОПКИ
            UpdateCategoryCommand.RaiseCanExecuteChanged();
            DeleteCategoryCommand.RaiseCanExecuteChanged();
        }
    }


    public RelayCommand AddCategoryCommand { get; }
    public RelayCommand UpdateCategoryCommand { get; }
    public RelayCommand DeleteCategoryCommand { get; }

    public CategoriesViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;
        _userId = CurrentUser.Id;

        SelectedCategoryType = CategoryType.Expense;

        AddCategoryCommand = new RelayCommand(AddCategory);
        UpdateCategoryCommand = new RelayCommand(UpdateCategory, () => SelectedCategory != null);
        DeleteCategoryCommand = new RelayCommand(DeleteCategory, () => SelectedCategory != null);

        LoadCategories();
    }

    private void LoadCategories()
    {
        Categories.Clear();
        foreach (var category in _categoryService.GetCategories(_userId))
            Categories.Add(category);
    }

    private void AddCategory()
    {
        var category = _categoryService.CreateCategory(
            new CreateCategoryRequest(
                Name,
                SelectedCategoryType,
                _userId
            )
        );

        Categories.Add(category);
        Name = string.Empty;
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
        if (SelectedCategory == null)
            return;

        _categoryService.DeleteCategory(SelectedCategory.Id);
        Categories.Remove(SelectedCategory);
        SelectedCategory = null;
    }
}
