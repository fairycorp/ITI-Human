using Microsoft.AspNetCore.Http;
using Stall.Guard.System;

using static API.Services.Helper.ResultFactory;


namespace API.Services.Auth
{
    /// <summary>
    /// Handles authentication checks.
    /// </summary>
    public class AuthCheckService
    {
        public AuthCheckService()
        {
        }

        /// <summary>
        /// Checks if user is authenticated in mentionned current context.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <returns></returns>
        public GuardResult CheckUserAuthenticationLevel(HttpContext context)
        {
            return (context.WebFrontAuthenticate().Level > 0)
                ? Success(null) : Failure("User is not authenticated.");
        }

        /// <summary>
        /// Checks if mentionned userId is the authenticated User's id of mentionned current context.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="userId">Supposed user's id.</param>
        /// <returns></returns>
        public GuardResult CheckUserAuthenticity(HttpContext context, int userId)
        {
            return (context.WebFrontAuthenticate().User.UserId == userId)
                ? Success(null) : Failure("User is not who he said he was.");
        }
    }
}
