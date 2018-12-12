using CK.AspNet.Auth;
using CK.Auth;
using CK.Core;
using CK.DB.Actor;
using CK.DB.Auth;
using CK.SqlServer;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Services
{
    /// <summary>
    /// Auto creation of account from Github login based on a simple invitation.
    /// This implements the optional <see cref="IWebFrontAuthAutoCreateAccountService"/>
    /// that is registered in the DI container.
    /// </summary>
    public class AutoCreateAccountService : IWebFrontAuthAutoCreateAccountService
    {
        private UserTable UserTable { get; }
        private IAuthenticationDatabaseService DbAuth { get; }
        private IAuthenticationTypeSystem TypeSystem { get; }

        public AutoCreateAccountService(
           UserTable userTable,
           IAuthenticationDatabaseService dbAuth,
           IAuthenticationTypeSystem typeSystem)
        {
            UserTable = userTable;
            DbAuth = dbAuth;
            TypeSystem = typeSystem;
        }

        /// <summary>
        /// Called for each failed login <see cref="UserLoginResult.IsUnregisteredUser"/> 
        /// is true and when there is no current authentication.
        /// This checks that the authentication is from GitHub and if it is the user
        /// is created an then logged in.
        /// </summary>
        /// <param name="monitor">The monitor to use.</param>
        /// <param name="context">The user creation context.</param>
        public async Task<UserLoginResult> CreateAccountAndLoginAsync(IActivityMonitor monitor, IWebFrontAuthAutoCreateAccountContext context)
        {
            object accountId = null;
            object userName = Guid.NewGuid();

            Type payloadType = context.Payload.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(payloadType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "GitHubAccountId")
                    accountId = prop.GetValue(context.Payload, null);

                if (prop.Name == "Name")
                    userName = BuildUserName(prop.GetValue(context.Payload, null).ToString());
            }
            if (accountId == null) return null;

            var ctx = context.HttpContext.RequestServices.GetService<ISqlCallContext>();
            var result = ValidateLoginContext(ctx, monitor, context);

            int idUser = await UserTable.CreateUserAsync(ctx, 1, userName.ToString());
            UCLResult dbResult = await result.Provider.CreateOrUpdateUserAsync(ctx, 1, idUser, context.Payload, UCLMode.CreateOnly | UCLMode.WithActualLogin);
            if (dbResult.OperationResult != UCResult.Created) return null;
            return await DbAuth.CreateUserLoginResultFromDatabase(ctx, TypeSystem, dbResult.LoginResult);
        }

        private string BuildUserName(string fullname)
        {
            var difference = fullname.Split(" ");
            var firstName = difference[0].ToCharArray();
            var lastName = difference[1];

            return string.Format("{0}.{1}", firstName[0].ToString().ToLower(), lastName.ToLower());
        }

        private struct ValidateResult
        {
            public IGenericAuthenticationProvider Provider { get; }

            public ValidateResult(IGenericAuthenticationProvider provider)
                => Provider = provider;
        }

        ValidateResult ValidateLoginContext(ISqlCallContext sqlCtx, IActivityMonitor monitor, IWebFrontAuthAutoCreateAccountContext context)
        {
            if (context.CallingScheme != "GitHub")
            {
                throw new ArgumentException("Only Github provider is supported.");
            }
            return new ValidateResult(DbAuth.FindProvider(context.CallingScheme));
        }
    }
}
