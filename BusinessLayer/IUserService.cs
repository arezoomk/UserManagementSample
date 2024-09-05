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
        public ServiceResult<bool> ChangePassword(string oldPassword, string newPassword);
        public ServiceResult<bool> RegisterUser(string username, string password);
        public ServiceResult<bool> Logout();
        public ServiceResult<bool> Login(string username, string password);
        public ServiceResult<bool> ChangeStatus(bool isAvailable);
        public ServiceResult<List<User>> GetAllUsersByUserName(string userName);
    }
}