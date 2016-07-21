using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Builder;

using Newtonsoft.Json.Linq;



using JsonWebTokenSample.Models;
using JsonWebTokenSample.Repository;
using Microsoft.Extensions.Logging;
using JsonWebTokenSample.Common;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
//using Microsoft.AspNetCore.Identity; [[RC2!!]]

namespace JsonWebTokenSample.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class AuthorizeController : Controller
    {
#if DATABASE_SUPPORT
        public IUserRepository _repository { get; set; }
#endif

        //[FromServices]
        public JwtBearerOptions BearerOptions { get; set; }

        //[FromServices]
        public SigningCredentials SigningCredentials { get; set; }

        private readonly ILogger<AuthorizeController> _logger;

        public AuthorizeController(ILogger<AuthorizeController> logger,
#if DATABASE_SUPPORT
            IUserRepository repository,
#endif
            JwtBearerOptions jwtBearerOptions, SigningCredentials signingCredentials)
        {
            this._logger = logger;
#if DATABASE_SUPPORT
            this._repository = repository;
#endif
            this.BearerOptions = jwtBearerOptions;
            this.SigningCredentials = signingCredentials;
        }


        
        [HttpGet("login")]
        public JObject LoginGet()
        {

            // "User Friendly" : Front End and Back End Devs would probably like to check http://localhost:x/authorize/login from a browser.
            // Since there's no way to "fetch" a token, this is just a convenient "warning" landing page.

            var result = new JObject();

            result.Add("use_http_post", "authorize api requires HTTP post");

            var appVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            result.Add("version",  appVersion );

            result.Add("origin", "RamSoft Authorization API");

            return result;

        }

        
        [HttpPost("login")]
        public JObject Login(Credentials credentials)
        {            
            bool success = false;
            JObject result = null;
            string password = string.Empty;
            try
            {
                // Object response
                result = new JObject();

                
                password = credentials.Password.ToSha256Hashing(); // extension method ToSha256Hashing is in Common/ExtensionMethod.cs

                // query DB here to validate credential's user
#if DATABASE_SUPPORT
                Users user = _repository.Login(credentials.UserName, password);
                success = user != null;
#else
                success = (credentials.UserName == "TEST");
#endif

                // Assume after authenticate successfully, signing credential                
                // check if verify OK, do generate access token string
                if (success)
                {
                    _logger.LogInformation("Get User Successfully {0} ", credentials.UserName);
                    success = true;
                    // TODO: these information need to double check
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, credentials.UserName));
#if DATABASE_SUPPORT
                    claims.Add(new Claim(ClaimTypes.Role, user.InternalRoleID.ToString()));
                    claims.Add(new Claim(ClaimTypes.StreetAddress, user.Address));
                    claims.Add(new Claim(ClaimTypes.Sid, user.InternalUserID.ToString()));
#endif

                    JwtSecurityTokenHandler handler = BearerOptions
                        .SecurityTokenValidators
                        .OfType<JwtSecurityTokenHandler>()
                        .First();

            //     BearerOptions.

                    var tokenData = new SecurityTokenDescriptor
                    {
                        // Without this, you get IDX10504
                        // SigningCredentials comes from a controller constructor parameter.
                        SigningCredentials = this.SigningCredentials,

                        Issuer = BearerOptions.TokenValidationParameters.ValidIssuer,
                        Audience = BearerOptions.TokenValidationParameters.ValidAudience,
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(1),
                        NotBefore = DateTime.Now
                    };

                    /*JwtSecurityToken*/
                    var securityToken =
                        handler.CreateToken
                        (
                            tokenData
                        );

                    string token = handler.WriteToken(securityToken);
                    result.Add("access_token", token);
                    result.Add("token_type", "bearer");
                    result.Add("success", success);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in Login Api, Message : " + ex.Message);
                result.Add("success", false);
            }
            finally
            {

            }

            _logger.LogInformation(string.Format("User Requested Logins with UserName = {0} , Password = {1}", credentials.UserName, password));
            return result;
        }
    }
}
