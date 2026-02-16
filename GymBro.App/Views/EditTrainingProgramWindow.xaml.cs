using GymBro.App.Commands;
using GymBro.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Input;
using GymBro.App.Views;

namespace GymBro.App.Views
{
    public partial class EditTrainingProgramWindow : Window
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(EditTrainingProgramWindow));
        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(EditTrainingProgramWindow));
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty ProgramTypeProperty =
            DependencyProperty.Register("ProgramType", typeof(string), typeof(EditTrainingProgramWindow));
        public string ProgramType
        {
            get => (string)GetValue(ProgramTypeProperty);
            set => SetValue(ProgramTypeProperty, value);
        }

        public static readonly DependencyProperty DurationWeeksProperty =
            DependencyProperty.Register("DurationWeeks", typeof(int), typeof(EditTrainingProgramWindow));
        public int DurationWeeks
        {
            get => (int)GetValue(DurationWeeksProperty);
            set => SetValue(DurationWeeksProperty, value);
        }

        public static readonly DependencyProperty DifficultyProperty =
            DependencyProperty.Register("Difficulty", typeof(int), typeof(EditTrainingProgramWindow));
        public int Difficulty
        {
            get => (int)GetValue(DifficultyProperty);
            set => SetValue(DifficultyProperty, value);
        }

        public static readonly DependencyProperty WorkoutsPerWeekProperty =
            DependencyProperty.Register("WorkoutsPerWeek", typeof(int), typeof(EditTrainingProgramWindow));
        public int WorkoutsPerWeek
        {
            get => (int)GetValue(WorkoutsPerWeekProperty);
            set => SetValue(WorkoutsPerWeekProperty, value);
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public EditTrainingProgramWindow()
        {
            InitializeComponent();
            DataContext = this;

            OkCommand = new RelayCommand(ExecuteOk, CanExecuteOk);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private bool CanExecuteOk(object param)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(ProgramType) &&
                   DurationWeeks > 0 &&
                   Difficulty >= 1 && Difficulty <= 5 &&
                   WorkoutsPerWeek >= 1 && WorkoutsPerWeek <= 7;
        }

        private void ExecuteOk(object param)
        {
            DialogResult = true;
            Close();
        }

        private void ExecuteCancel(object param)
        {
            DialogResult = false;
            Close();
        }

        public static TrainingProgram ShowDialog(TrainingProgram program = null, Window owner = null)
        {
            var window = new EditTrainingProgramWindow
            {
                Owner = owner
            };

            if (program != null)
            {
                window.Name = program.Name;
                window.Description = program.Description;
                window.ProgramType = program.ProgramType;
                window.DurationWeeks = program.DurationWeeks;
                window.Difficulty = program.Difficulty;
                window.WorkoutsPerWeek = program.WorkoutsPerWeek;
            }
            else
            {
                window.DurationWeeks = 8;
                window.Difficulty = 3;
                window.WorkoutsPerWeek = 3;
                window.ProgramType = "Силовая";
            }

            if (((Window)window).ShowDialog() == true)
            {
                return new TrainingProgram
                {
                    Name = window.Name,
                    Description = window.Description,
                    ProgramType = window.ProgramType,
                    DurationWeeks = window.DurationWeeks,
                    Difficulty = window.Difficulty,
                    WorkoutsPerWeek = window.WorkoutsPerWeek,
                    CreatedDate = DateTime.Now
                };
            }
            return null;
        }
    }
}