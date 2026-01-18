namespace FinanceTracker.Core.Contracts;

public record CreateOperationRequest(
    decimal Amount,
    DateTime Date,
    int CategoryId,
    int UserId,
    string? Comment
);
