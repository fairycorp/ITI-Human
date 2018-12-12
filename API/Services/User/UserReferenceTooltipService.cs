using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.User.SchoolMember;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.User
{
    /// <summary>
    /// Handles interactions with User Tooltips.
    /// </summary>
    public class UserReferenceTooltipService
    {
        public SchoolMemberTable SchoolMemberTable { get; set; }

        public UserReferenceTooltipService(SchoolMemberTable sTable)
        {
            SchoolMemberTable = sTable;
        }

        /// <summary>
        /// Gets all Users' detailed SchoolMember info.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="DetailedDataUserReferenceTooltip"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            var result = await GetAll();
            if (result == null) return Failure("Not a single School Member was found.");

            return Success(result);
        }

        /// <summary>
        /// Gets a specific User's detailed SchoolMember info.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="DetailedDataUserReferenceTooltip"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int userId)
        {
            var result = await Get(userId);
            if (result == null) return Failure(
                string.Format("No School Member with id {0} was found.", userId)
            );

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<DetailedDataUserReferenceTooltip>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[SchoolMemberTable].Connection
                    .QueryAsync<DetailedDataUserReferenceTooltip>(
                        "SELECT * FROM ITIH.vSchoolMembers;"
                    );
            }
        }

        private async Task<DetailedDataUserReferenceTooltip> Get(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[SchoolMemberTable].Connection
                    .QueryFirstOrDefaultAsync<DetailedDataUserReferenceTooltip>(
                        "SELECT * FROM ITIH.vSchoolMembers WHERE UserId = @id;",
                        new { id = userId }
                    );
            }
        }
    }
}
