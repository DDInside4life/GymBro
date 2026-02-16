using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows;

namespace GymBro.App.ViewModels
{
    public class ExercisesPageViewModel : ViewModelBase
    {
        private readonly ExerciseManager _exerciseManager;
        private ObservableCollection<Exercise> _exercises;
        private Exercise _selectedExercise;
        private bool _isLoading;
        private readonly EquipmentManager _equipmentManager;
        private readonly bool _isAdmin;

        public ICommand AddExerciseCommand { get; }

        public ExercisesPageViewModel()
        {
            var factory = new ManagersFactory();
            _exerciseManager = factory.GetExerciseManager();
            _equipmentManager = factory.GetEquipmentManager();
            _isAdmin = SessionManager.IsInRole("Admin");
            Exercises = new ObservableCollection<Exercise>();
            LoadExercisesAsync();
            AddExerciseCommand = new RelayCommand(ExecuteAddExercise, _ => _isAdmin);
        }

        private async void ExecuteAddExercise(object param)
        {
            var allEquipment = (await _equipmentManager.GetAllEquipmentAsync()).ToList();
            var dialog = new Views.EditExerciseWindow(null, allEquipment);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                var newExercise = new Exercise
                {
                    Name = dialog.ExerciseName,
                    Description = dialog.ExerciseDescription,
                    DefaultSets = dialog.DefaultSets,
                    DefaultRepsMin = dialog.RepsMin,
                    DefaultRepsMax = dialog.RepsMax,
                    RestBetweenSets = TimeSpan.FromSeconds(dialog.RestSeconds),
                    TechniqueTips = dialog.TechniqueTips,
                    CommonMistakes = dialog.CommonMistakes,
                    Equipment = dialog.SelectedEquipment
                };
                await _exerciseManager.CreateExerciseAsync(newExercise);
                await LoadExercisesAsync(); // перезагрузить список
            }
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