using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.IRepository
{
   public interface IUserRepository
    {
        bool IsUniqueUser(string Username);

        User Authenticate(string Username, string Password);

        User Register(string Username, string Password);
    }
}
