using GymBro.Domain.Entities;
using System.Windows;

namespace GymBro.App.Views
{
    public partial class UserProfileWindow : Window
    {
        public UserProfileWindow(UserProfile user)
        {
            InitializeComponent();
            var vm = new ViewModels.UserProfileViewModel(user);
            vm.CloseRequested += (s, result) => { DialogResult = result; Close(); };
            DataContext = vm;
        }
    }
}