using JavaHateBE.Data;
using JavaHateBE.Exceptions;
using JavaHateBE.Model;
using JavaHateBE.Model.DTOs;
using JavaHateBE.Repository;
using JavaHateBE.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JavaHateBE.Test.Service
{
    public class UserServiceTest
    {
        private MathMasterDBContext CreateFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<MathMasterDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MathMasterDBContext(options);
        }

        private readonly string ValidUsername = "TestUser";
        private readonly string ValidPassword = "TestPassword";
        private readonly string ValidEmail = "Test@email.com";

        [Fact]
        public async Task AddUser_WithValidUser_AddsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            // Act
            var primeuser = await UserService.CreateUser(ValidUser);
            var User = await FakeDbContext.Users.FirstOrDefaultAsync(u => u.Id == primeuser.Id);
            // Assert
            Assert.Equal(primeuser, User);
        }

        [Fact]
        public async Task AddUser_WithDuplicateUsername_ThrowsIllegalArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var ValidUser2 = new UserCreateInput(ValidUsername, "ValidPassword", "Valid@Email.com");
            // Act
            await UserService.CreateUser(ValidUser);
            // Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => UserService.CreateUser(ValidUser2));
            Assert.Equal("Username already exists", ex.Message);
            Assert.Equal("username", ex.Argument);
        }

        [Fact]
        public async Task AddUser_WithDuplicateEmail_ThrowsIllegalArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var ValidUser2 = new UserCreateInput("ValidUsername", "ValidPassword", ValidEmail);
            // Act
            await UserService.CreateUser(ValidUser);
            // Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => UserService.CreateUser(ValidUser2));
            Assert.Equal("Email already exists", ex.Message);
            Assert.Equal("email", ex.Argument);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            // Act
            var User = await UserService.GetUserById(primeuser.Id);
            // Assert
            Assert.NotNull(User);
            Assert.Equal(primeuser.Id, User.Id);
            Assert.Equal(primeuser.Username, User.Username);
            Assert.Equal(primeuser.Email, User.Email);
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.GetUserById(Guid.NewGuid()));
            Assert.Equal("No users found with that ID", ex.Message);
            Assert.Equal("user", ex.Object);
        }

        [Fact]
        public async Task GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            // Act
            var User = await UserService.GetUserByUsername(primeuser.Username);
            // Assert
            Assert.NotNull(User);
            Assert.Equal(primeuser.Id, User.Id);
            Assert.Equal(primeuser.Username, User.Username);
            Assert.Equal(primeuser.Email, User.Email);
        }

        [Fact]
        public async Task GetUserByUsername_WithInvalidUsername_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.GetUserByUsername("InvalidUsername"));
            Assert.Equal("No users found with that username", ex.Message);
            Assert.Equal("user", ex.Object);
        }

        [Fact]
        public async Task GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            // Act
            var User = await UserService.GetUserByEmail(primeuser.Email);
            // Assert
            Assert.NotNull(User);
            Assert.Equal(primeuser.Id, User.Id);
            Assert.Equal(primeuser.Username, User.Username);
            Assert.Equal(primeuser.Email, User.Email);
        }

        [Fact]
        public async Task GetUserByEmail_WithInvalidEmail_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.GetUserByEmail("InvalidEmail"));
            Assert.Equal("No users found with that email", ex.Message);
            Assert.Equal("user", ex.Object);
        }

        [Fact]
        public async Task UpdateUser_WithValidUser_UpdatesUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            primeuser.UpdateUsername("UpdatedUsername");
            primeuser.UpdatePassword("UpdatedPassword");
            primeuser.UpdateEmail("updated@email.com");
            // Act
            await UserService.UpdateUser(primeuser);
            var User = await UserService.GetUserById(primeuser.Id);
            // Assert
            Assert.Equal(primeuser, User);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidUser_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            primeuser.Id = Guid.NewGuid();
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.UpdateUser(primeuser));
            Assert.Equal("No users found with that ID", ex.Message);
            Assert.Equal("user", ex.Object);
        }

        [Fact]
        public async Task UpdateUser_WithDuplicateUsername_ThrowsIllegalArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            var InvalidableUser = new UserCreateInput("InvalidUsername", "InvalidPassword", "Invalid@Email.com");
            var invaliduser = await UserService.CreateUser(InvalidableUser);
            invaliduser.UpdateUsername(ValidUsername);
            // Act & Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => UserService.UpdateUser(invaliduser));
            Assert.Equal("Username already exists", ex.Message);
            Assert.Equal("username", ex.Argument);
        }

        [Fact]
        public async Task UpdateUser_WithDuplicateEmail_ThrowsIllegalArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            var InvalidableUser = new UserCreateInput("InvalidUsername", "InvalidPassword", "Invalid@email.com");
            var invaliduser = await UserService.CreateUser(InvalidableUser);
            invaliduser.UpdateEmail(ValidEmail);
            // Act & Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => UserService.UpdateUser(invaliduser));
            Assert.Equal("Email already exists", ex.Message);
            Assert.Equal("email", ex.Argument);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_DeletesUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            // Act
            var User = await UserService.DeleteUser(primeuser.Id);
            // Assert
            Assert.Equal(primeuser, User);
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.GetUserById(primeuser.Id));
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.DeleteUser(Guid.NewGuid()));
            Assert.Equal("No users found with that ID", ex.Message);
            Assert.Equal("user", ex.Object);
        }

        [Fact]
        public async Task Login_WithValidUserNameAndPassword_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            // Act
            var User = await UserService.Login(ValidPassword, ValidUsername);
            // Assert
            Assert.NotNull(User);
            Assert.Equal(primeuser.Id, User.Id);
            Assert.Equal(primeuser.Username, User.Username);
            Assert.Equal(primeuser.Email, User.Email);
        }

        [Fact]
        public async Task Login_WithValidEmailAndPassword_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var primeuser = await UserService.CreateUser(ValidUser);
            // Act
            var User = await UserService.Login(ValidPassword, ValidEmail);
            // Assert
            Assert.NotNull(User);
            Assert.Equal(primeuser.Id, User.Id);
            Assert.Equal(primeuser.Username, User.Username);
            Assert.Equal(primeuser.Email, User.Email);
        }

        

        [Fact]
        public async Task Login_WithInvalidUsernameOrEmail_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => UserService.Login(ValidPassword, "invalidusfeprname"));
            Assert.Equal("No users found with that username or email", ex.Message);
            Assert.Equal("user", ex.Object);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ThrowsIllegalArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var UserService = new UserService(UserRepository);
            var ValidUser = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            await UserService.CreateUser(ValidUser);
            // Act & Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => UserService.Login( "InvalidPassword", ValidUsername));
            Assert.Equal("Invalid password", ex.Message);
            Assert.Equal("password", ex.Argument);
        }
    }
}