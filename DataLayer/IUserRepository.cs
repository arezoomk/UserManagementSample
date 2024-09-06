using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IUserRepository
    {  
       Task<User?> GetUserByUsernameAsync(string username);
       Task<bool> AddAsync(User user);
       Task<bool> UpdateAsync(User user);
       Task<User?> GetUserByIdAsync(int id);
       Task<List<User>> GetAllUsersByUsernameAsync(string username);
    }
}