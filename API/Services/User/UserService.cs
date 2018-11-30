using CK.DB.Actor;
using CK.SqlServer;
using Dapper;
using ITI.Human.ViewModels.User;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Gets a User by its id.
        /// </summary>
        /// <param name="userId">Classroom id.</param>
        private async Task<UserBasicData> GetUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserTable].Connection
                    .QueryFirstOrDefaultAsync<UserBasicData>(
                        @"SELECT
                            UserId
                        FROM
                            CK.tUser
                        WHERE
                            UserId = @id",
                        new { id = userId }
                    );
            }
        }

        /// <summary>
        /// Gets all Projects from database.
        /// </summary>
        public async Task<GuardResult> GetAllUsers()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[UserTable].Connection
                    .QueryAsync<UserBasicData>(
                         @"SELECT
                            *
                        FROM 
                            CK.tUser;"
                    );
                if (result == null) return Failure("No element was found.");
                return Success(result.ToArray());
            }
        }

        /// <summary>
        /// Gets all Users from database.
        /// </summary>
        public async Task<IEnumerable<UserBasicData>> GetAllUsers()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[UserTable].Connection
                .QueryAsync<UserBasicData>(
                    @"SELECT
                        *
                    FROM
                        CK.tUser;"
                );
                return result.ToArray();
            }
        }
    }
}
