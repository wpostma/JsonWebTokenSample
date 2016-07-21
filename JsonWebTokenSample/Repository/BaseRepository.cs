using JsonWebTokenSample.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CS1591

namespace JsonWebTokenSample.Repository
{
    public class BaseRepository : IBaseRepository
    {
        public DbContext DbContext { get; set; }

        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Connect to database
        /// </summary>
        /// <returns></returns>
        public bool ConnectDataBase()
        {
            bool result = false;
            try
            {
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Disconnect database
        /// </summary>
        /// <returns></returns>
        public bool DisconnectDataBase()
        {
            bool result = false;
            try
            {
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }

    //[Serializable()]
    //public class DatabaseAccessLayerFailureException : System.ApplicationException
    //{
    //    public DatabaseAccessLayerFailureException() { }
    //    public DatabaseAccessLayerFailureException(string message) : base(message) { }
    //}
}
