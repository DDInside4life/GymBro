using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace GymBro.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Entities.UserProfile> UserProfilesRepository { get; }
        IRepository<Entities.TrainingProgram> TrainingProgramsRepository { get; }
        void SaveChanges();
    }
}
