using FinanceTracker.Core.Common;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using FinanceTracker.core.Enums;

namespace FinanceTracker.Core.ViewModels;

public class EditCategoryViewModel : BaseViewModel
{
    private readonly CategoryService _categoryService;

    public Category Category { get; }

    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private CategoryType _type;
    public CategoryType SelectedCategoryType
    {
        get => _type;
        set { _type = value; OnPropertyChanged(); }
    }

    public Array CategoryTypes => Enum.GetValues(typeof(CategoryType));

    public RelayCommand SaveCommand { get; }

    public EditCategoryViewModel(Category category, CategoryService categoryService)
    {
        Category = category;
        _categoryService = categoryService;

        Name = category.Name;
        SelectedCategoryType = category.Type;

        SaveCommand = new RelayCommand(Save);
    }

    private void Save()
    {
        Category.Name = Name;
        Category.Type = SelectedCategoryType;

        _categoryService.UpdateCategory(Category);

    }
}
