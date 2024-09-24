using JavaHateBE.model;

namespace JavaHateBE.repository
{
    public class UserRepository
    {
        public List<User> Users { get; private set; } =
        [
            new("David", "Password1", "david@example.com"), 
            new("Robbe","Password2","robbe@example.com"),
            new("Valdemar","Password3","valdemar@example.com"),
            new("Matas","Password4","matas@example.com")
        ];

        public UserRepository() { }

        public async Task<User?> GetUserById(Guid id)
        {
            User? response = await Task.FromResult(Users.FirstOrDefault(u => u.Id == id));
            return await Task.FromResult(response);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            User? response = await Task.FromResult(Users.FirstOrDefault(u => u.Username == username));
            return await Task.FromResult(response);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            User? response = await Task.FromResult(Users.FirstOrDefault(u => u.Email == email));
            return await Task.FromResult(response);
        }

        public async Task<User> CreateUser(User user)
        {
            Users.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<User?> UpdateUser(User user)
        {
            User? currentUser = await GetUserById(user.Id);
            if (currentUser == null)
            {
                return await Task.FromResult(currentUser);
            }
            currentUser.UpdateUsername(user.Username);
            currentUser.UpdatePassword(user.Password);
            currentUser.UpdateEmail(user.Email);
            return await Task.FromResult(user);
        }

        public async Task<User?> DeleteUser(Guid id)
        {
            User? user = await GetUserById(id);
            if (user == null)
            {
                return await Task.FromResult(user);
            }
            Users.Remove(user);
            return await Task.FromResult(user);
        }
    }
}
