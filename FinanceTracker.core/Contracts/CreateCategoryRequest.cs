using FinanceTracker.core.Enums;

namespace FinanceTracker.Core.Contracts;

public record CreateCategoryRequest(
    string Name,
    CategoryType Type,
    int UserId
);