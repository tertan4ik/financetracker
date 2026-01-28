using FinanceTracker.Core.Common;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Services;
using System.Collections.ObjectModel;

namespace FinanceTracker.Core.ViewModels;

public class SavingGoalsViewModel : BaseViewModel
{
    private readonly SavingGoalService _service;

    // ❗ временно, как у тебя в проекте
    private const int UserId = 1;

    // ===== СПИСОК ЦЕЛЕЙ =====
    public ObservableCollection<SavingGoal> Goals { get; } = new();

    // ===== ВЫБРАННАЯ ЦЕЛЬ (ТОЛЬКО ДЛЯ УДАЛЕНИЯ) =====
    private SavingGoal? _selectedGoal;
    public SavingGoal? SelectedGoal
    {
        get => _selectedGoal;
        set
        {
            _selectedGoal = value;
            OnPropertyChanged();
            DeleteGoalCommand.RaiseCanExecuteChanged();
        }
    }

    // ===== ПОЛЯ ДЛЯ СОЗДАНИЯ =====
    private string _newName = string.Empty;
    public string NewName
    {
        get => _newName;
        set { _newName = value; OnPropertyChanged(); }
    }

    private decimal _newTargetAmount;
    public decimal NewTargetAmount
    {
        get => _newTargetAmount;
        set { _newTargetAmount = value; OnPropertyChanged(); }
    }

    private decimal _newCurrentAmount;
    public decimal NewCurrentAmount
    {
        get => _newCurrentAmount;
        set { _newCurrentAmount = value; OnPropertyChanged(); }
    }

    private DateTime? _newDeadline;
    public DateTime? NewDeadline
    {
        get => _newDeadline;
        set { _newDeadline = value; OnPropertyChanged(); }
    }

    // ===== КОМАНДЫ =====
    public RelayCommand CreateGoalCommand { get; }
    public RelayCommand DeleteGoalCommand { get; }

    public RelayCommand SaveGoalCommand { get; }

    public SavingGoalsViewModel(SavingGoalService service)
    {
        _service = service;

        CreateGoalCommand = new RelayCommand(CreateGoal);
        DeleteGoalCommand = new RelayCommand(DeleteGoal, () => SelectedGoal != null);
        SaveGoalCommand = new RelayCommand(SaveGoal);
        LoadGoals();
    }

    // ===== ЗАГРУЗКА ЦЕЛЕЙ =====
    public void LoadGoals()
    {
        Goals.Clear();
        foreach (var goal in _service.GetGoals(UserId))
            Goals.Add(goal);

        // 🔥 ничего не выбираем автоматически
        SelectedGoal = null;
    }

    // ===== СОЗДАНИЕ ЦЕЛИ =====
    private void CreateGoal()
    {
        if (string.IsNullOrWhiteSpace(NewName))
            return;

        if (NewTargetAmount <= 0)
            return;

        if (NewCurrentAmount < 0)
            return;

        _service.CreateGoal(
            NewName,
            NewTargetAmount,
            NewCurrentAmount,
            NewDeadline,
            UserId);

        // 🔥 ОЧИСТКА ФОРМЫ
        NewName = string.Empty;
        NewTargetAmount = 0;
        NewCurrentAmount = 0;
        NewDeadline = null;

        LoadGoals();
    }

    // ===== УДАЛЕНИЕ ЦЕЛИ =====
    private void DeleteGoal()
    {
        if (SelectedGoal == null)
            return;

        _service.DeleteGoal(SelectedGoal.Id);
        LoadGoals();
    }
    public void SetSelectedGoal(SavingGoal? goal)
    {
        SelectedGoal = goal;

        if (goal == null)
        {
            // ➕ добавление
            NewName = string.Empty;
            NewTargetAmount = 0;
            NewCurrentAmount = 0;
            NewDeadline = null;
        }
        else
        {
            // ✏️ редактирование
            NewName = goal.Name;
            NewTargetAmount = goal.TargetAmount;
            NewCurrentAmount = goal.CurrentAmount;
            NewDeadline = goal.Deadline;
        }
    }
    private void SaveGoal()
    {
        Console.WriteLine("Save method executed");
        Console.WriteLine(NewName);
        if (string.IsNullOrWhiteSpace(NewName))
     
        return;
        Console.WriteLine("Name not white space");
        if (NewTargetAmount <= 0)
            return;
        Console.WriteLine("Target amount over zero");
        if (NewCurrentAmount < 0)
            return;
        Console.WriteLine("Ccurrent amount over zero");
        if (SelectedGoal == null)
        {
            Console.WriteLine("Creating Goal in view model");
            // ➕ добавление
            _service.CreateGoal(
                NewName,
                NewTargetAmount,
                NewCurrentAmount,
                NewDeadline,
                UserId);
        }
        else
        {
            // ✏️ редактирование
            Console.WriteLine("Edit Goal in view model");
            SelectedGoal.Name = NewName;
            SelectedGoal.TargetAmount = NewTargetAmount;
            SelectedGoal.CurrentAmount = NewCurrentAmount;
            SelectedGoal.Deadline = NewDeadline;

            _service.UpdateGoal(SelectedGoal);
        }

        LoadGoals();
    }

}
