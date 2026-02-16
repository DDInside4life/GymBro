using GymBro.App.Commands;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GymBro.App.ViewModels
{
    public class ExercisesPageViewModel : ViewModelBase
    {
        private readonly ExerciseManager _exerciseManager;
        private ObservableCollection<Exercise> _exercises;
        private Exercise _selectedExercise;
        private bool _isLoading;

        public ExercisesPageViewModel()
        {
            var factory = new ManagersFactory();
            _exerciseManager = factory.GetExerciseManager();
            Exercises = new ObservableCollection<Exercise>();

            LoadExercisesAsync();
        }

        public ObservableCollection<Exercise> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set => SetProperty(ref _selectedExercise, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private async Task LoadExercisesAsync()
        {
            try
            {
                IsLoading = true;
                var exercises = await _exerciseManager.GetAllExercisesAsync();
                Exercises.Clear();
                foreach (var ex in exercises)
                {
                    Exercises.Add(ex);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки упражнений: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}