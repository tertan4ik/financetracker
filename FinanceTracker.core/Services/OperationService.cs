using FinanceTracker.Core.Contracts;
using FinanceTracker.Core.Data;
using FinanceTracker.Core.Models;
using FinanceTracker.core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Core.Services;

public class OperationService
{
    private readonly FinanceTrackerDbContext _db;

    public OperationService(FinanceTrackerDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Получить операции пользователя
    /// </summary>
    public IReadOnlyList<Operation> GetOperations(int userId)
    {
        return _db.Operations
            .Include(o => o.Category)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Date)
            .AsNoTracking()
            .ToList();
    }

    /// <summary>
    /// Создание операции (MAUI-ready)
    /// </summary>
    public Operation CreateOperation(CreateOperationRequest request)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        var category = _db.Categories.FirstOrDefault(c => c.Id == request.CategoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found");

        var operation = new Operation
        {
            Amount = request.Amount,
            Date = request.Date,
            CategoryId = request.CategoryId,
            UserId = request.UserId,
            Comment = request.Comment,

            // ❗ ТИП ОПЕРАЦИИ ОПРЕДЕЛЯЕТ CORE
            Type = category.Type == CategoryType.Доход
                ? OperationType.Доход
                : OperationType.Расход
        };

        _db.Operations.Add(operation);
        _db.SaveChanges();

        return operation;
    }

    /// <summary>
    /// Редактирование операции
    /// </summary>
    public void UpdateOperation(Operation operation)
    {
        var existing = _db.Operations.FirstOrDefault(o => o.Id == operation.Id);
        if (existing == null)
            throw new InvalidOperationException("Operation not found");

        var category = _db.Categories.FirstOrDefault(c => c.Id == operation.CategoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found");

        existing.Amount = operation.Amount;
        existing.Date = operation.Date;
        existing.Comment = operation.Comment;
        existing.CategoryId = operation.CategoryId;

        // ❗ Тип снова вычисляется, UI не влияет
        existing.Type = category.Type == CategoryType.Доход
            ? OperationType.Доход
            : OperationType.Расход;

        _db.SaveChanges();
    }

    /// <summary>
    /// Удаление операции
    /// </summary>
    public void DeleteOperation(int operationId)
    {
        var operation = _db.Operations.FirstOrDefault(o => o.Id == operationId);
        if (operation == null)
            return;

        _db.Operations.Remove(operation);
        _db.SaveChanges();
    }
}
