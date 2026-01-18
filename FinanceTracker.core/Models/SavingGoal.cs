using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Core.Models;

public class SavingGoal
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal TargetAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CurrentAmount { get; set; }

    public DateTime? Deadline { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
