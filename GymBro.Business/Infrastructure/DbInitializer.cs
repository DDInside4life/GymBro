using GymBro.Domain.Entities;
using GymBro.DAL.Data;
using System;
using System.Linq;

namespace GymBro.Business.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(GymBroContext context)
        {
            // Убедимся, что база создана (если ещё не)
            context.Database.EnsureCreated();

            // Если уже есть пользователи – ничего не делаем
            if (context.UserProfiles.Any())
                return;

            // Добавляем оборудование
            var equipmentList = new[]
            {
                new Equipment { Name = "Штанга", Description = "Олимпийский гриф 20 кг" },
                new Equipment { Name = "Гантели", Description = "Набор разборных гантелей" },
                new Equipment { Name = "Скамья", Description = "Регулируемая скамья" },
                new Equipment { Name = "Беговая дорожка", Description = "Кардиотренажёр" },
                new Equipment { Name = "Турник", Description = "Для подтягиваний" },
                new Equipment { Name = "Брусья", Description = "Для отжиманий" }
    };
            context.Equipment.AddRange(equipmentList);
            context.SaveChanges();

            // Добавляем упражнения с привязкой к оборудованию
            var exercises = new[]
            {
        new Exercise
        {
            Name = "Жим штанги лёжа",
            Description = "Базовое упражнение для грудных мышц",
            DefaultSets = 4,
            DefaultRepsMin = 8,
            DefaultRepsMax = 12,
            RestBetweenSets = TimeSpan.FromMinutes(2),
            TechniqueTips = "Не отрывайте таз от скамьи, держите лопатки сведёнными",
            CommonMistakes = "Слишком широкий хват, отбив штанги от груди",
            ImageUrl = "benchpress.jpg",
            VideoUrl = "benchpress.mp4",
            Equipment = new List<Equipment> { equipmentList[0], equipmentList[2] } // штанга, скамья
        },
        new Exercise
        {
            Name = "Приседания со штангой",
            Description = "Базовое упражнение для мышц ног",
            DefaultSets = 4,
            DefaultRepsMin = 8,
            DefaultRepsMax = 12,
            RestBetweenSets = TimeSpan.FromMinutes(2.5),
            TechniqueTips = "Колени не должны выходить за носки, спина прямая",
            CommonMistakes = "Округление спины, недостаточная глубина",
            ImageUrl = "squat.jpg",
            VideoUrl = "squat.mp4",
            Equipment = new List<Equipment> { equipmentList[0] } // штанга
        },
        new Exercise
        {
            Name = "Тяга верхнего блока к груди",
            Description = "Упражнение для широчайших мышц спины",
            DefaultSets = 3,
            DefaultRepsMin = 10,
            DefaultRepsMax = 15,
            RestBetweenSets = TimeSpan.FromMinutes(1.5),
            TechniqueTips = "Локти направлены вниз и назад, сводите лопатки",
            CommonMistakes = "Рывки корпусом, слишком широкий хват",
            ImageUrl = "latpulldown.jpg",
            VideoUrl = "latpulldown.mp4",
            Equipment = new List<Equipment> { equipmentList[4] } // турник (условно, обычно тренажёр)
        },
        new Exercise
        {
            Name = "Бег на беговой дорожке",
            Description = "Кардио упражнение",
            DefaultSets = 1,
            DefaultRepsMin = 20,
            DefaultRepsMax = 30,
            RestBetweenSets = TimeSpan.Zero,
            TechniqueTips = "Сохраняйте ровный темп, следите за пульсом",
            CommonMistakes = "Держание за поручни, слишком большой наклон",
            ImageUrl = "treadmill.jpg",
            VideoUrl = "treadmill.mp4",
            Equipment = new List<Equipment> { equipmentList[3] } // беговая дорожка
        },
        new Exercise
        {
            Name = "Подтягивания",
            Description = "Упражнение для спины и бицепса",
            DefaultSets = 3,
            DefaultRepsMin = 5,
            DefaultRepsMax = 12,
            RestBetweenSets = TimeSpan.FromMinutes(2),
            TechniqueTips = "Тяните локти вниз, не раскачивайтесь",
            CommonMistakes = "Рывки, неполная амплитуда",
            ImageUrl = "pullups.jpg",
            VideoUrl = "pullups.mp4",
            Equipment = new List<Equipment> { equipmentList[4] } // турник
        }
    };
            context.Exercises.AddRange(exercises);
            context.SaveChanges();



            // Добавляем пользователей
            var users = new[]
            {
                new UserProfile
                {
                    Name = "Кулеш Роман",
                    Age = 25,
                    Weight = 80,
                    Height = 180,
                    BirthDate = new DateTime(1998, 5, 15),
                    FitnessLevel = "Продвинутый",
                    TrainingGoal = "Набор массы"
                },
                new UserProfile
                {
                    Name = "Анна Смирнова",
                    Age = 28,
                    Weight = 60,
                    Height = 165,
                    BirthDate = new DateTime(1995, 8, 22),
                    FitnessLevel = "Средний",
                    TrainingGoal = "Похудение"
                },
                new UserProfile
                {
                    Name = "Дмитрий Петров",
                    Age = 32,
                    Weight = 75,
                    Height = 175,
                    BirthDate = new DateTime(1991, 11, 3),
                    FitnessLevel = "Начинающий",
                    TrainingGoal = "Поддержание"
                }
            };
            context.UserProfiles.AddRange(users);
            context.SaveChanges();

            // Получаем Id добавленных пользователей
            var userIds = users.Select(u => u.Id).ToArray();

            // Добавляем тренировочные программы
            var programs = new[]
            {
                new TrainingProgram
                {
                    Name = "Силовая программа для набора массы",
                    Description = "Интенсивные тренировки с акцентом на базовые упражнения",
                    ProgramType = "Силовая",
                    DurationWeeks = 12,
                    Difficulty = 4,
                    WorkoutsPerWeek = 4,
                    CreatedDate = DateTime.Now,
                    UserProfileId = userIds[0] // Кулеш Роман
                },
                new TrainingProgram
                {
                    Name = "Кардио для похудения",
                    Description = "Высокоинтенсивные интервальные тренировки",
                    ProgramType = "Кардио",
                    DurationWeeks = 8,
                    Difficulty = 3,
                    WorkoutsPerWeek = 5,
                    CreatedDate = DateTime.Now,
                    UserProfileId = userIds[1] // Анна Смирнова
                },
                new TrainingProgram
                {
                    Name = "Фулбоди для начинающих",
                    Description = "Круговая тренировка на всё тело",
                    ProgramType = "Фулбоди",
                    DurationWeeks = 6,
                    Difficulty = 2,
                    WorkoutsPerWeek = 3,
                    CreatedDate = DateTime.Now,
                    UserProfileId = userIds[2] // Дмитрий Петров
                }
            };
            context.TrainingPrograms.AddRange(programs);
            context.SaveChanges();

            // Добавляем упражнения
             exercises = new[]
            {
                new Exercise
                {
                    Name = "Жим штанги лёжа",
                    Description = "Базовое упражнение для грудных мышц",
                    DefaultSets = 4,
                    DefaultRepsMin = 8,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(2),
                    TechniqueTips = "Не отрывайте таз от скамьи, держите лопатки сведёнными",
                    CommonMistakes = "Слишком широкий хват, отбив штанги от груди",
                    ImageUrl = "benchpress.jpg",
                    VideoUrl = "benchpress.mp4",
                    EquipmentNeeded = "штанга, скамья, блины"
                },
                new Exercise
                {
                    Name = "Приседания со штангой",
                    Description = "Базовое упражнение для мышц ног",
                    DefaultSets = 4,
                    DefaultRepsMin = 8,
                    DefaultRepsMax = 12,
                    RestBetweenSets = TimeSpan.FromMinutes(2.5),
                    TechniqueTips = "Колени не должны выходить за носки, спина прямая",
                    CommonMistakes = "Округление спины, недостаточная глубина",
                    ImageUrl = "squat.jpg",
                    VideoUrl = "squat.mp4",
                    EquipmentNeeded = "штанга, стойка"
                },
                new Exercise
                {
                    Name = "Тяга верхнего блока к груди",
                    Description = "Упражнение для широчайших мышц спины",
                    DefaultSets = 3,
                    DefaultRepsMin = 10,
                    DefaultRepsMax = 15,
                    RestBetweenSets = TimeSpan.FromMinutes(1.5),
                    TechniqueTips = "Локти направлены вниз и назад, сводите лопатки",
                    CommonMistakes = "Рывки корпусом, слишком широкий хват",
                    ImageUrl = "latpulldown.jpg",
                    VideoUrl = "latpulldown.mp4",
                    EquipmentNeeded = "тренажёр, V-рукоять"
                },
                new Exercise
                {
                    Name = "Бег на беговой дорожке",
                    Description = "Кардио упражнение",
                    DefaultSets = 1,
                    DefaultRepsMin = 20,
                    DefaultRepsMax = 30,
                    RestBetweenSets = TimeSpan.Zero,
                    TechniqueTips = "Сохраняйте ровный темп, следите за пульсом",
                    CommonMistakes = "Держание за поручни, слишком большой наклон",
                    ImageUrl = "treadmill.jpg",
                    VideoUrl = "treadmill.mp4",
                    EquipmentNeeded = "беговая дорожка"
                }
            };
            context.Exercises.AddRange(exercises);
            context.SaveChanges();
        }
    }
}