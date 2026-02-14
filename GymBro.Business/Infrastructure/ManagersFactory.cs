using GymBro.Business.Managers;
using GymBro.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GymBro.Business.Infrastructure
{
    /// <summary>
    /// Фабрика для создания менеджеров
    /// Паттерн Factory - используется для внедрения зависимостей
    /// </summary>
    public class ManagersFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private UserProfileManager _userProfileManager;
        private TrainingProgramManager _trainingProgramManager;

        public ManagersFactory(string connectionString)
        {
            // Создаем UnitOfWork с переданной строкой подключения
            _unitOfWork = new GymBro.DAL.Repositories.EFUnitOfWork(connectionString);
        }




        /// <summary>
        /// Создать фабрику с конфигурацией из файла appsettings.json
        /// </summary>
        public ManagersFactory() : this(GetConnectionStringFromConfig())
        {
        }

        private static string GetConnectionStringFromConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Получить менеджер профилей пользователей
        /// </summary>
        public UserProfileManager GetUserProfileManager()
        {
            return _userProfileManager ??= new UserProfileManager(_unitOfWork);
        }

        /// <summary>
        /// Получить менеджер тренировочных программ
        /// </summary>
        public TrainingProgramManager GetTrainingProgramManager()
        {
            return _trainingProgramManager ??= new TrainingProgramManager(_unitOfWork);
        }
    }
}