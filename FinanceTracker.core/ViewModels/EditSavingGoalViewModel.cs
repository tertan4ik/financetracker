using FinanceTracker.Core.Common;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;

namespace FinanceTracker.Core.ViewModels;

public class EditSavingGoalViewModel : BaseViewModel
{
    private readonly SavingGoalService _savingGoalService;

    public SavingGoal Goal { get; }

    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private decimal _targetAmount;
    public decimal TargetAmount
    {
        get => _targetAmount;
        set { _targetAmount = value; OnPropertyChanged(); }
    }

    private decimal _currentAmount;
    public decimal CurrentAmount
    {
        get => _currentAmount;
        set { _currentAmount = value; OnPropertyChanged(); }
    }

    public RelayCommand SaveCommand { get; }

    public EditSavingGoalViewModel(
        SavingGoal goal,
        SavingGoalService savingGoalService)
    {
        Goal = goal;
        _savingGoalService = savingGoalService;

        Name = goal.Name;
        TargetAmount = goal.TargetAmount;
        CurrentAmount = goal.CurrentAmount;

        SaveCommand = new RelayCommand(Save);
    }

    private void Save()
    {
        Goal.Name = Name;
        Goal.TargetAmount = TargetAmount;
        Goal.CurrentAmount = CurrentAmount;

        _savingGoalService.UpdateGoal(Goal);
    }
}
