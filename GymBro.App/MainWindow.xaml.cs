using System.Windows;
using System.Windows.Controls;
using GymBro.App.ViewModels;

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
                    MainContentFrame.Navigate(new Pages.HomePage());
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
                    // Открываем окно профиля (не страницу)
                    var profileWindow = new UserProfileWindow();
                    profileWindow.Owner = this;
                    profileWindow.ShowDialog();
                    break;
            }
        }
    }
}