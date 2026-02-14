//using System.Configuration;
//using System.Data;
//using System.Windows;

//namespace GymBro.App
//{
//    /// <summary>
//    /// Interaction logic for App.xaml
//    /// </summary>
//    public partial class App : Application
//    {
//    }

//}

using GymBro.Business.Infrastructure;
using GymBro.App.Views;
using System.Windows;

namespace GymBro.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация базы данных
            try
            {
                var factory = new ManagersFactory();
                factory.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}\n\nInner: {ex.InnerException?.Message}");
                // Можно продолжить или завершить
            }

            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                try
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании главного окна: {ex.Message}\n\n{ex.StackTrace}");
                    Shutdown();
                }
            }
            else
            {
                Shutdown();
            }
        }
    }
}