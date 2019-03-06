using API.Services.Storage;
using API.Services.User;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Fork.ViewModels.Project;
using Fork.ViewModels.Project.Member;
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
        public UserService UserService { get; set; }

        public ProjectTable ProjectTable { get; set; }

        public ProjectMemberTable ProjectMemberTable { get; set; }

        public StorageTable StorageTable { get; set; }

        public ProjectVotesTable ProjectVotesTable { get; set; }

        public ProjectService(UserService uService, ProjectTable pTable,
            ProjectMemberTable pMTable, StorageTable storageTable, ProjectVotesTable pVTable)
        {
            UserService = uService;
            ProjectTable = pTable;
            ProjectMemberTable = pMTable;
            StorageTable = storageTable;
            ProjectVotesTable = pVTable;
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
        /// Gets the Project from a ProjectMemberId.
        /// </summary>
        /// <param name="projectMemberId">Project member's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataProject"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromMember(int projectMemberId)
        {
            var result = await GetFromMember(projectMemberId);
            if (result == null) return Failure(
                string.Format("No Project from projectMemberId {0} was found.", projectMemberId)
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
        public async Task<GuardResult> GuardedCreate(Fork.ViewModels.Project.CreationViewModel model)
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

                foreach (var userId in model.Members)
                {
                    var doUsersExist = await UserService.GuardedGet(userId);
                    if (doUsersExist.Code == Status.Failure) return Failure(doUsersExist.Info);
                }
            }
            var result = await Create(model);
            if (result == 0) return Failure("Error in creation process.");

            Fork.ViewModels.Project.Member.CreationViewModel memberAddModel =
                new Fork.ViewModels.Project.Member.CreationViewModel
                {
                    ProjectId = result,
                    UserId = 0
                };
            foreach (var userId in model.Members)
            {
                memberAddModel.UserId = userId;
                await AddMember(memberAddModel);
            }

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
        public async Task<GuardResult> GuardedAddMember(Fork.ViewModels.Project.Member.CreationViewModel model)
        {
            var doesProjectExist = await Get(model.ProjectId);
            if (doesProjectExist == null) return Failure(
                string.Format("No Project with id {0} was found.", model.ProjectId)
            );

            using (var ctx = new SqlStandardCallContext())
            {
                var doesMemberAlreadyRegistered = await ctx[ProjectMemberTable].Connection
                    .QueryFirstOrDefaultAsync<int>(
                        "SELECT ProjectMemberId FROM ITIH.tProjectMember WHERE UserId = @uId AND ProjectId = @pId;",
                        new { uId = model.UserId, pId = model.ProjectId }
                    );
                if (doesMemberAlreadyRegistered > 0) return Failure(
                    string.Format("Project Member with userId {0} is already registered in project.", model.UserId)
                );
            }

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
                if (projects == null) return null;

                foreach (var project in projects)
                {
                    project.Members = await ctx[ProjectMemberTable].Connection
                        .QueryAsync<DetailedDataProjectMember>(
                            "SELECT * FROM ITIH.vProjectMembers WHERE ProjectId = @id;",
                            new { id = project.ProjectId }
                        );

                    project.Votes = await ctx[ProjectVotesTable].Connection
                        .QueryAsync<int>(
                            "SELECT Note FROM ITIH.tProjectVotes WHERE ProjectId = @id;",
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
                if (project == null) return null;

                project.Members = await ctx[ProjectMemberTable].Connection
                    .QueryAsync<DetailedDataProjectMember>(
                        "SELECT * FROM ITIH.vProjectMembers WHERE ProjectId = @id;",
                        new { id = project.ProjectId }
                    );

                project.Votes = await ctx[ProjectVotesTable].Connection
                    .QueryAsync<int>(
                        "SELECT Note FROM ITIH.tProjectVotes WHERE ProjectId = @id;",
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

        private async Task<BasicDataProject> GetFromMember(int projectMemberId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var projectId = await ctx[ProjectMemberTable].Connection
                    .QueryFirstOrDefaultAsync<int>(
                        "SELECT ProjectId FROM ITIH.vProjectMembers WHERE ProjectMemberId = @id;",
                        new { id = projectMemberId }
                    );
                return await Get(projectId);
            }
        }
        
        private async Task<int> Create(Fork.ViewModels.Project.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var projectId = await ProjectTable.Create(ctx, model.ActorId, 1, model.SemesterId, model.Name, model.Headline, model.Pitch);
                await ProjectMemberTable.Create(ctx, model.ActorId, projectId, 1, model.ActorId);

                if (model.SemesterId == 4)
                {
                    Fork.ViewModels.Storage.CreationViewModel storageModel = new Fork.ViewModels.Storage.CreationViewModel
                    {
                        UserId = model.ActorId,
                        ProjectId = projectId
                    };
                    await StorageTable.Create(ctx, storageModel.UserId, storageModel.ProjectId);
                }

                return projectId;
            }
        }

        private async Task<int> AddMember(Fork.ViewModels.Project.Member.CreationViewModel model)
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
