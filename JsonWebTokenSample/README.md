# Code Sample JWT Authorization API NOT Suitable for Real Use

C# 6, ASP.NET Core 1.0, dotnet 1.0.0 rtm

This is a minimal ASP.NET Core token generation demo. It currently does not actually work.
Derived from https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample

Problem with this code:

1. Run it inside the IDE, so you can see the Console Log Output.

2. From a command prompt run "python test\testloginapi.py" (requires python 3)

3. The test does a POST to http://localhost:54993/authorize/login?username=TEST&password=SECRET

3. The demo app returns a token something like this:
    eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9.

4. Now we try to use that token, as an HTTP header and make a GET request to a "secured" asp.net mvc api.
    http://localhost:54993/authorizetest/test  

5. Now we get this error:


     Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerMiddleware:Information: Failed to validate the token eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9..
     Microsoft.IdentityModel.Tokens.SecurityTokenInvalidSignatureException: IDX10504: Unable to validate signature, token does not have a signature: 'eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9.'
       at System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.ValidateSignature(String token, TokenValidationParameters validationParameters)
        at System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.ValidateToken(String token, TokenValidationParameters validationParameters, SecurityToken& validatedToken)
        at Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler.<HandleAuthenticateAsync>d__1.MoveNext()
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerMiddleware:Information: Bearer was not authenticated. Failure message: IDX10504: Unable to validate signature, token does not have a signature: 'eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6IlRFU1QiLCJuYmYiOjE0NjkxMDc0NzUsImV4cCI6MTQ2OTE5Mzg3NSwiaWF0IjoxNDY5MTA3NDc1LCJpc3MiOiJEVU1NWSIsImF1ZCI6IkRVTU1ZL3Jlc291cmNlcyJ9.'
        Microsoft.AspNetCore.Authorization.DefaultAuthorizationService:Information: Authorization failed for user: .


----

Revision log
----

|Date    | Rev|      Rev   |
|--------|-------------|
|2016-07-21| 0.1|  Ported to .net core 1.0 rtm - warren.postma@gmail.com|



