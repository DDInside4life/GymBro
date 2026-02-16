using GymBro.App.Infrastructure;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GymBro.App.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly TrainingProgramManager _programManager;
        private readonly ExerciseManager _exerciseManager;
        private string _currentDate;
        private string _programName;
        private ObservableCollection<ExerciseDisplay> _exercises;
        private bool _isLoading;

        public HomePageViewModel()
        {
            var factory = new ManagersFactory();
            _programManager = factory.GetTrainingProgramManager();
            _exerciseManager = factory.GetExerciseManager();

            CurrentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy", new CultureInfo("ru-RU"));
            Exercises = new ObservableCollection<ExerciseDisplay>();

            LoadDataAsync();
        }

        public string CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        public string ProgramName
        {
            get => _programName;
            set => SetProperty(ref _programName, value);
        }

        public ObservableCollection<ExerciseDisplay> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool HasProgram => !string.IsNullOrEmpty(ProgramName) && Exercises.Any();

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                var currentUser = SessionManager.CurrentUser;
                if (currentUser?.UserProfileId == null)
                {
                    ProgramName = "Войдите в систему";
                    return;
                }

                int? programId = SessionManager.SelectedProgramId;
                TrainingProgram program = null;

                if (programId.HasValue)
                {
                    program = await _programManager.GetTrainingProgramByIdAsync(programId.Value);
                }
                else
                {
                    // Если программа не выбрана, берём первую доступную (например, свою)
                    var programs = await _programManager.GetProgramsByUserAsync(currentUser.UserProfileId.Value);
                    program = programs.FirstOrDefault();
                }

                if (program == null)
                {
                    ProgramName = "Нет активных программ";
                    return;
                }

                ProgramName = program.Name;
                var exercises = await _exerciseManager.GetExercisesByProgramAsync(program.Id);
                Exercises.Clear();
                foreach (var ex in exercises)
                {
                    Exercises.Add(new ExerciseDisplay
                    {
                        Name = ex.Name,
                        Sets = ex.DefaultSets,
                        RepsMin = ex.DefaultRepsMin,
                        RepsMax = ex.DefaultRepsMax,
                        Equipment = ex.Equipment?.Select(e => e.Name) ?? Enumerable.Empty<string>()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    public class ExerciseDisplay
    {
        public string Name { get; set; }
        public int Sets { get; set; }
        public int RepsMin { get; set; }
        public int RepsMax { get; set; }
        public IEnumerable<string> Equipment { get; set; }
        public string SetsReps => $"{Sets} x {RepsMin}-{RepsMax}";
        public string EquipmentString => Equipment != null && Equipment.Any() ? string.Join(", ", Equipment) : "нет";
    }
}