using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace checkoutdotcom.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate nextComponentDelegate;

        public AuthorizationMiddleware(RequestDelegate nextComponentDelegate)
        {
            this.nextComponentDelegate = nextComponentDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
                  
            if (context.Request.Headers.Keys.Contains("Authorization"))
            {
                var authorizationValue = context.Request.Headers["Authorization"];
                if (authorizationValue.Count == 1)
                {
                    if (authorizationValue[0].Split(' ').Length == 1)
                    {
                        // we might have just an API key in the authentication header.
                        // note that Checkout.com's library is not really convenient because it doesn't specify an authentication scheme, and
                        // I don't have the time to refactor their code anymore, so we'll adapt to their exotic authentication headers.
                        var apiKeyIdentity = new ClaimsIdentity(new[] { new Claim("ApiKeyIdentity", authorizationValue) }, "ApiKeyIdentity");
                        context.User.AddIdentity(apiKeyIdentity);
                    }
                }
            }

            await this.nextComponentDelegate.Invoke(context);
        }
    }


}