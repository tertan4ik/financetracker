using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Core.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();
}
