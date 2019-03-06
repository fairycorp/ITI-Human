using CK.DB.Actor;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Fork.ViewModels.User;
using Fork.ViewModels.User.Profile;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.User
{
    /// <summary>
    /// Handles interactions with Users.
    /// </summary>
    public class UserService
    {
        public UserTable UserTable { get; set; }
        public UserDetailsTable UserDetailsTable { get; set; }
        public SchoolMemberTable SchoolMemberTable { get; set; }

        public UserService(UserTable uTable, UserDetailsTable uDTable,
            SchoolMemberTable sMTable)
        {
            UserTable = uTable;
            UserDetailsTable = uDTable;
            SchoolMemberTable = sMTable;
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

                if (result == null) return Failure("Not a single User was found.");
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

            if (result == null) return Failure(
                string.Format("No User with id {0} was found.", userId)
            );
            return Success(result);
        }

        /// <summary>
        /// Gets a User's profile.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedGetProfile(int userId)
        {
            var result = await GetProfile(userId);

            if (result == null) return Failure(
                string.Format("No Profile with userId {0} was found.", userId)
            );
            return Success(result);
        }

        /// <summary>
        /// Gets user's current profile setup completed state.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Success result where content is true if profile is completed.</returns>
        public async Task<GuardResult> GuardedGetProfileSetupCompletedState(int userId)
        {
            var doesUserExist = await Get(userId);
            if (doesUserExist == null) return Failure("User does not exist in database.");

            var result = await GetProfileSetupCompletedState(userId);
            return Success(result);
        }

        /// <summary>
        /// Setups user's current profile. Creates it if does not exist.
        /// </summary>
        /// <param name="model">Creation view model.</param>
        /// <returns>Success result where content gathers created id outputs.</returns>
        public async Task<GuardResult> GuardedSetupProfile(CreationViewModel model)
        {
            var doesUserExist = await Get(model.UserId);
            if (doesUserExist == null) return Failure("User does not exist in database.");

            var results = await SetupProfile(model);
            return Success(results);
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

        private async Task<BasicDataUserProfile> GetProfile(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataUserProfile>(
                        "SELECT * FROM FRK.vUserProfile WHERE UserId = @id;",
                        new { id = userId }
                    );
            }
        }

        private async Task<bool> GetProfileSetupCompletedState(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesMemberExist = await ctx[SchoolMemberTable].Connection
                    .QueryFirstOrDefaultAsync<int>(
                        "SELECT SchoolMemberId FROM FRK.tSchoolMember WHERE UserId = @id;",
                        new { id = userId }
                    );

                if (doesMemberExist == 0) return false;
                return true;
            }
        }

        private async Task<int[]> SetupProfile(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesDetailsExist = await ctx[UserDetailsTable].Connection
                    .QueryFirstOrDefaultAsync<int>(
                        "SELECT UserDetailsId FROM FRK.tUserDetails WHERE UserId = @id;",
                        new { id = model.UserId }
                    );

                var doesMemberExist = await ctx[SchoolMemberTable].Connection
                    .QueryFirstOrDefaultAsync<int>(
                        "SELECT SchoolMemberId FROM FRK.tSchoolMember WHERE UserId = @id;",
                        new { id = model.UserId }
                    );

                int result1 = 0, result2 = 0;
                bool launchCreationProcess = false;
                switch (model.SchoolStatusId)
                {
                    case 0:
                        launchCreationProcess = true;
                        break;

                    case 1:
                        if (model.SecretCode == "A8191?KS#QL?QM°&S+=QJN61I4P0QI1S&&3840#-2DK")
                            launchCreationProcess = true;
                        break;

                    case 2:
                        if (model.SecretCode == "A8191?KS#QL?QM°&S+=QJN61I4P0QI1S&&3840#-2DK")
                            launchCreationProcess = true;
                        break;

                    case 3:
                        launchCreationProcess = true;
                        break;
                }
                if (launchCreationProcess && doesDetailsExist == 0)
                {
                    var date = DateTime.Now;
                    result1 = await UserDetailsTable.Create(ctx, model.UserId, model.UserId, model.Firstname, model.Lastname, date.AddHours(1));

                    if (doesMemberExist == 0)
                    {
                        result2 = await SchoolMemberTable.Create(ctx, model.UserId, model.UserId, model.SchoolStatusId);
                    }
                }
                if (launchCreationProcess && doesDetailsExist > 0)
                {
                    result1 = -1;
                    var date = DateTime.Now;
                    await UserDetailsTable.Update(ctx, model.UserId, model.UserId, model.Firstname, model.Lastname, date.AddHours(1));

                    if (doesMemberExist == 0)
                    {
                        result2 = await SchoolMemberTable.Create(ctx, model.UserId, model.UserId, model.SchoolStatusId);
                    }
                }
                return new int[] { result1, result2 };
            }
        }
    }
}
