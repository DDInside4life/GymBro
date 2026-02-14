using BCrypt.Net;
using GymBro.Domain.Entities;
using GymBro.Domain.Interfaces;
using System.Linq;

namespace GymBro.Business.Managers
{
    public class UserManager : BaseManager
    {
        private readonly IRepository<User> _userRepository;

        public UserManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.UsersRepository;
        }

        public User ValidateUser(string login, string password)
        {
            var user = _userRepository.Find(u => u.Login == login).FirstOrDefault();
            if (user == null)
                return null;

            // Используем полное имя для гарантии
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return valid ? user : null;
        }

        public User Register(string login, string password, string email, string fullName)
        {
            if (_userRepository.Find(u => u.Login == login).Any())
                throw new InvalidOperationException("Логин уже занят");

            var user = new User
            {
                Login = login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email,
                FullName = fullName
            };

            _userRepository.Create(user);
            _unitOfWork.SaveChanges();
            return user;
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
            return true;
        }
    }
}