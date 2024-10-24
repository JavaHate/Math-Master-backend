using JavaHateBE.Util;
using System.ComponentModel.DataAnnotations;

namespace JavaHateBE.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public DateTime LastLogin { get; private set; }
        public List<Game> Games { get; private set; } = new List<Game>();


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
