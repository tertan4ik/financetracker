namespace FinanceTracker.Core.Contracts;

public record CreateBudgetRequest(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal TotalLimit,
    int UserId
);
