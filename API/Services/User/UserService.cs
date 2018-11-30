using CK.DB.Actor;
using CK.SqlServer;
using Dapper;
using ITI.Human.ViewModels.User;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.User
{
    public class UserService
    {
        public UserTable UserTable { get; set; }

        public UserService(UserTable uTable)
        {
            UserTable = uTable;
        }

        /// <summary>
        /// Gets all Users from database.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataUser"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await GetAll();

                if (result == null) return Failure("No element was found.");
                return Success(result);
            }
        }

        /// <summary>
        /// Gets a User by its id.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataUser"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int userId)
        {
            var result = await Get(userId);

            if (result == null) return Failure("No element was found.");
            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataUser>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return (await ctx[UserTable].Connection
                    .QueryAsync<BasicDataUser>(
                        @"SELECT
                            *
                        FROM 
                            CK.tUser;"
                    )).ToArray();
            }
        }

        private async Task<BasicDataUser> Get(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataUser>(
                        @"SELECT
                            *
                        FROM
                            CK.tUser
                        WHERE
                            UserId = @id;",
                        new { id = userId }
                    );
            }
        }
    }
}
