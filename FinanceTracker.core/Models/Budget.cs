using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Core.Models;

public class Budget
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime PeriodStart { get; set; }

    [Required]
    public DateTime PeriodEnd { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalLimit { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<BudgetCategoryLimit> CategoryLimits { get; set; } = new List<BudgetCategoryLimit>();
}
