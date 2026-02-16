using GymBro.App.Commands;
using GymBro.App.Infrastructure;
using GymBro.Business.Infrastructure;
using GymBro.Business.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;

namespace GymBro.App.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private string _selectedLanguage;

        public SettingsPageViewModel()
        {
            var factory = new ManagersFactory();
            _userManager = factory.GetUserManager();

            AvailableLanguages = new List<string> { "ru", "en" };
            SelectedLanguage = SessionManager.CurrentUser?.Language ?? "ru";

            SaveCommand = new RelayCommand(ExecuteSave);
        }

        public List<string> AvailableLanguages { get; }
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public ICommand SaveCommand { get; }

        private void ExecuteSave(object param)
        {
            var user = SessionManager.CurrentUser;
            if (user != null && user.Language != SelectedLanguage)
            {
                user.Language = SelectedLanguage;
                _userManager.UpdateUserLanguage(user.Id, SelectedLanguage); // добавим метод

                // Применяем язык сейчас (простейший способ – перезапустить приложение)
                System.Windows.MessageBox.Show("Язык изменён. Перезапустите приложение для полного применения.", "Настройки");
            }
        }
    }
}