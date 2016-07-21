using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using System;

// AspNet.* -> AspNetCore.*
// System.IdentityModel.Tokens -> Microsoft.IdentityModel.Tokens
// Microsoft.AspNetCore.Builder -> Microsoft.AspNetCore.Authentication.JwtBearer

namespace JsonWebTokenSample.Common
{
    /// <summary>
    /// This class is used in Resources.cs of JsonWebTokenSample.Authorization.Api to initialize security resources.
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Generate RSA key. This code is for development
        /// </summary>
        /// <returns></returns>
        public static RsaSecurityKey GenerateRsaKeys()
        {
            RsaSecurityKey rsaKey = null;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsaKey = new RsaSecurityKey(rsa.ExportParameters(true));
                rsa.PersistKeyInCsp = false;
            }
            return rsaKey;
        }

        public static RSAParameters GetRandomKey()
        {
         
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    return rsa.ExportParameters(true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Create signing credential for generating access token string.
        /// </summary>
        /// <param name="key">The key to signing credentials</param>
        /// <returns>SigningCredentials object</returns>
        public static SigningCredentials CreateSigningCredentials(RsaSecurityKey key)
        {


            //  Changed to this for temporary
            return new SigningCredentials
                (
                    key,
                    SecurityAlgorithms.RsaSha256Signature
                );
        }

        /// <summary>
        /// Create JWT option
        /// </summary>
        /// <returns></returns>
        public static JwtBearerOptions CreateJwtBearerOption(RsaSecurityKey key, string tokenIssuer, string tokenAudience)
        {
            var jwtBearerOptions = new JwtBearerOptions();

            
            jwtBearerOptions.AutomaticAuthenticate = true;
            jwtBearerOptions.AutomaticChallenge = true;
            jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
            jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = key;
            jwtBearerOptions.TokenValidationParameters.ValidIssuer = tokenIssuer;
            jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
            jwtBearerOptions.TokenValidationParameters.ValidateLifetime = true;
            jwtBearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;



            jwtBearerOptions.TokenValidationParameters.ValidAudience = tokenAudience;
            return jwtBearerOptions;
        }

        // The test code
        /*
        public static string GenerateAccessToken()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                _key = new RsaSecurityKey(rsa.ExportParameters(true));
                rsa.PersistKeyInCsp = false;
            }
            SigningCredentials _signingCredentials = new SigningCredentials
            (
                _key,
                SecurityAlgorithms.RsaSha256Signature,
                SecurityAlgorithms.Sha256Digest,
                "secret"
            );

            var _jwtBearerOptions = new JwtBearerOptions();
            _jwtBearerOptions.AutomaticAuthentication = true;
            _jwtBearerOptions.AutomaticChallenge = true;
            _jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = _key;
            _jwtBearerOptions.TokenValidationParameters.ValidIssuer = _tokenIssuer;
            _jwtBearerOptions.TokenValidationParameters.ValidAudience = _tokenAudience;

            JwtSecurityTokenHandler handler = _jwtBearerOptions
                .SecurityTokenValidators
                .OfType<JwtSecurityTokenHandler>()
                .First();

            JwtSecurityToken securityToken =
                handler.CreateToken
                (
                    issuer: _tokenIssuer,
                    audience: _tokenAudience,
                    signingCredentials: _signingCredentials,

                    subject: new ClaimsIdentity
                    (
                        new Claim[]
                        {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim(ClaimTypes.Role, "teacher"),
                        }
                    ),
                    expires: DateTime.Today.AddDays(1)
                );

            string token = handler.WriteToken(securityToken);

            Console.Out.Write(VerifyToken(token));

            return token;
        }

        static bool VerifyToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = _key,
                ValidAudience = _tokenAudience,
                ValidIssuer = _tokenIssuer,
                ValidateAudience = true,
                ValidateIssuer = true
                //ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            //... manual validations return false if anything untoward is discovered
            return validatedToken != null;
        }
        */
    }
}
