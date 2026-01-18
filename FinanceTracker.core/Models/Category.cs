using FinanceTracker.core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Core.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public CategoryType Type { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    public ICollection<BudgetCategoryLimit> BudgetCategoryLimits { get; set; } = new List<BudgetCategoryLimit>();
}
