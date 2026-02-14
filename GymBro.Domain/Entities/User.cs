namespace GymBro.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; } // Хеш пароля
        public string Email { get; set; }        // Добавим для регистрации
        public string FullName { get; set; }     // Полное имя (можно использовать вместо UserProfile, но у нас уже есть UserProfile)

        // Связь с профилем (один к одному, опционально)
        public int? UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}