using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Project;
using ITI.Human.ViewModels.Project.Member;
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

        public ProjectMemberTable ProjectMemberTable { get; set; }

        public ProjectService(ProjectTable pTable, ProjectMemberTable pMTable)
        {
            ProjectTable = pTable;
            ProjectMemberTable = pMTable;
        }

        /// <summary>
        /// Gets all Projects from database.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataProject"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            var result = await GetAll();
            if (result == null) return Failure("Not a single Project was found.");

            return Success(result);
        }

        /// <summary>
        /// Gets a Project by its id.
        /// </summary>
        /// <param name="projectId">Project's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataProject"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int projectId)
        {
            var result = await Get(projectId);
            if (result == null) return Failure(
                string.Format("No Project with id {0} was found.", projectId)
            );

            return Success(result);
        }

        public async Task<GuardResult> GuardedGetAllFromUser(int userId)
        {
            var result = await GetAllFromUser(userId);
            if (result == null) return Failure(
                string.Format("No Project with userIid {0} was found.", userId)
            );

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataProject>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var projects = await ctx[ProjectTable].Connection
                    .QueryAsync<BasicDataProject>(
                         @"SELECT
                            *
                        FROM 
                            ITIH.vProjects;"
                    );
                foreach (var project in projects)
                {
                    project.Members = await ctx[ProjectMemberTable].Connection
                        .QueryAsync<DetailedDataProjectMember>(
                            "SELECT * FROM ITIH.vProjectMembers WHERE ProjectId = @id;",
                            new { id = project.ProjectId }
                        );
                }

                return projects;
            }
        }

        private async Task<BasicDataProject> Get(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var project = await ctx[ProjectTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProject>(
                        @"SELECT
                        *
                    FROM 
                        ITIH.vProjects
                    WHERE
                        ProjectId = @Id;",
                        new { Id = projectId }
                    );
                project.Members = await ctx[ProjectMemberTable].Connection
                    .QueryAsync<DetailedDataProjectMember>(
                        "SELECT * FROM ITIH.vProjectMembers WHERE ProjectId = @id;",
                        new { id = project.ProjectId }
                    );

                return project;
            }
        }

        private async Task<IEnumerable<BasicDataProject>> GetAllFromUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var projectMemberings = await ctx[ProjectMemberTable].Connection
                    .QueryAsync<DetailedDataProjectMember>(
                        "SELECT * FROM ITIH.vProjectMembers WHERE UserId = @id;",
                        new { id = userId }
                    );
                List<BasicDataProject> projects = new List<BasicDataProject>();
                foreach (var projectMember in projectMemberings)
                {
                    projects.Add(
                        await Get(projectMember.ProjectId)
                    );
                }

                return projects;
            }
        }
    }
}
