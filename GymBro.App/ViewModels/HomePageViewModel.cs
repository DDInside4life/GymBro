using System;
using System.Globalization;

namespace GymBro.App.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private string _currentDate;

        public HomePageViewModel()
        {
            // Форматируем текущую дату на русском языке (можно заменить на текущую культуру)
            CurrentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy", new CultureInfo("ru-RU"));
        }

        public string CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }
    }
}