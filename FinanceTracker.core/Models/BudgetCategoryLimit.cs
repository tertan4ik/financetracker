using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Core.Models;

public class BudgetCategoryLimit
{
    [Key]
    public int Id { get; set; }

    [Range(0, double.MaxValue)]
    public decimal LimitAmount { get; set; }

    [ForeignKey(nameof(Budget))]
    public int BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
