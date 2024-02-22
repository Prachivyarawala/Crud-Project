using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IUserRepositories
    {
        User GetOne(int id);
        bool Register(User user);
        void Update(User user);
        User Login(string email , string password);

        bool IsEmailExists(string email);
        

    }
}