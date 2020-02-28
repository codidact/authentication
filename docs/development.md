# Development Instructions

The authentication server is configured though the `src/WebApp/appsettings.json`
file. There are no comments allowed in this file, the following is pseudo code:

~~~json
{
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1#configuration
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning"
        }
    },
    "IdentityServer": {
        "Clients": [
            {
                "ClientId": "codidact_client",
                "ClientSecrets": [
                    {
                        // This is the SHA256 of 'foo' encoded using base64.
                        "Value": "LCa0a2j/xo/5m0U8HTBBNBNCLXBkg7+g+YpeiGJm564="
                    }
                ],
                "AllowedGrantTypes": [
                    "authorization_code"
                ],
                "AllowedScopes": [
                    "openid",
                    "profile"
                ],
                "RedirectUris": [
                    // The hostname must match with the hostname that core listens too. The path 'signin-oidc'
                    // must match with the 'CallbackUrl' parameter.
                    "http://localhost:5000/signin-oidc"
                ],
                "RequireConsent": false
            }
        ]
    },
    "ConnectionStrings": {
        // This path is relative to `src/WebApp`.
        "Authentication": "Data Source=authentication.db"
    }
}
~~~
