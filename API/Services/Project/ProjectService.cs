using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Project;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Project
{
    /// <summary>
    /// Handles interactions with Projects.
    /// </summary>
    public class ProjectService
    {
        public ProjectTable ProjectTable { get; set; }

        public ProjectService(ProjectTable pTable)
        {
            ProjectTable = pTable;
        }

        /// <summary>
        /// Gets all Projects from database.
        /// </summary>
        /// Success result where result content is a list of <see cref="BasicDataProject"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await GetAll();
                if (result == null) return Failure("Not a single Project was found.");

                return Success(result);
            }
        }

        /// <summary>
        /// Gets a Project by its id.
        /// </summary>
        /// <param name="projectId">Project's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataProject"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GetProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await Get(projectId);
                if (result == null) return Failure(
                    string.Format("No Project with id {0} was found.", projectId)
                );

                return Success(result);
            }
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataProject>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return (await ctx[ProjectTable].Connection
                    .QueryAsync<BasicDataProject>(
                         @"SELECT
                            *
                        FROM 
                            ITIH.vProjects;"
                    )).ToArray();
            }
        }

        private async Task<BasicDataProject> Get(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[ProjectTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProject>(
                        @"SELECT
                        *
                    FROM 
                        ITIH.vProjects
                    WHERE
                        ProjectId = @Id;",
                        new { Id = projectId }
                    );
            }
        }
    }
}
