using CK.SqlServer;
using Dapper;
using Fork.Data;
using Fork.ViewModels.Classroom;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Classroom
{
    /// <summary>
    /// Handles interactions with Classrooms.
    /// </summary>
    public class ClassroomService
    {
        public ClassroomTable ClassroomTable { get; set; }

        public ClassroomService(ClassroomTable cTable)
        {
            ClassroomTable = cTable;
        }

        /// <summary>
        /// Gets all Classrooms from database.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataClassroom"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            var result = await GetAll();
            if (result == null) return Failure("Not a single Classroom was found.");

            return Success(result);
        }

        /// <summary>
        /// Gets a Classroom by its id.
        /// </summary>
        /// <param name="classroomId">Classroom id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataClassroom"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int classroomId)
        {
            var result = await Get(classroomId);
            if (result == null) return Failure(
                string.Format("No Classroom with id {0} was found.", classroomId)
            );

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataClassroom>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return (await ctx[ClassroomTable].Connection
                .QueryAsync<BasicDataClassroom>(
                    @"SELECT
                        *
                    FROM
                        FRK.tClassroom;"
                )).ToArray();
            }
        }

        private async Task<BasicDataClassroom> Get(int classroomId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[ClassroomTable].Connection
                .QueryFirstOrDefaultAsync<BasicDataClassroom>(
                    @"SELECT
                        *
                    FROM
                        FRK.tClassroom
                    WHERE
                        ClassroomId = @id;",
                    new { id = classroomId }
                );
            }
        }
    }
}
