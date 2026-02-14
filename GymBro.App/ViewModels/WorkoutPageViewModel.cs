using GymBro.App.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class WorkoutPageViewModel : ViewModelBase
    {
        private ObservableCollection<string> _todayExercises;
        private string _selectedExercise;

        public WorkoutPageViewModel()
        {
            // Заглушка: позже загрузим из базы
            TodayExercises = new ObservableCollection<string>
            {
                "Жим штанги лёжа - 4x8",
                "Приседания со штангой - 4x10",
                "Тяга верхнего блока - 3x12"
            };
        }

        public ObservableCollection<string> TodayExercises
        {
            get => _todayExercises;
            set => SetProperty(ref _todayExercises, value);
        }

        public string SelectedExercise
        {
            get => _selectedExercise;
            set => SetProperty(ref _selectedExercise, value);
        }

        public ICommand StartWorkoutCommand { get; } = new RelayCommand(_ => { /* начать тренировку */ });
        public ICommand CompleteSetCommand { get; } = new RelayCommand(_ => { /* завершить подход */ });
    }
}