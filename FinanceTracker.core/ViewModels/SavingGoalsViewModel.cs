using FinanceTracker.Core.Common;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;

namespace FinanceTracker.Core.ViewModels;

public class SavingGoalsViewModel : BaseViewModel
{
    private readonly SavingGoalService _service;
    private const int UserId = 1;

    public ObservableCollection<SavingGoal> Goals { get; } = new();

    private SavingGoal? _selectedGoal;
    public SavingGoal? SelectedGoal
    {
        get => _selectedGoal;
        set
        {
            _selectedGoal = value;
            OnPropertyChanged();

            if (value != null)
            {
                EditName = value.Name;
                EditTargetAmount = value.TargetAmount;
                EditCurrentAmount = value.CurrentAmount;
            }

            DeleteGoalCommand.RaiseCanExecuteChanged();
            SaveGoalCommand.RaiseCanExecuteChanged();
        }
    }

    public string EditName { get; set; } = string.Empty;
    public decimal EditTargetAmount { get; set; }
    public decimal EditCurrentAmount { get; set; }

    public RelayCommand CreateGoalCommand { get; }
    public RelayCommand SaveGoalCommand { get; }
    public RelayCommand DeleteGoalCommand { get; }

    public SavingGoalsViewModel(SavingGoalService service)
    {
        _service = service;

        CreateGoalCommand = new RelayCommand(CreateGoal);
        SaveGoalCommand = new RelayCommand(SaveGoal, () => SelectedGoal != null);
        DeleteGoalCommand = new RelayCommand(DeleteGoal, () => SelectedGoal != null);

        LoadGoals();
    }

    private void LoadGoals()
    {
        Goals.Clear();
        foreach (var g in _service.GetGoals(UserId))
            Goals.Add(g);

        SelectedGoal = Goals.FirstOrDefault();
    }

    private void CreateGoal()
    {
        if (string.IsNullOrWhiteSpace(EditName) || EditTargetAmount <= 0)
            return;

        _service.CreateGoal(EditName, EditTargetAmount, UserId);
        LoadGoals();
    }

    private void SaveGoal()
    {
        if (SelectedGoal == null)
            return;

        SelectedGoal.Name = EditName;
        SelectedGoal.TargetAmount = EditTargetAmount;
        SelectedGoal.CurrentAmount = EditCurrentAmount;

        _service.UpdateGoal(SelectedGoal);
        LoadGoals();
    }

    private void DeleteGoal()
    {
        if (SelectedGoal == null)
            return;

        _service.DeleteGoal(SelectedGoal.Id);
        LoadGoals();
    }
}
