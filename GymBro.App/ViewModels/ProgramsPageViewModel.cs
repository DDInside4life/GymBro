using GymBro.App.Commands;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using GymBro.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class ProgramsPageViewModel : ViewModelBase
    {
        private readonly UserProfileManager _userManager;
        private readonly TrainingProgramManager _programManager;

        private ObservableCollection<UserProfile> _users;
        private UserProfile _selectedUser;
        private ObservableCollection<TrainingProgram> _programs;
        private TrainingProgram _selectedProgram;
        private bool _isLoading;

        public ProgramsPageViewModel()
        {
            var factory = new ManagersFactory();
            _userManager = factory.GetUserProfileManager();
            _programManager = factory.GetTrainingProgramManager();

            Users = new ObservableCollection<UserProfile>();
            Programs = new ObservableCollection<TrainingProgram>();

            // Загружаем пользователей
            LoadUsers();

            // Команды
            AddUserCommand = new RelayCommand(ExecuteAddUser);
            EditUserCommand = new RelayCommand(ExecuteEditUser, CanEditUser);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser, CanDeleteUser);

            AddProgramCommand = new RelayCommand(ExecuteAddProgram, CanAddProgram);
            EditProgramCommand = new RelayCommand(ExecuteEditProgram, CanEditProgram);
            DeleteProgramCommand = new RelayCommand(ExecuteDeleteProgram, CanDeleteProgram);
        }

        public ObservableCollection<UserProfile> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public UserProfile SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    LoadProgramsAsync();
                }
            }
        }

        public ObservableCollection<TrainingProgram> Programs
        {
            get => _programs;
            set => SetProperty(ref _programs, value);
        }

        public TrainingProgram SelectedProgram
        {
            get => _selectedProgram;
            set => SetProperty(ref _selectedProgram, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public ICommand AddProgramCommand { get; }
        public ICommand EditProgramCommand { get; }
        public ICommand DeleteProgramCommand { get; }

        private void LoadUsers()
        {
            try
            {
                var users = _userManager.GetAllUserProfiles().ToList();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                if (Users.Any())
                {
                    SelectedUser = Users.First();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка");
            }
        }

        private async Task LoadProgramsAsync()
        {
            if (SelectedUser == null)
            {
                Programs.Clear();
                return;
            }

            try
            {
                IsLoading = true;
                var programs = await _programManager.GetProgramsByUserAsync(SelectedUser.Id);
                Programs.Clear();
                foreach (var prog in programs)
                {
                    Programs.Add(prog);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки программ: {ex.Message}", "Ошибка");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ---------- Команды для пользователей ----------
        private void ExecuteAddUser(object param)
        {
            var newUser = Views.EditUserProfileWindow.ShowDialog(owner: Application.Current.MainWindow);
            if (newUser != null)
            {
                _userManager.CreateUserProfile(newUser);
                Users.Add(newUser);
                SelectedUser = newUser;
            }
        }

        private bool CanEditUser(object param) => SelectedUser != null;

        private void ExecuteEditUser(object param)
        {
            var updatedUser = Views.EditUserProfileWindow.ShowDialog(SelectedUser, Application.Current.MainWindow);
            if (updatedUser != null)
            {
                // Сохраняем Id исходного пользователя
                updatedUser.Id = SelectedUser.Id;
                _userManager.UpdateUserProfile(updatedUser);

                // Обновляем в коллекции
                var index = Users.IndexOf(SelectedUser);
                Users[index] = updatedUser;
                SelectedUser = updatedUser;
            }
        }

        private bool CanDeleteUser(object param) => SelectedUser != null;

        private void ExecuteDeleteUser(object param)
        {
            var result = MessageBox.Show($"Удалить пользователя '{SelectedUser.Name}'?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_userManager.DeleteUserProfile(SelectedUser.Id))
                {
                    Users.Remove(SelectedUser);
                    SelectedUser = Users.FirstOrDefault();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить пользователя.", "Ошибка");
                }
            }
        }

        // ---------- Команды для программ ----------
        private bool CanAddProgram(object param) => SelectedUser != null;

        private void ExecuteAddProgram(object param)
        {
            var newProgram = Views.EditTrainingProgramWindow.ShowDialog(owner: Application.Current.MainWindow);
            if (newProgram != null)
            {
                newProgram.UserProfileId = SelectedUser.Id;
                _programManager.CreateTrainingProgram(newProgram);
                Programs.Add(newProgram);
                SelectedProgram = newProgram;
            }
        }

        private bool CanEditProgram(object param) => SelectedProgram != null;

        private void ExecuteEditProgram(object param)
        {
            var updatedProgram = Views.EditTrainingProgramWindow.ShowDialog(SelectedProgram, Application.Current.MainWindow);
            if (updatedProgram != null)
            {
                updatedProgram.Id = SelectedProgram.Id;
                updatedProgram.UserProfileId = SelectedProgram.UserProfileId;
                _programManager.UpdateTrainingProgram(updatedProgram);

                var index = Programs.IndexOf(SelectedProgram);
                Programs[index] = updatedProgram;
                SelectedProgram = updatedProgram;
            }
        }

        private bool CanDeleteProgram(object param) => SelectedProgram != null;

        private void ExecuteDeleteProgram(object param)
        {
            var result = MessageBox.Show($"Удалить программу '{SelectedProgram.Name}'?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_programManager.DeleteTrainingProgram(SelectedProgram.Id))
                {
                    Programs.Remove(SelectedProgram);
                    SelectedProgram = Programs.FirstOrDefault();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить программу.", "Ошибка");
                }
            }
        }
    }
}