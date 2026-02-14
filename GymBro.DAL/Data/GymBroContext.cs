using GymBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GymBro.DAL.Data
{
    /// <summary>
    /// Контекст Entity Framework для работы с базой данных
    /// Подход Code First
    /// </summary>
    public class GymBroContext : DbContext
    {
        public GymBroContext(DbContextOptions<GymBroContext> options)
            : base(options)
        {
        }

        
        // Таблица профилей пользователей
        public DbSet<UserProfile> UserProfiles { get; set; }

        // Таблица тренировочных программ
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи один-ко-многим
            // Один UserProfile -> много TrainingProgram
            modelBuilder.Entity<TrainingProgram>()
                .HasOne(p => p.UserProfile)
                .WithMany(u => u.TrainingPrograms)
                .HasForeignKey(p => p.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка ограничений для UserProfile
            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.FitnessLevel)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.TrainingGoal)
                .IsRequired()
                .HasMaxLength(50);

            // Настройка ограничений для TrainingProgram
            modelBuilder.Entity<TrainingProgram>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<TrainingProgram>()
                .Property(p => p.ProgramType)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}