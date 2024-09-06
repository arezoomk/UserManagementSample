using BusinessLayer.DTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IUserService
    {
        Task<ServiceResult<bool>> ChangePasswordAsync(string oldPassword, string newPassword);
        Task<ServiceResult<bool>> RegisterUserAsync(string username, string password);
        ServiceResult<bool> Logout();
        Task<ServiceResult<bool>> LoginAsync(string username, string password);
        Task<ServiceResult<bool>> ChangeStatusAsync(bool isAvailable);
        Task<ServiceResult<List<User>>> GetAllUsersByUserNameAsync(string userName);
    }
}