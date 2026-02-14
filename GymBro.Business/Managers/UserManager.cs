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
            _userRepository = unitOfWork.UsersRepository; // нужно добавить в IUnitOfWork
        }

        // Проверка логина и пароля, возвращает пользователя при успехе
        public User ValidateUser(string login, string password)
        {
            var user = _userRepository.Find(u => u.Login == login).FirstOrDefault();
            if (user == null)
                return null;

            bool valid = BCrypt.Verify(password, user.PasswordHash);
            return valid ? user : null;
        }

        // Регистрация нового пользователя
        public User Register(string login, string password, string email, string fullName)
        {
            // Проверка, не занят ли логин
            if (_userRepository.Find(u => u.Login == login).Any())
                throw new InvalidOperationException("Логин уже занят");

            var user = new User
            {
                Login = login,
                PasswordHash = BCrypt.HashPassword(password),
                Email = email,
                FullName = fullName
            };

            _userRepository.Create(user);
            _unitOfWork.SaveChanges();
            return user;
        }

        // Смена пароля
        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
                return false;

            if (!BCrypt.Verify(oldPassword, user.PasswordHash))
                return false;

        

            user.PasswordHash = BCrypt.Net.HashPassword(newPassword);
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
            return true;
        }
    }
}