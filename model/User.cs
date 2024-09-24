using JavaHateBE.util;
using System.ComponentModel.DataAnnotations;

namespace JavaHateBE.model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public DateTime LastLogin { get; private set; }


        public User(string username, string password, string email)
        {
            Username = username;
            Password = PasswordHasher.HashPassword(password);
            Email = email;
            LastLogin = DateTime.Now;
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.Now;
        }

        public void UpdatePassword(string password)
        {
            Password = PasswordHasher.HashPassword(password);
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void UpdateUsername(string username)
        {
            Username = username;
        }

        public bool IsPasswordCorrect(string password)
        {
            return PasswordHasher.VerifyPassword(password, Password);
        }
    }
}
