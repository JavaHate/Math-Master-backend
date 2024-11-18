using Xunit;
using JavaHateBE.Model;
using JavaHateBE.Util;
using JavaHateBE.Exceptions;
using JavaHateBE.Model.DTOs;

namespace JavaHateBE.Test.Domain
{
    public class UserTest
    {
        private readonly string ValidUsername = "username";
        private readonly string ValidPassword = "password";
        private readonly string ValidEmail = "email@email.com";

        [Fact]
        public void CreatingValidUser_CreatesUser()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.NotNull(User);
            Assert.NotEqual(Guid.Empty, User.Id);
            Assert.Equal(ValidUsername, User.Username);
            Assert.True(PasswordHasher.VerifyPassword(ValidPassword, User.Password));
            Assert.Equal(ValidEmail, User.Email);
            Assert.NotEqual(DateTime.MinValue, User.LastLogin);
        }

        [Fact]
        public void UpdatingLastLogin_UpdatesLastLogin()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var lastLogin = User.LastLogin;

            User.UpdateLastLogin();

            Assert.NotEqual(lastLogin, User.LastLogin);
        }

        [Fact]
        public void UpdatingPassword_UpdatesPassword()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var newPassword = "newPassword";

            User.UpdatePassword(newPassword);

            Assert.True(PasswordHasher.VerifyPassword(newPassword, User.Password));
        }

        [Fact]
        public void UpdatingEmail_UpdatesEmail()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var newEmail = "newEmail@email.com";

            User.UpdateEmail(newEmail);

            Assert.Equal(newEmail, User.Email);
        }

        [Fact]
        public void UpdatingUsername_UpdatesUsername()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var newUsername = "newUsername";

            User.UpdateUsername(newUsername);

            Assert.Equal(newUsername, User.Username);
        }

        [Fact]
        public void IsPasswordCorrect_ReturnsTrue()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.True(User.IsPasswordCorrect(ValidPassword));
        }

        [Fact]
        public void IsPasswordCorrect_ReturnsFalse()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.False(User.IsPasswordCorrect("wrongPassword"));
        }

        [Fact]
        public void CreatingUserWithInvalidUsername_ThrowsIllegalArgumentException()
        {
            Assert.Throws<IllegalArgumentException>(() => new User("u", ValidPassword, ValidEmail));
        }

        [Fact]
        public void CreatingUserWithInvalidPassword_ThrowsIllegalArgumentException()
        {
            Assert.Throws<IllegalArgumentException>(() => new User(ValidUsername, "passw", ValidEmail));
        }

        [Fact]
        public void CreatingUserWithInvalidEmail_ThrowsIllegalArgumentException()
        {
            Assert.Throws<IllegalArgumentException>(() => new User(ValidUsername, ValidPassword, "email"));
        }

        [Fact]
        public void UpdatingPasswordWithInvalidPassword_ThrowsIllegalArgumentException()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.Throws<IllegalArgumentException>(() => User.UpdatePassword("passw"));
        }

        [Fact]
        public void UpdatingEmailWithInvalidEmail_ThrowsIllegalArgumentException()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.Throws<IllegalArgumentException>(() => User.UpdateEmail("email"));
        }

        [Fact]
        public void UpdatingUsernameWithInvalidUsername_ThrowsIllegalArgumentException()
        {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.Throws<IllegalArgumentException>(() => User.UpdateUsername("u"));
        }

        [Fact]
        public void IsPasswordCorrectWithInvalidPassword_ReturnsFalse()
        {
            var user = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.False(user.IsPasswordCorrect("passw"));
        }

        [Fact]
        public void IsPasswordCorrectWithNullPassword_ReturnsFalse()
        {
            var user = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.False(user.IsPasswordCorrect(null));
        }

        [Fact]
        public void IsPasswordCorrectWithEmptyPassword_ReturnsFalse()
        {
            var user = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.False(user.IsPasswordCorrect(""));
        }

        [Fact]
        public void IsPasswordCorrectWithWhitespacePassword_ReturnsFalse()
        {
            var user = new User(ValidUsername, ValidPassword, ValidEmail);

            Assert.False(user.IsPasswordCorrect(" "));
        }

        [Fact]
        public void AddGame_AddsGameToUser() {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var Game = new Game(GameMode.ENDLESS, User.Id);

            User.AddGame(Game);

            Assert.Single(User.Games);
            Assert.Contains(Game, User.Games);
        }

        [Fact]
        public void NewGame_CreatesGame() {
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var Game = User.NewGame(GameMode.ENDLESS);

            Assert.NotNull(Game);
            Assert.NotEqual(Guid.Empty, Game.Id);
            Assert.Equal(GameMode.ENDLESS, Game.GameMode);
            Assert.Equal(User.Id, Game.UserId);
            Assert.Empty(Game.Questions);
            Assert.NotEqual(DateTime.MinValue, Game.StartTime);
            Assert.Equal(DateTime.MinValue, Game.EndTime);
            Assert.Single(User.Games);
            Assert.Contains(Game, User.Games);
        }

        [Fact]
        public void UserFromReturnsValidUser() {
            var validID = Guid.NewGuid();
            var UpdateUser = new UpdateUserInput(validID, ValidUsername, PasswordHasher.HashPassword(ValidPassword), ValidEmail);
            var User = Model.User.From(UpdateUser);

            Assert.Equal(validID, User.Id);
            Assert.Equal(ValidUsername, User.Username);
            Assert.True(PasswordHasher.VerifyPassword(ValidPassword, User.Password));
            Assert.Equal(ValidEmail, User.Email);
            Assert.NotEqual(DateTime.MinValue, User.LastLogin);
        }
    }
}