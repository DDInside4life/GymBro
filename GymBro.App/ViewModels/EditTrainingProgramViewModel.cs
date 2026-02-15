using GymBro.App.Commands;
using GymBro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class EditTrainingProgramViewModel : ViewModelBase
    {
        private string _name;
        private string _description;
        private string _programType;
        private int _durationWeeks;
        private int _difficulty;
        private int _workoutsPerWeek;
        private readonly List<string> _programTypes = new() { "Силовая", "Кардио", "Фулбоди", "Сплит" };

        public EditTrainingProgramViewModel(TrainingProgram program = null)
        {
            if (program != null)
            {
                _name = program.Name;
                _description = program.Description;
                _programType = program.ProgramType;
                _durationWeeks = program.DurationWeeks;
                _difficulty = program.Difficulty;
                _workoutsPerWeek = program.WorkoutsPerWeek;
            }
            else
            {
                _durationWeeks = 8;
                _difficulty = 3;
                _workoutsPerWeek = 3;
                _programType = "Силовая";
            }
            OkCommand = new RelayCommand(ExecuteOk, CanOk);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string ProgramType { get => _programType; set => SetProperty(ref _programType, value); }
        public int DurationWeeks { get => _durationWeeks; set => SetProperty(ref _durationWeeks, value); }
        public int Difficulty { get => _difficulty; set => SetProperty(ref _difficulty, value); }
        public int WorkoutsPerWeek { get => _workoutsPerWeek; set => SetProperty(ref _workoutsPerWeek, value); }
        public List<string> ProgramTypes => _programTypes;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanOk(object param) =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(ProgramType) &&
            DurationWeeks > 0 && Difficulty >= 1 && Difficulty <= 5 && WorkoutsPerWeek >= 1 && WorkoutsPerWeek <= 7;

        private void ExecuteOk(object param) => CloseRequested?.Invoke(this, true);
        private void ExecuteCancel(object param) => CloseRequested?.Invoke(this, false);

        public event EventHandler<bool> CloseRequested;
    }
}