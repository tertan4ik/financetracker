using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Data;
using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Core.Services;

public class CategoryService
{
    private readonly FinanceTrackerDbContext _db;

    public CategoryService(FinanceTrackerDbContext db)
    {
        _db = db;
    }


    /// <summary>
    /// Получить категории пользователя
    /// </summary>
    public IReadOnlyList<Category> GetCategories(int userId)
    {
        return _db.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .AsNoTracking()
            .ToList();
    }

    /// <summary>
    /// Создание категории (MAUI-ready)
    /// </summary>
    public Category CreateCategory(CreateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Category name is required");
       

        var category = new Category
        {
            Name = request.Name.Trim(),
            Type = request.Type,
            UserId = request.UserId
        };

        _db.Categories.Add(category);
        _db.SaveChanges();

        return category;
    }

    /// <summary>
    /// Редактирование категории
    /// </summary>
    public void UpdateCategory(Category category)
    {
        var existing = _db.Categories.FirstOrDefault(c => c.Id == category.Id);
        if (existing == null)
            throw new InvalidOperationException("Category not found");

        existing.Name = category.Name.Trim();
        existing.Type = category.Type;

        _db.SaveChanges();
    }

    /// <summary>
    /// Удаление категории
    /// </summary>
    public void DeleteCategory(int categoryId)
    {
        var category = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category == null)
            return;

        _db.Categories.Remove(category);
        _db.SaveChanges();
    }
}
