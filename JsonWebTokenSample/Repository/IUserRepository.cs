using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonWebTokenSample.Repository
{
    /// <summary>
    /// A database connection repository interface for user logins
    /// </summary>
    public interface IUserRepository
    {
        Models.Users Login(string username, string password);
    }
}
