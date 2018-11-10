using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Classroom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Classroom
{
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
        public async Task<IEnumerable<BasicDataClassroom>> GetAllClassrooms()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[ClassroomTable].Connection
                .QueryAsync<BasicDataClassroom>(
                    @"SELECT
                        *
                    FROM
                        ITIH.tClassroom;"
                );
                return result.ToArray();
            }
        }

        /// <summary>
        /// Gets a Classroom by its id.
        /// </summary>
        /// <param name="classroomId">Classroom id.</param>
        public async Task<BasicDataClassroom> GetClassroom(int classroomId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[ClassroomTable].Connection
                .QueryFirstOrDefaultAsync<BasicDataClassroom>(
                    @"SELECT
                        *
                    FROM
                        ITIH.tClassroom
                    WHERE
                        ClassroomId = @Id",
                    new { Id = classroomId }
                );
                return result;
            }
        } 
    }
}
