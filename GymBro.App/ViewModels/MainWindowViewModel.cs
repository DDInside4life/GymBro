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