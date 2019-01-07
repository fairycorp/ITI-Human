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

        /// <summary>
        /// Gets all Projects from a specific User.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataProject"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAllFromUser(int userId)
        {
            var result = await GetAllFromUser(userId);
            if (result == null) return Failure(
                string.Format("No Project with userId {0} was found.", userId)
            );

            return Success(result);
        }

        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="BasicDataProject.ProjectId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedCreate(ITI.Human.ViewModels.Project.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesProjectAlreadyExist =
                    await ctx[ProjectTable].Connection
                        .QueryFirstOrDefaultAsync<int>(
                            "SELECT ProjectId FROM ITIH.tProject WHERE Name = @nm;",
                            new { nm = model.Name }
                        );
                if (doesProjectAlreadyExist > 0) return Failure("A project with this name already exists.");

            }
            var result = await Create(model);
            if (result == 0) return Failure("Error in creation process.");

            return Success(result);
        }

        /// <summary>
        /// Adds a new Project Member to a Project.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="DetailedDataProjectMember.ProjectMemberId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedAddMember(ITI.Human.ViewModels.Project.Member.CreationViewModel model)
        {
            var doesProjectExist = await Get(model.ProjectId);
            if (doesProjectExist == null) return Failure(
                string.Format("No Project with id {0} was found.", model.ProjectId)
            );

            var result = await AddMember(model);
            if (result == 0) return Failure("Error in creation process.");

            return Success(result);
        }

        /// <summary>
        /// Removes a Project Member from a Project.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result is a <see cref="bool"/> that is set to <see cref="true"/>,
        /// or Failure result if element has not been removed.</returns>
        public async Task<GuardResult> GuardedRemoveMember(DeletionViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesProjectMemberExist = await ctx[ProjectMemberTable].Connection
                    .QueryFirstOrDefaultAsync(
                        "SELECT * FROM ITIH.tProjectMember WHERE ProjectMemberId = @id;",
                        new { id = model.ProjectMemberId }
                    );
                if (doesProjectMemberExist == null) return Failure(
                    string.Format("No Project Member with id {0} was found.", model.ProjectMemberId)
                );
            }
            var result = await RemoveMember(model);
            if (result == false) return Failure("Error in deletion process.");

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
        
        private async Task<int> Create(ITI.Human.ViewModels.Project.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var projectId = await ProjectTable.Create(ctx, model.ActorId, 1, model.SemesterId, model.Name, model.Headline, model.Pitch);
                await ProjectMemberTable.Create(ctx, model.ActorId, projectId, 1, model.ActorId);
                return projectId;
            }
        }

        private async Task<int> AddMember(ITI.Human.ViewModels.Project.Member.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ProjectMemberTable.Create(ctx, model.UserId, model.ProjectId, 2, model.UserId);
            }
        }

        private async Task<bool> RemoveMember(DeletionViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ProjectMemberTable.Delete(ctx, model.ActorId, model.ProjectMemberId);
            }
        }
    }
}
