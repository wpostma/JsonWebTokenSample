using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;


// Due to a policy, the authorizetest/test endpoint will allow us to check if our authorization logic works.
namespace JsonWebTokenSample.Controllers
{
    /// <summary>
    ///  Test authorization
    /// </summary>
    [Authorize(Policy = "JsonWebTokenSamplePolicy")]
    [Route("[controller]")]
    public class AuthorizeTestController : Controller
    {

        /// <summary>
        ///  Test authorization 
        /// </summary>
        [HttpGet("test")]
        public JObject TestGet()
        {
            var result = new JObject();

            result.Add("test_get", "This is a test endpoint");

            var appVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            result.Add("version", appVersion);

            result.Add("origin", "Code Sample JWT Authorization API NOT Suitable for Real Use");

            return result;

        }
    }
}
