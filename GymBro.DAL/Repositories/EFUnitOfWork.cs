using GymBro.DAL.Data;
using GymBro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace GymBro.DAL.Repositories
{
    /// <summary>
    /// Реализация Unit of Work для управления транзакциями
    /// Паттерн Unit of Work
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly GymBroContext _context;
        private IRepository<Domain.Entities.UserProfile> _userProfilesRepository;
        private IRepository<Domain.Entities.TrainingProgram> _trainingProgramsRepository;

        public EFUnitOfWork(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            // Создаем опции для DbContext
            var options = new DbContextOptionsBuilder<GymBroContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new GymBroContext(options);

            // Гарантируем создание базы данных
            _context.Database.EnsureCreated();
        }

        public IRepository<Domain.Entities.UserProfile> UserProfilesRepository
        {
            get
            {
                return _userProfilesRepository ??= new UserProfileRepository(_context);
            }
        }

        public IRepository<Domain.Entities.TrainingProgram> TrainingProgramsRepository
        {
            get
            {
                return _trainingProgramsRepository ??= new TrainingProgramRepository(_context);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}