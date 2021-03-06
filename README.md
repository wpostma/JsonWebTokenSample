# Code Sample JWT Authorization API NOT Suitable for Real Use

C# 6, ASP.NET Core 1.0, dotnet 1.0.0 rtm

This is a minimal ASP.NET Core token generation demo. It  works (thanks to user @Pinpoint on stackoverflow) but it is NOT recommended for production use.

For production use, some proper OAUTH2 compliant server, something capable of [open ID  connect discovery](https://openid.net/specs/openid-connect-discovery-1_0.html#ProviderMetadata), would be preferred.


Derived from https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample

Problem with this code:

1. Run it inside the IDE, so you can see the Console Log Output.

2. From a command prompt run "python tests\testloginapi.py" (requires python 3)

        d:\dev\JsonWebTokenSample>python tests\testloginapi.py
        GET OK: 200 http://localhost:54993/authorize/login?username=TEST&pas...
        POST OK: 200 http://localhost:54993/authorize/login?username=TEST&pas...
        authorization token received... eyJhbGciOi...
        expected status 200 but got 401 for GET http://localhost:54993/authorizetest/test

3. The test does a POST to `http://localhost:54993/authorize/login?username=TEST&password=SECRET`


3. The demo app returns a token something like this:


      eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9.

4. Now we try to use that token, as an HTTP header and make a GET request to a "secured" asp.net mvc api.
   `http://localhost:54993/authorizetest/test  `

5. Originally  we would get this error, due to lack of signing credentials.


     Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerMiddleware:Information: Failed to validate the token eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9..
     Microsoft.IdentityModel.Tokens.SecurityTokenInvalidSignatureException: IDX10504: Unable to validate signature, token does not have a signature: 'eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9.'
       at System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.ValidateSignature(String token, TokenValidationParameters validationParameters)
        at System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.ValidateToken(String token, TokenValidationParameters validationParameters, SecurityToken& validatedToken)
        at Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler.<HandleAuthenticateAsync>d__1.MoveNext()
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerMiddleware:Information: Bearer was not authenticated. Failure message: IDX10504: Unable to validate signature, token does not have a signature: 'eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9.'
        Microsoft.AspNetCore.Authorization.DefaultAuthorizationService:Information: Authorization failed for user: .

6.  The minimal parts of the code provided have some stubs (currently conditionally defined out) where you could create a Repository (C# class for data layer connection) and then get user name and password (hopefully stored with a hash and a salt in your db).



----

References

Make a valid JSON Web token that is signed:
  - http://stackoverflow.com/questions/38506113/usejwtbearerauthentication-fails-with-idx10504-unable-to-validate-signature-to/38507044#38507044

Configure authorization server endpoint:
  -  http://stackoverflow.com/questions/30768015/configure-the-authorization-server-endpoint/30857524#30857524

General useful backgrounder:
  - https://stormpath.com/blog/token-authentication-asp-net-core

----

Revision log
----

|Date    | Rev|      Rev   |
|--------|-------------|
|2016-07-21| 0.1|  Ported to .net core 1.0 rtm - warren.postma@gmail.com|
|2016-07-21| 0.2|  Repairs, thanks to user Pinpoint on stackoverflow! |



