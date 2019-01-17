using API.Services.Product;
using API.Services.Project;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Product.Ordered;
using ITI.Human.ViewModels.Storage;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Storage
{
    /// <summary>
    /// Handles interactions with Storages.
    /// </summary>
    public class StorageService
    {
        public ProjectService ProjectService { get; set; }

        public ProductService ProductService { get; set; }
        
        public StorageTable StorageTable { get; set; }

        public StorageLinkedProductTable SLPTable { get; set; }

        public StorageService(ProjectService projService, ProductService prodService,
            StorageTable sTable, StorageLinkedProductTable slpTable)
        {
            ProjectService = projService;
            ProductService = prodService;
            StorageTable = sTable;
            SLPTable = slpTable;
        }

        /// <summary>
        /// Gets all Storages from database.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataStorage"/>
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await GetAll();
                if (result == null) return Failure("Not a single Storage was found.");

                return Success(result);
            }
        }

        /// <summary>
        /// Gets a Storage by its id.
        /// </summary>
        /// <param name="storageId">Storage's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataStorage"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int storageId)
        {
            var result = await Get(storageId);
            if (result == null) return Failure(
                string.Format("No Storage with id {0} was found.", storageId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets a Storage from a specific Project.
        /// </summary>
        /// <param name="projectId">Project's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataStorage"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromProject(int projectId)
        {
            var result = await GetFromProject(projectId);
            if (result == null) return Failure(
                string.Format("No Storage with projectId {0} was found.", projectId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets a Storage from an Ordered Product.
        /// </summary>
        /// <param name="orderedProduct">Ordered Product.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataStorage"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromOrderedProduct(BasicDataOrderedProduct orderedProduct)
        {
            var result = await GetFromOrderedProduct(orderedProduct);
            if (result == null) return Failure(
                string.Format("No Storage with orderedProductId {0} was found.", orderedProduct)
            );

            return Success(result);
        }

        /// <summary>
        /// Changes current Storage Stall open state.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedUpdateStall(StallUpdateViewModel model)
        {
            var doesStorageExist = await Get(model.StorageId);
            if (doesStorageExist == null) return Failure(
               string.Format("No Storage with storageId {0} was found.", model.StorageId)
            );

            var result = await UpdateStall(model);
            if (result == false) return Failure("Error in update process.");

            return Success(result);
        }

        /// <summary>
        /// Creates a new Storage.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="BasicDataStorage.StorageId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedCreate(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                // Checks if Project already exists with this specific project id.
                // If not, returns Failure().
                var doesProjectExist =
                    await ProjectService.GuardedGet(model.ProjectId);

                if (doesProjectExist.Code == Status.Failure) return Failure(doesProjectExist.Info);

                // Check if Storage already exists with this specific project id.
                // If does, returns Failure().
                var doesStorageExist =
                    await GuardedGetFromProject(model.ProjectId);

                if (doesStorageExist.Code == Status.Success) return Failure("A storage has already been created for this project.");

                // Launches creation process.
                var result = Create(model);
                if (result == null) return Failure("Error in creation process.");

                return Success(result);
            }
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataStorage>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return (await ctx[StorageTable].Connection
                    .QueryAsync<BasicDataStorage>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tStorage;"
                    )).ToArray();
            }
        }

        private async Task<BasicDataStorage> Get(int storageId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[StorageTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorage>(
                        @"SELECT
                            *
                        FROM 
                            ITIH.tStorage
                        WHERE
                            StorageId = @Id;",
                        new { Id = storageId }
                    );
            }
        }

        private async Task<BasicDataStorage> GetFromProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[StorageTable].Connection
                        .QueryFirstOrDefaultAsync<BasicDataStorage>(
                            @"SELECT
                                *
                            FROM 
                                ITIH.tStorage
                            WHERE
                                ProjectId = @Id;",
                            new { Id = projectId }
                        );
            }
        }

        private async Task<BasicDataStorage> GetFromOrderedProduct(BasicDataOrderedProduct orderedProduct)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var storageId =
                    await ctx[SLPTable].Connection
                        .QueryFirstAsync<int>(
                            "SELECT StorageId FROM ITIH.tStorageLinkedProduct WHERE StorageLinkedProductId = @id",
                            new { id = orderedProduct.StorageLinkedProductId }
                        );

                return await Get(storageId);
            }
        }

        private async Task<bool> UpdateStall(StallUpdateViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await StorageTable.UpdateStall(ctx, model.UserId, model.StorageId, model.OpenState);
            }
        }

        private async Task<int> Create(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await StorageTable.Create(ctx, model.UserId, model.ProjectId);
            }
        }
    }
}
