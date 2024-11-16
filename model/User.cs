using JavaHateBE.Exceptions;
using JavaHateBE.Util;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JavaHateBE.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; protected set; } = string.Empty;
        public string Password { get; protected set; } = string.Empty;
        public string Email { get; protected set; } = string.Empty;
        public DateTime LastLogin { get; private set; }
        public List<Game> Games { get; private set; } = new List<Game>();

        public User() { }


        public User(string username, string password, string email)
        {
            if(username.Trim().Length <= 1)
            {
                throw new IllegalArgumentException("username","Username must be at least 2 characters long");
            }
            Username = username;
            if(password.Trim().Length <= 5)
            {
                throw new IllegalArgumentException("password","Password must be at least 6 characters long");
            }
            Password = PasswordHasher.HashPassword(password);
            Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if(!EmailRegex.IsMatch(email))
            {
                throw new IllegalArgumentException("email","Email is not valid");
            }
            Email = email;
            LastLogin = DateTime.Now;
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.Now;
        }

        public void UpdatePassword(string password)
        {
            if (password.Trim().Length <= 5)
            {
                throw new IllegalArgumentException("password", "Password must be at least 6 characters long");
            }
            Password = PasswordHasher.HashPassword(password);
        }

        public void UpdateEmail(string email)
        {
            Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!EmailRegex.IsMatch(email))
            {
                throw new IllegalArgumentException("email", "Email is not valid");
            }
            Email = email;
        }

        public void UpdateUsername(string username)
        {
            if (username.Trim().Length <= 1)
            {
                throw new IllegalArgumentException("username", "Username must be at least 2 characters long");
            }
            Username = username;
        }

        public bool IsPasswordCorrect(string? password)
        {
            if(password == null){
                return false;
            }
            return PasswordHasher.VerifyPassword(password: password, hashedPassword: Password);
        }

        public void AddGame(Game game)
        {
            Games.Add(game);
        }

        public Game NewGame(GameMode gameMode)
        {
            Game game = new Game(gameMode, Id);
            Games.Add(game);
            return game;
        }
    }
}
