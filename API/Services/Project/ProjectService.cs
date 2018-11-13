﻿using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Project;
using Stall.Guard.System;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Project
{
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
        public async Task<GuardResult> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[ProjectTable].Connection
                    .QueryAsync<BasicDataProject>(
                         @"SELECT
                            *
                        FROM 
                            ITIH.tProject;"
                    );
                if (result == null) return Failure("No element was found.");
                return Success(result.ToArray());
            }
        }

        /// <summary>
        /// Gets a project by its id.
        /// </summary>
        /// <param name="projectId">Project id.</param>
        public async Task<GuardResult> GetProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[ProjectTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProject>(
                        @"SELECT
                            *
                        FROM 
                            ITIH.tProject
                        WHERE
                            ProjectId = @Id;",
                        new { Id = projectId }
                    );
                if (result == null) return Failure("Element does not exist in database.");
                return Success(result);
            }
        }
    }
}