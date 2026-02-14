using GymBro.App.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class ExercisesPageViewModel : ViewModelBase
    {
        private ObservableCollection<string> _exercises;
        private string _selectedExercise;

        public ExercisesPageViewModel()
        {
            Exercises = new ObservableCollection<string>
            {
                "Жим штанги лёжа",
                "Приседания со штангой",
                "Тяга верхнего блока",
                "Бег на беговой дорожке"
            };
        }

        public ObservableCollection<string> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public string SelectedExercise
        {
            get => _selectedExercise;
            set => SetProperty(ref _selectedExercise, value);
        }

        public ICommand ViewExerciseCommand { get; } = new RelayCommand(_ => { /* показать детали */ });
    }
}