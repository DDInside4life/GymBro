//using GymBro.Domain.Entities;
//using System;
//using System.Windows;
//using System.Windows.Input;

//namespace GymBro.App
//{
//    public partial class EditTrainingProgramWindow : Window
//    {
//        public EditTrainingProgramWindow()
//        {
//            InitializeComponent();
//            DataContext = this;
//        }

//        // Dependency Properties
//        public static readonly DependencyProperty NameProperty =
//            DependencyProperty.Register("Name", typeof(string), typeof(EditTrainingProgramWindow));

//        public string Name
//        {
//            get => (string)GetValue(NameProperty);
//            set => SetValue(NameProperty, value);
//        }

//        public static readonly DependencyProperty DescriptionProperty =
//            DependencyProperty.Register("Description", typeof(string), typeof(EditTrainingProgramWindow));

//        public string Description
//        {
//            get => (string)GetValue(DescriptionProperty);
//            set => SetValue(DescriptionProperty, value);
//        }

//        public static readonly DependencyProperty ProgramTypeProperty =
//            DependencyProperty.Register("ProgramType", typeof(string), typeof(EditTrainingProgramWindow));

//        public string ProgramType
//        {
//            get => (string)GetValue(ProgramTypeProperty);
//            set => SetValue(ProgramTypeProperty, value);
//        }

//        public static readonly DependencyProperty DurationWeeksProperty =
//            DependencyProperty.Register("DurationWeeks", typeof(int), typeof(EditTrainingProgramWindow));

//        public int DurationWeeks
//        {
//            get => (int)GetValue(DurationWeeksProperty);
//            set => SetValue(DurationWeeksProperty, value);
//        }

//        public static readonly DependencyProperty DifficultyProperty =
//            DependencyProperty.Register("Difficulty", typeof(int), typeof(EditTrainingProgramWindow));

//        public int Difficulty
//        {
//            get => (int)GetValue(DifficultyProperty);
//            set => SetValue(DifficultyProperty, value);
//        }

//        public static readonly DependencyProperty WorkoutsPerWeekProperty =
//            DependencyProperty.Register("WorkoutsPerWeek", typeof(int), typeof(EditTrainingProgramWindow));

//        public int WorkoutsPerWeek
//        {
//            get => (int)GetValue(WorkoutsPerWeekProperty);
//            set => SetValue(WorkoutsPerWeekProperty, value);
//        }

//        // Команды
//        private ICommand _okCommand;
//        public ICommand OkCommand => _okCommand ??= new Commands.RelayCommand(ExecuteOk, CanExecuteOk);

//        private ICommand _cancelCommand;
//        public ICommand CancelCommand => _cancelCommand ??= new Commands.RelayCommand(ExecuteCancel);

//        private bool CanExecuteOk(object parameter)
//        {
//            return !string.IsNullOrWhiteSpace(Name) &&
//                   !string.IsNullOrWhiteSpace(ProgramType) &&
//                   DurationWeeks > 0 && DurationWeeks <= 52 &&
//                   Difficulty >= 1 && Difficulty <= 5 &&
//                   WorkoutsPerWeek >= 1 && WorkoutsPerWeek <= 7;
//        }

//        private void ExecuteOk(object parameter)
//        {
//            DialogResult = true;
//            Close();
//        }

//        private void ExecuteCancel(object parameter)
//        {
//            DialogResult = false;
//            Close();
//        }

//        // Обработчики событий кнопок
//        private void OkButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (CanExecuteOk(null))
//            {
//                DialogResult = true;
//                Close();
//            }
//        }

//        private void CancelButton_Click(object sender, RoutedEventArgs e)
//        {
//            DialogResult = false;
//            Close();
//        }

//        // Статический метод для открытия окна
//        public static TrainingProgram ShowDialog(TrainingProgram program = null, Window owner = null)
//        {
//            var window = new EditTrainingProgramWindow();

//            if (owner != null)
//            {
//                window.Owner = owner;
//            }

//            // Если передана существующая программа - заполняем поля
//            if (program != null)
//            {
//                window.Name = program.Name;
//                window.Description = program.Description;
//                window.ProgramType = program.ProgramType;
//                window.DurationWeeks = program.DurationWeeks;
//                window.Difficulty = program.Difficulty;
//                window.WorkoutsPerWeek = program.WorkoutsPerWeek;
//            }
//            else
//            {
//                // Значения по умолчанию для новой программы
//                window.DurationWeeks = 8;
//                window.Difficulty = 3;
//                window.WorkoutsPerWeek = 3;
//                window.ProgramType = "Силовая";
//            }

//            if (window.ShowDialog() == true)
//            {
//                return new TrainingProgram
//                {
//                    Name = window.Name,
//                    Description = window.Description,
//                    ProgramType = window.ProgramType,
//                    DurationWeeks = window.DurationWeeks,
//                    Difficulty = window.Difficulty,
//                    WorkoutsPerWeek = window.WorkoutsPerWeek,
//                    CreatedDate = DateTime.Now
//                };
//            }

//            return null;
//        }
//    }
//}