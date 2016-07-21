
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;

namespace JsonWebTokenSample.Common
{
    public class Resources
    {
        private static volatile Resources instance;
        private static object syncRoot = new Object();

        public const string tokenIssuer = "DUMMY";
        public const string tokenAudience = "DUMMY/resources";

        public RsaSecurityKey Key { get; private set; }
        public SigningCredentials SigningCredentials { get; private set; }
        public JwtBearerOptions JwtBearerOptions { get; private set; }

        /// <summary>
        /// Create RSA key when initilizing this class.
        /// </summary>
        private Resources()
        {            
        }

        /// <summary>
        /// Get instance of Encryption class in thread-safe.
        /// </summary>
        public static Resources Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Resources();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Initilize security data.
        /// </summary>
        public void Init()
        {
            // Generate key
            Key = Encryption.GenerateRsaKeys();

            // Create Signing Credential based on key
            SigningCredentials = Encryption.CreateSigningCredentials(Key);

            // Create 
            JwtBearerOptions = Encryption.CreateJwtBearerOption(Key, tokenIssuer, tokenAudience);

            
        }


    }
}
