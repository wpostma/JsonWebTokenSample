using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonWebTokenSample.Repository
{
    /// <summary>
    /// Database Repo for Users
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public Models.Users Login(string username, string password)
        {
            /*
            Models.Users data = null;

            
            data = DbContext.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
            return data;
            */
            throw new NotImplementedException("not implemented");

        }
    }
}
