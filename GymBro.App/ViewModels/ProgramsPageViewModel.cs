using GymBro.App.Commands;
using GymBro.App.Infrastructure;
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
        private readonly bool _isAdmin;
        private readonly int? _currentUserId;

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

            _isAdmin = SessionManager.IsInRole("Admin");
            _currentUserId = SessionManager.CurrentUser?.UserProfileId; // предполагаем, что у User есть UserProfileId

            Users = new ObservableCollection<UserProfile>();
            Programs = new ObservableCollection<TrainingProgram>();

            LoadInitialData();

            AddUserCommand = new RelayCommand(ExecuteAddUser, _ => _isAdmin);
            EditUserCommand = new RelayCommand(ExecuteEditUser, _ => _isAdmin && SelectedUser != null);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser, _ => _isAdmin && SelectedUser != null);

            AddProgramCommand = new RelayCommand(ExecuteAddProgram, CanAddProgram);
            EditProgramCommand = new RelayCommand(ExecuteEditProgram, CanEditProgram);
            DeleteProgramCommand = new RelayCommand(ExecuteDeleteProgram, CanDeleteProgram);
        }

        // Свойства для привязки
        public ObservableCollection<UserProfile> Users { get => _users; set => SetProperty(ref _users, value); }
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
        public ObservableCollection<TrainingProgram> Programs { get => _programs; set => SetProperty(ref _programs, value); }
        public TrainingProgram SelectedProgram { get => _selectedProgram; set => SetProperty(ref _selectedProgram, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        public bool IsAdmin => _isAdmin;

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand AddProgramCommand { get; }
        public ICommand EditProgramCommand { get; }
        public ICommand DeleteProgramCommand { get; }

        private void LoadInitialData()
        {
            if (_isAdmin)
            {
                LoadAllUsers();
            }
            else
            {
                // Для обычного пользователя показываем только его профиль
                if (_currentUserId.HasValue)
                {
                    var user = _userManager.GetUserProfileById(_currentUserId.Value);
                    if (user != null)
                    {
                        Users.Add(user);
                        SelectedUser = user;
                    }
                }
                else
                {
                    // Если у пользователя нет профиля? По идее должен быть.
                }
            }
        }

        private void LoadAllUsers()
        {
            try
            {
                var users = _userManager.GetAllUserProfiles().ToList();
                Users.Clear();
                foreach (var user in users) Users.Add(user);
                if (Users.Any()) SelectedUser = Users.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
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
                foreach (var prog in programs) Programs.Add(prog);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки программ: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Команды для пользователей (только админ)
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

        private void ExecuteEditUser(object param)
        {
            var updatedUser = Views.EditUserProfileWindow.ShowDialog(SelectedUser, Application.Current.MainWindow);
            if (updatedUser != null)
            {
                updatedUser.Id = SelectedUser.Id;
                _userManager.UpdateUserProfile(updatedUser);
                var index = Users.IndexOf(SelectedUser);
                Users[index] = updatedUser;
                SelectedUser = updatedUser;
            }
        }

        private void ExecuteDeleteUser(object param)
        {
            if (!_isAdmin) return;
            var result = MessageBox.Show($"Удалить пользователя '{SelectedUser.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_userManager.DeleteUserProfile(SelectedUser.Id))
                {
                    Users.Remove(SelectedUser);
                    SelectedUser = Users.FirstOrDefault();
                }
                else MessageBox.Show("Не удалось удалить пользователя.");
            }
        }

        // Команды для программ
        private bool CanAddProgram(object param)
        {
            if (_isAdmin) return SelectedUser != null;
            else return true; // обычный пользователь может добавлять себе программу (текущий пользователь)
        }

        private void ExecuteAddProgram(object param)
        {
            if (_isAdmin)
            {
                // Для админа открываем обычное окно редактирования с пустой программой
                var newProgram = Views.EditTrainingProgramWindow.ShowDialog(owner: Application.Current.MainWindow);
                if (newProgram != null)
                {
                    newProgram.UserProfileId = SelectedUser.Id;
                    _programManager.CreateTrainingProgram(newProgram);
                    Programs.Add(newProgram);
                    SelectedProgram = newProgram;
                }
            }
            else
            {
                // Для обычного пользователя открываем окно выбора шаблона
                var template = Views.ProgramSelectionWindow.ShowDialog(Application.Current.MainWindow);
                if (template != null)
                {
                    // Создаём программу на основе шаблона
                    var newProgram = new TrainingProgram
                    {
                        Name = template.Name,
                        Description = template.Description,
                        ProgramType = template.ProgramType,
                        DurationWeeks = template.DurationWeeks,
                        Difficulty = template.Difficulty,
                        WorkoutsPerWeek = template.WorkoutsPerWeek,
                        CreatedDate = DateTime.Now,
                        UserProfileId = _currentUserId.Value,
                        IsTemplate = false
                    };
                    _programManager.CreateTrainingProgram(newProgram);
                    Programs.Add(newProgram);
                    SelectedProgram = newProgram;
                }
            }
        }

        private bool CanEditProgram(object param)
        {
            if (_isAdmin) return SelectedProgram != null;
            else return SelectedProgram != null && SelectedProgram.UserProfileId == _currentUserId;
        }

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

        private bool CanDeleteProgram(object param)
        {
            if (_isAdmin) return SelectedProgram != null;
            else return SelectedProgram != null && SelectedProgram.UserProfileId == _currentUserId;
        }

        private void ExecuteDeleteProgram(object param)
        {
            var program = SelectedProgram;
            if (program == null) return;
            if (!_isAdmin && program.UserProfileId != _currentUserId) return;

            var result = MessageBox.Show($"Удалить программу '{program.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_programManager.DeleteTrainingProgram(program.Id))
                {
                    Programs.Remove(program);
                    SelectedProgram = Programs.FirstOrDefault();
                }
                else MessageBox.Show("Не удалось удалить программу.");
            }
        }
    }
}