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
       public User? GetUserByUsername(string username);
       public bool Add(User user);
       public bool Update(User user);
       public User? GetUserById(int id);
       public List<User> GetAllUsersByUsername(string username);
    }
}