﻿using JavaHateBE.Exceptions;
using JavaHateBE.Model;
using JavaHateBE.Repository;
using JavaHateBE.Model.DTOs;
using JavaHateBE.Util;

namespace JavaHateBE.Service
{
    /// <summary>
    /// Provides services for managing users.
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified ID.</exception>
        public async Task<User> GetUserById(Guid id)
        {
            User? user = await _userRepository.GetUserById(id) ?? throw new ObjectNotFoundException("user", "No users found with that ID");
            return await Task.FromResult(user);
        }

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user with the specified username.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified username.</exception>
        public async Task<User> GetUserByUsername(string username)
        {
            User? user = await _userRepository.GetUserByUsername(username) ?? throw new ObjectNotFoundException("user", "No users found with that username");
            return await Task.FromResult(user);
        }

        /// <summary>
        /// Gets a user by their email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The user with the specified email.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified email.</exception>
        public async Task<User> GetUserByEmail(string email)
        {
            User? user = await _userRepository.GetUserByEmail(email) ?? throw new ObjectNotFoundException("user", "No users found with that email");
            return await Task.FromResult(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The created user.</returns>
        /// <exception cref="IllegalArgumentException">Thrown when a user with the same username or email already exists.</exception>
        public async Task<User> CreateUser(UserCreateInput user)
        {
            if (await _userRepository.GetUserByUsername(user.Username) != null)
            {
                throw new IllegalArgumentException("username", "Username already exists");
            }
            if (await _userRepository.GetUserByEmail(user.Email) != null)
            {
                throw new IllegalArgumentException("email", "Email already exists");
            }
            User newUser = await _userRepository.CreateUser(new User(user.Username, user.Password, user.Email));
            return await Task.FromResult(newUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when the user is not found.</exception>
        /// <exception cref="IllegalArgumentException">Thrown when a user with the same username or email already exists.</exception>
        public async Task<User> UpdateUser(User user)
        {
            var existingUser = await _userRepository.GetUserById(user.Id);
            if (existingUser == null)
            {
                throw new ObjectNotFoundException("user", "No users found with that ID");
            }

            var existingUserByUsername = await _userRepository.GetUserByUsername(user.Username);
            if (existingUserByUsername != null && existingUserByUsername.Id != user.Id)
            {
                throw new IllegalArgumentException("username", "Username already exists");
            }

            var existingUserByEmail = await _userRepository.GetUserByEmail(user.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
            {
                throw new IllegalArgumentException("email", "Email already exists");
            }

            existingUser.UpdateUsername(user.Username);
            existingUser.UpdateEmail(user.Email);
            if(user.Password != null && user.Password != "" && user.Password != existingUser.Password) 
            {
                existingUser.UpdatePassword(user.Password);
            }

            User? updatedUser = await _userRepository.UpdateUser(existingUser)
                ?? throw new ObjectNotFoundException("user", "No users found with that ID");

            return await Task.FromResult(updatedUser);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>The deleted user.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified ID.</exception>
        public async Task<User> DeleteUser(Guid id)
        {
            User deletedUser = await _userRepository.DeleteUser(id) ?? throw new ObjectNotFoundException("user", "No users found with that ID");
            return await Task.FromResult(deletedUser);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="username">The username (or email adress) of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The logged in user.</returns>
        /// <exception cref="ObjectNotFoundException">Thrown when no user is found with the specified username.</exception>
        public async Task<User> Login(string password, string username)
        {
            User user = await _userRepository.GetUserByUsername(username) 
                ?? await _userRepository.GetUserByEmail(username) 
                ?? throw new ObjectNotFoundException("user", "No users found with that username or email");
            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                throw new IllegalArgumentException("password", "Invalid password");
            }
            user.UpdateLastLogin();
            await _userRepository.UpdateUser(user);
            return await Task.FromResult(user);
        }
    }
}
