
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace NextAuthjs.Common;

public static class AuthUtils
{
    public static Task ExtractToken(MessageReceivedContext rawContext)
    {
        /*var context = rawContext.RequireNotNull(paramName: "context");

        var cookieToken = 
            context.Request.Cookies["__Secure-next-auth.session-token"] 
            ?? context.Request.Cookies["next-auth.session-token"];

        string urlToken = context.Request.Query["token"]; //// Here we are using string instead of var on purpose - https://stackoverflow.com/questions/60191812/why-is-stringvalues-assignable-to-string

        string? headerToken = null;

        string authHeader = context.Request.Headers["Authorization"]; //// Here we are using string instead of var on purpose

        if (authHeader != null
            && authHeader.StartsWith("Bearer "))
        {
            headerToken = authHeader.Substring("Bearer ".Length).Trim();
        }

        context.Token = cookieToken ?? urlToken ?? headerToken ?? context.Token;
        return Task.CompletedTask;*/
        return Task.CompletedTask;
    }
}