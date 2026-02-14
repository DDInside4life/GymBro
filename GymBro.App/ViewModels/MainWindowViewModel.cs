//using GymBro.App.Commands;
//using GymBro.Business.Infrastructure; // добавьте using
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.IO;
//using System.Windows;
//using System.Windows.Input;

//namespace GymBro.App.ViewModels
//{
//    public class MainWindowViewModel : ViewModelBase
//    {
//        public event EventHandler<string> NavigationRequested;

//        private ICommand _navigateCommand;
//        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand(ExecuteNavigate);

//        public MainWindowViewModel()
//        {
//            // Принудительно инициализируем фабрику и менеджер, чтобы создать БД
//            InitializeDatabase();

//            // остальной код (если есть)
//        }

//        //private void InitializeDatabase()
//        //{
//        //    try
//        //    {
//        //        var factory = new ManagersFactory(); // читает строку из appsettings.json
//        //        var userManager = factory.GetUserProfileManager();
//        //        // вызываем любой метод, чтобы активировать доступ к БД
//        //        var users = userManager.GetAllUserProfiles();
//        //        System.Diagnostics.Debug.WriteLine($"База данных инициализирована. Найдено пользователей: {users.Count()}");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        System.Diagnostics.Debug.WriteLine("Ошибка при инициализации БД: " + ex);
//        //        // можно показать сообщение пользователю
//        //    }
//        //}

//        private void InitializeDatabase()
//        {
//            try
//            {
//                System.Diagnostics.Debug.WriteLine("InitializeDatabase started");

//                var configuration = new ConfigurationBuilder()
//                    .SetBasePath(Directory.GetCurrentDirectory())
//                    .AddJsonFile("appsettings.json")
//                    .Build();

//                var connectionString = configuration.GetConnectionString("DefaultConnection");
//                System.Diagnostics.Debug.WriteLine("Connection string: " + connectionString);

//                var options = new DbContextOptionsBuilder<GymBro.DAL.Data.GymBroContext>()
//                    .UseSqlServer(connectionString)
//                    .Options;

//                using (var context = new GymBro.DAL.Data.GymBroContext(options))
//                {
//                    System.Diagnostics.Debug.WriteLine("Calling EnsureCreated...");
//                    var created = context.Database.EnsureCreated();
//                    System.Diagnostics.Debug.WriteLine("EnsureCreated returned: " + created);

//                    if (created)
//                    {
//                        System.Diagnostics.Debug.WriteLine("Filling with test data...");
//                        GymBro.Business.Infrastructure.DbInitializer.Initialize(context);
//                        System.Diagnostics.Debug.WriteLine("Test data added.");
//                    }
//                    else
//                    {
//                        System.Diagnostics.Debug.WriteLine("Database already exists.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine("ERROR in InitializeDatabase: " + ex.ToString());
//                MessageBox.Show("Ошибка инициализации БД: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }


//        private void ExecuteNavigate(object parameter)
//        {
//            string pageKey = parameter?.ToString();
//            if (!string.IsNullOrEmpty(pageKey))
//            {
//                NavigationRequested?.Invoke(this, pageKey);
//            }
//        }
//    }
//}

using System;
using System.Windows.Input;
using GymBro.App.Commands;



namespace GymBro.App.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {

        public event EventHandler<string> NavigationRequested;

        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand(ExecuteNavigate);

        private void ExecuteNavigate(object parameter)
        {
            string pageKey = parameter?.ToString();
            if (!string.IsNullOrEmpty(pageKey))
            {
                NavigationRequested?.Invoke(this, pageKey);
            }
        }
    }
}