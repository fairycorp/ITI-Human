using API.Services.Github;
using CK.AspNet.Auth;
using CK.Auth;
using CK.Core;
using CK.DB.Actor;
using CK.DB.Auth;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Services.Auth
{
    /// <summary>
    /// Auto creation of account from Github login based on a simple invitation.
    /// This implements the optional <see cref="IWebFrontAuthAutoCreateAccountService"/>
    /// that is registered in the DI container.
    /// </summary>
    public class AutoCreateAccountService : IWebFrontAuthAutoCreateAccountService
    {
        private GithubService GithubService { get; }
        private UserTable UserTable { get; }
        private UserAvatarsTable UserAvatarsTable { get; }
        private UserDetailsTable UserDetailsTable { get; }
        private IAuthenticationDatabaseService DbAuth { get; }
        private IAuthenticationTypeSystem TypeSystem { get; }

        public AutoCreateAccountService(
           GithubService gService,
           UserTable userTable,
           UserAvatarsTable uATable,
           UserDetailsTable uDTable,
           IAuthenticationDatabaseService dbAuth,
           IAuthenticationTypeSystem typeSystem)
        {
            GithubService = gService;
            UserTable = userTable;
            UserAvatarsTable = uATable;
            UserDetailsTable = uDTable;
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
            const string GitHubAccountIdPropName = "GitHubAccountId";
            const string UserFullNamePropName = "Name";
            const string AvatarUrlPropName = "AvatarUrl";
            object accountId = null;
            object tmpUserName = null;
            object finalUserName = Guid.NewGuid();
            string userAvatar = null;

            var payloadType = context.Payload.GetType();
            var props = new List<PropertyInfo>(payloadType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == GitHubAccountIdPropName)
                    accountId = prop.GetValue(context.Payload, null);

                if (prop.Name == UserFullNamePropName)
                {
                    tmpUserName = prop.GetValue(context.Payload, null).ToString();
                    finalUserName = BuildUserName(prop.GetValue(context.Payload, null).ToString());
                }

                if (prop.Name == AvatarUrlPropName)
                {
                    userAvatar = prop.GetValue(context.Payload, null).ToString();
                }

            }
            if (accountId == null) return null;

            var ctx = context.HttpContext.RequestServices.GetService<ISqlCallContext>();
            var result = ValidateLoginContext(ctx, monitor, context);

            int idUser = await UserTable.CreateUserAsync(ctx, 1, finalUserName.ToString());
            UCLResult dbResult = await result.Provider.CreateOrUpdateUserAsync(ctx, 1, idUser, context.Payload, UCLMode.CreateOnly | UCLMode.WithActualLogin);
            if (dbResult.OperationResult != UCResult.Created) return null;
            int idAvatarUser = await CreateAvatar(idUser, userAvatar);
            int idDetails = await CreateDetails(idUser, tmpUserName.ToString());
            return await DbAuth.CreateUserLoginResultFromDatabase(ctx, TypeSystem, dbResult.LoginResult);
        }

        private async Task<int> CreateAvatar(int userId, string url)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await UserAvatarsTable.Create(ctx, 1, userId, url);
            }
        }

        private async Task<int> CreateDetails(int userId, string fullname)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var difference = fullname.Split(" ");
                var firstName = difference[0];
                var lastName = difference[1];
                return await UserDetailsTable.Create(ctx, 1, userId, firstName, lastName, DateTime.Now);
            }
        }

        private string BuildUserName(string fullname)
        {
            var difference = fullname.Split(" ");
            var firstName = difference[0].ToCharArray();
            var lastName = difference[1];

            string createIdentifier()
            {
                Random rdm = new Random();
                return string.Format(
                    "{0}{1}{2}{3}",
                    rdm.Next(0, 9),
                    rdm.Next(0, 9),
                    rdm.Next(0, 9),
                    rdm.Next(0, 9)
                );
            }

            return string.Format(
                "{0}.{1}#{2}",
                firstName[0].ToString().ToLower(),
                lastName.ToLower(),
                createIdentifier()
            );
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
