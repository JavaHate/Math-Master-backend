using JavaHateBE.Data;
using JavaHateBE.Model;
using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.Repository
{
    public class UserRepository
    {
        private readonly MathMasterDBContext _context;

        public UserRepository(MathMasterDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(User user)
        {
            var currentUser = await GetUserById(user.Id);
            if (currentUser == null)
            {
                return null;
            }
            currentUser.UpdateUsername(user.Username);
            if (currentUser.Password != user.Password)
            {
                currentUser.UpdatePassword(user.Password);
            }
            currentUser.UpdateEmail(user.Email);
            
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
            return currentUser;
        }

        public async Task<User?> DeleteUser(Guid id)
        {
            var user = await GetUserById(id);
            if (user == null)
            {
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
