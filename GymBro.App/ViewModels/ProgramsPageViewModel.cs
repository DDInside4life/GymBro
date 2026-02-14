using GymBro.App.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class ProgramsPageViewModel : ViewModelBase
    {
        private ObservableCollection<string> _users;
        private string _selectedUser;
        private ObservableCollection<string> _programs;

        public ProgramsPageViewModel()
        {
            Users = new ObservableCollection<string> { "Кулеш Роман", "Анна Смирнова", "Дмитрий Петров" };
            Programs = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public string SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    // Загружаем программы выбранного пользователя
                    LoadPrograms();
                }
            }
        }

        public ObservableCollection<string> Programs
        {
            get => _programs;
            set => SetProperty(ref _programs, value);
        }

        private void LoadPrograms()
        {
            Programs.Clear();
            if (SelectedUser == null) return;
            // Заглушка
            if (SelectedUser.Contains("Роман"))
                Programs.Add("Силовая программа (12 недель)");
            else if (SelectedUser.Contains("Анна"))
                Programs.Add("Кардио для похудения (8 недель)");
            else
                Programs.Add("Фулбоди для начинающих (6 недель)");
        }

        public ICommand AddUserCommand { get; } = new RelayCommand(_ => { });
        public ICommand DeleteUserCommand { get; } = new RelayCommand(_ => { });
        public ICommand AddProgramCommand { get; } = new RelayCommand(_ => { });
        public ICommand DeleteProgramCommand { get; } = new RelayCommand(_ => { });
    }
}