using System;
using System.Collections.Generic;
using System.Text;

namespace GymBro.Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public ExerciseType ExerciseType { get; set; }
        //public List<BodyPart> PrimaryMuscles { get; set; }
        //public List<BodyPart> SecondaryMuscles { get; set; }
        public string EquipmentNeeded { get; set; }
        public string TechniqueTips { get; set; }
        public string CommonMistakes { get; set; }
        public string ImageUrl { get; set; } // Изображение техники
        public string VideoUrl { get; set; } // Видео демонстрация

        // Стандартные параметры
        public int DefaultSets { get; set; }
        public int DefaultRepsMin { get; set; }
        public int DefaultRepsMax { get; set; }
        public TimeSpan RestBetweenSets { get; set; }

        // Внешние ключи
        public int TrainingProgramId { get; set; }
        public int? ProgramDayId { get; set; }

        // Навигационные свойства
        public TrainingProgram TrainingProgram { get; set; }
        public ICollection<Equipment> Equipment { get; set; }
        //public ProgramDay ProgramDay { get; set; }
        //public ICollection<WorkoutSession> WorkoutSessions { get; set; }
    }





}
