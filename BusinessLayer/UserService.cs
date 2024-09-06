using BusinessLayer.DTOs;
using BusinessLayer.Securities;
using DataLayer;
using Entities;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private User? _loggedInUser;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task< ServiceResult<bool>> RegisterUserAsync(string username, string password)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z]+$"))
            {
                return ServiceResult<bool>.FailureResult("Username contains invalid characters. Only English letters are allowed.");
            }

            if (await _userRepository.GetUserByUsernameAsync(username) != null)
            {
                return ServiceResult<bool>.FailureResult("Username already exists.");
            }

            var user = new User { Username = username.ToLower(), PasswordHash = HashConverter.SHA256(password) };
            await _userRepository.AddAsync(user);
            return ServiceResult<bool>.SuccessResult(true, "Successfully registered.");
        }

        public async Task<ServiceResult<bool>> LoginAsync(string username, string password)
        {
            var user =await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.PasswordHash != HashConverter.SHA256(password))
            {
                return ServiceResult<bool>.FailureResult("Invalid username or password.");

            }
            _loggedInUser = user;
            return ServiceResult<bool>.SuccessResult(true, "You Loged in successfully.");

        }
        public async Task<ServiceResult<bool>> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<bool>.FailureResult("No user is currently logged in.");
            }

            var user = await _userRepository.GetUserByIdAsync(_loggedInUser.ID);
            if (user == null)
            {
                return ServiceResult<bool>.FailureResult("User Not Found.");

            }
            if (user.PasswordHash != HashConverter.SHA256(oldPassword))
            {
                return ServiceResult<bool>.FailureResult("Old password is incorrect.");
            }

            user.PasswordHash = HashConverter.SHA256(newPassword);
            var res = await _userRepository.UpdateAsync(user);
            if (res)
            {
                return ServiceResult<bool>.SuccessResult(true, "Password changed successfully.");

            }
            return ServiceResult<bool>.FailureResult("password Didnt Change.");

        }

        public ServiceResult<bool> Logout()
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<bool>.FailureResult("No user is currently logged in.");
            }

            _loggedInUser = null;

            return ServiceResult<bool>.SuccessResult(true, "Logged out successfully.");
        }

        public async Task<ServiceResult<bool>> ChangeStatusAsync(bool isAvailable)
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<bool>.FailureResult("No user is currently logged in.");
            }

            var user = await _userRepository.GetUserByIdAsync(_loggedInUser.ID);
            if (user == null)
            {
                return ServiceResult<bool>.FailureResult("User Not Found.");

            }
            user.IsAvailable = isAvailable;
            var res = await _userRepository.UpdateAsync(user);
            if (res)
            {
                return ServiceResult<bool>.SuccessResult(true, "Status changed successfully.");

            }
            return ServiceResult<bool>.FailureResult("Status Didnt Change");

        }

        public async Task<ServiceResult<List<User>>> GetAllUsersByUserNameAsync(string userName)
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<List<User>>.FailureResult("No user is currently logged in.");
            }

            var users =await _userRepository.GetAllUsersByUsernameAsync(userName);

            if (users.Count == 0)
            {
                return ServiceResult<List<User>>.FailureResult("No users found.");
            }

            return ServiceResult<List<User>>.SuccessResult(users);
        }


    }

}