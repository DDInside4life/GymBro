using GymBro.App.ViewModels;
using GymBro.Domain.Entities;
using System.Windows;
using System.Windows.Controls;
using GymBro.App.Views;

namespace GymBro.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Подписываемся на событие навигации из ViewModel
            if (DataContext is MainWindowViewModel vm)
            {
                vm.NavigationRequested += OnNavigationRequested;
                // Инициализируем стартовую страницу
                vm.NavigateCommand.Execute("Home");
            }
        }

        private void OnNavigationRequested(object sender, string pageKey)
        {
            // Загружаем соответствующую страницу по ключу
            switch (pageKey)
            {
                case "Home":
                    var homePage = new Pages.HomePage();

                    // Подписка на события HomePage
                    homePage.StartWorkoutClicked += (s, e) =>
                    {
                        var workoutWindow = new WorkoutSessionWindow(); // откроется окно тренировки
                        workoutWindow.Owner = this;
                        workoutWindow.ShowDialog();
                    };

                    homePage.ViewProgramClicked += (s, e) =>
                    {
                        var programWindow = new ProgramSelectionWindow(); // откроется окно выбора программы
                        programWindow.Owner = this;
                        programWindow.ShowDialog();
                    };

                    homePage.ViewProgressClicked += (s, e) =>
                    {
                        // Если нужно открыть страницу прогресса (например, уже есть ProgressPage)
                        MainContentFrame.Navigate(new Pages.ProgressPage());
                    };

                    MainContentFrame.Navigate(homePage);
                    break;
                case "Workout":
                    MainContentFrame.Navigate(new Pages.WorkoutPage());
                    break;
                case "Programs":
                    MainContentFrame.Navigate(new Pages.ProgramsPage());
                    break;
                case "Exercises":
                    MainContentFrame.Navigate(new Pages.ExercisesPage());
                    break;
                case "Progress":
                    MainContentFrame.Navigate(new Pages.ProgressPage());
                    break;
                case "Settings":
                    MainContentFrame.Navigate(new Pages.SettingsPage());
                    break;
                case "Profile":
                    var currentUser = new UserProfile
                    {
                        Name = "Кулеш Роман",
                        Age = 25,
                        Weight = 80,
                        Height = 180,
                        BirthDate = new DateTime(1998, 5, 15),
                        FitnessLevel = "Продвинутый",
                        TrainingGoal = "Набор массы"
                    };
                    var profileWindow = new UserProfileWindow(currentUser);
                    profileWindow.Owner = this;
                    profileWindow.ShowDialog();
                    break;

            }
        }
    }
}