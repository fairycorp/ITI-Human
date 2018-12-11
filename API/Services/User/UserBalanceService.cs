using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.User;
using Stall.Guard.System;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.User
{
    public class UserBalanceService
    {
        public UserBalanceTable UserBalanceTable { get; set; }

        public UserBalanceService(UserBalanceTable uBTable)
        {
            UserBalanceTable = uBTable;
        }

        /// <summary>
        /// Gets a User's Balance by its id.
        /// </summary>
        /// <param name="userBalanceId">User's Balance id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataUserBalance"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int userBalanceId)
        {
            var result = await Get(userBalanceId);
            if (result == null) return Failure(
                string.Format("No UserBalance with id {0} was found.", userBalanceId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets a Balance from a User.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataUserBalance"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromUser(int userId)
        {
            var result = await GetFromUser(userId);
            if (result == null) return Failure(
                string.Format("No UserBalance with userId {0} was found.", userId)
            );

            return Success(result);
        }
        
        /// <summary>
        /// Creates a new User Balance.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="projectId">Project's id.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedCreateUserBalance(int userId, int projectId)
        {
            var result = await Create(userId, projectId);
            if (result == 0) return Failure("Error in creation process.");

            return Success(result);
        }

        /// <summary>
        /// Updates a User's Balance.
        /// </summary>
        /// <param name="userBalanceId">User's Balance id.</param>
        /// <param name="amount">In/Out.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedUpdateUserBalance(int userBalanceId, int amount)
        {
            var doesUBExist = await Get(userBalanceId);
            if (doesUBExist == null) return Failure(
                 string.Format("No UserBalance with id {0} was found.", userBalanceId)
            );

            var result = await UpdateUserBalance(userBalanceId, amount);
            if (!result) return Failure("No update was proceeded.");

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<BasicDataUserBalance> Get(int userBalanceId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserBalanceTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataUserBalance>(
                        "SELECT * FROM ITIH.tUserBalance WHERE UserBalanceId = @id",
                        new { id = userBalanceId }
                    );
            }
        }

        private async Task<BasicDataUserBalance> GetFromUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserBalanceTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataUserBalance>(
                        "SELECT * FROM ITIH.tUserBalance WHERE UserId = @id",
                        new { id = userId }
                    );
            }
        }

        private async Task<int> Create(int userId, int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await UserBalanceTable.Create(ctx, 0, userId, projectId);
            }
        }

        private async Task<bool> UpdateUserBalance(int userBalanceId, int amount)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await UserBalanceTable.Update(ctx, 0, userBalanceId, amount);
            }
        }
    }
}
