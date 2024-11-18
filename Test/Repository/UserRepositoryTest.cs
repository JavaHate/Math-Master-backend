using JavaHateBE.Data;
using JavaHateBE.Model;
using JavaHateBE.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JavaHateBE.Test.Repository
{
    public class UserRepositoryTest
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
        private readonly string ValidEmail = "TestEmail@email.com";

        [Fact]
        public async Task AddUser_WithValidUser_AddsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            // Act
            await UserRepository.CreateUser(ValidUser);
            var User = await FakeDbContext.Users.FirstOrDefaultAsync(u => u.Id == ValidUser.Id);
            // Assert
            Assert.Equal(ValidUser, User);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            // Act
            var User = await UserRepository.GetUserById(ValidUser.Id);
            // Assert
            Assert.NotNull(User);
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ReturnsNull() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            // Act
            var User = await UserRepository.GetUserById(Guid.NewGuid());
            // Assert
            Assert.Null(User);
        }

        [Fact]
        public async Task GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            // Act
            var User = await UserRepository.GetUserByUsername(ValidUsername);
            // Assert
            Assert.NotNull(User);
        }

        [Fact]
        public async Task GetUserByUsername_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            // Act
            var User = await UserRepository.GetUserByUsername("InvalidUsername");
            // Assert
            Assert.Null(User);
        }

        [Fact]
        public async Task GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            // Act
            var User = await UserRepository.GetUserByEmail(ValidEmail);
            // Assert
            Assert.NotNull(User);
        }

        [Fact]
        public async Task GetUserByEmail_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            // Act
            var User = await UserRepository.GetUserByEmail("InvalidEmail");
            // Assert
            Assert.Null(User);
        }

        [Fact]
        public async Task UpdateUser_WithValidUser_UpdatesUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            ValidUser.UpdateUsername("UpdatedUsername");
            ValidUser.UpdatePassword("UpdatedPassword");
            ValidUser.UpdateEmail("UpdatedEmail@email.com");
            // Act
            await UserRepository.UpdateUser(ValidUser);
            var User = await UserRepository.GetUserById(ValidUser.Id);
            // Assert
            Assert.Equal(ValidUser, User);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidUser_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            // Act
            var User = await UserRepository.UpdateUser(ValidUser);
            // Assert
            Assert.Null(User);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_DeletesUser()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            // Act
            await UserRepository.DeleteUser(ValidUser.Id);
            var User = await UserRepository.GetUserById(ValidUser.Id);
            // Assert
            Assert.Null(User);
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var UserRepository = new UserRepository(FakeDbContext);
            // Act
            var User = await UserRepository.DeleteUser(Guid.NewGuid());
            // Assert
            Assert.Null(User);
        }
    }
}