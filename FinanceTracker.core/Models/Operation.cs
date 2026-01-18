using FinanceTracker.core.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Core.Models;

public class Operation
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(255)]
    public string? Comment { get; set; }

    [Required]
    public OperationType Type { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
