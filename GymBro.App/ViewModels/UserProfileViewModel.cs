using GymBro.App.Commands;
using GymBro.Domain.Entities;
using System;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        private UserProfile _user;

        public UserProfileViewModel(UserProfile user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            SaveCommand = new RelayCommand(ExecuteSave, CanSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        public string Name { get => _user.Name; set { _user.Name = value; OnPropertyChanged(); } }
        public int Age { get => _user.Age; set { _user.Age = value; OnPropertyChanged(); } }
        public decimal Weight { get => _user.Weight; set { _user.Weight = value; OnPropertyChanged(); } }
        public decimal Height { get => _user.Height; set { _user.Height = value; OnPropertyChanged(); } }
        public DateTime BirthDate { get => _user.BirthDate; set { _user.BirthDate = value; OnPropertyChanged(); } }
        public string FitnessLevel { get => _user.FitnessLevel; set { _user.FitnessLevel = value; OnPropertyChanged(); } }
        public string TrainingGoal { get => _user.TrainingGoal; set { _user.TrainingGoal = value; OnPropertyChanged(); } }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanSave(object param) => !string.IsNullOrWhiteSpace(Name) && Age > 0 && Weight > 0 && Height > 0;

        private void ExecuteSave(object param) => CloseRequested?.Invoke(this, true);
        private void ExecuteCancel(object param) => CloseRequested?.Invoke(this, false);

        public event EventHandler<bool> CloseRequested;
    }
}