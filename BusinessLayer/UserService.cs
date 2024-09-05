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

        public ServiceResult<bool> RegisterUser(string username, string password)
        {
            if (_userRepository.GetUserByUsername(username) != null)
            {
                return ServiceResult<bool>.FailureResult("Username already exists.");
            }

            var user = new User { Username = username, PasswordHash = HashConverter.SHA256(password) };
            _userRepository.Add(user);
            return ServiceResult<bool>.SuccessResult(true, "Successfully registered.");
        }

        public ServiceResult<bool> Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || user.PasswordHash != HashConverter.SHA256(password))
            {
                return ServiceResult<bool>.FailureResult("Invalid username or password.");

            }
            _loggedInUser = user;
            return ServiceResult<bool>.SuccessResult(true, "You Loged in successfully.");

        }
        public ServiceResult<bool> ChangePassword(string oldPassword, string newPassword)
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<bool>.FailureResult("No user is currently logged in.");
            }

            var user = _userRepository.GetUserById(_loggedInUser.ID);
            if (user == null)
            {
                return ServiceResult<bool>.FailureResult("User Not Found.");

            }
            if (user.PasswordHash != HashConverter.SHA256(oldPassword))
            {
                return ServiceResult<bool>.FailureResult("Old password is incorrect.");
            }

            user.PasswordHash = HashConverter.SHA256(newPassword);
            var res = _userRepository.Update(user);
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

        public ServiceResult<bool> ChangeStatus(bool isAvailable)
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<bool>.FailureResult("No user is currently logged in.");
            }

            var user = _userRepository.GetUserById(_loggedInUser.ID);
            if (user == null)
            {
                return ServiceResult<bool>.FailureResult("User Not Found.");

            }
            user.IsAvailable = isAvailable;
            var res = _userRepository.Update(user);
            if (res)
            {
                return ServiceResult<bool>.SuccessResult(true, "Status changed successfully.");

            }
            return ServiceResult<bool>.FailureResult("Status Didnt Change");

        }

        public ServiceResult<List<User>> GetAllUsersByUserName(string userName)
        {
            if (_loggedInUser == null)
            {
                return ServiceResult<List<User>>.FailureResult("No user is currently logged in.");
            }

            var users = _userRepository.GetAllUsersByUsername(userName);

            if (users.Count == 0)
            {
                return ServiceResult<List<User>>.FailureResult("No users found.");
            }

            return ServiceResult<List<User>>.SuccessResult(users);
        }


    }

}