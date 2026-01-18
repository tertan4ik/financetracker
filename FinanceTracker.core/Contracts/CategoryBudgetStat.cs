namespace FinanceTracker.Core.Contracts;

public class CategoryBudgetStat
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public decimal LimitAmount { get; set; }
    public decimal SpentAmount { get; set; }

    public bool IsExceeded => LimitAmount > 0 && SpentAmount > LimitAmount;
}
