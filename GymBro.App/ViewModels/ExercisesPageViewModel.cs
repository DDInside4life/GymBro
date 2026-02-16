using GymBro.App.Commands;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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

            ViewExerciseCommand = new RelayCommand(ExecuteViewExercise, CanViewExercise);
        }

        public ObservableCollection<Exercise> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set
            {
                if (SetProperty(ref _selectedExercise, value))
                {
                    // Можно загрузить детали, если нужно
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ViewExerciseCommand { get; }

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

        private bool CanViewExercise(object param) => SelectedExercise != null;
        private void ExecuteViewExercise(object param)
        {
            // Открыть детали упражнения, например, показать сообщение или открыть окно
            string equipment = SelectedExercise.Equipment != null && SelectedExercise.Equipment.Any()
                ? string.Join(", ", SelectedExercise.Equipment.Select(e => e.Name))
                : "нет";
            System.Windows.MessageBox.Show(
                $"{SelectedExercise.Name}\n\nОписание: {SelectedExercise.Description}\n" +
                $"Подходы: {SelectedExercise.DefaultSets} x {SelectedExercise.DefaultRepsMin}-{SelectedExercise.DefaultRepsMax}\n" +
                $"Оборудование: {equipment}\n\n" +
                $"Советы: {SelectedExercise.TechniqueTips}\n" +
                $"Ошибки: {SelectedExercise.CommonMistakes}",
                "Детали упражнения");
        }
    }
}