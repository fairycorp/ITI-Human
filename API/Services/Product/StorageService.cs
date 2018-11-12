using API.Services.Helper;
using API.Services.Project;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Storage;
using ITI.Human.ViewModels.Storage.LinkedProduct;
using Stall.Guard.System;
using System;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Product
{
    public class StorageService
    {
        public ProjectService ProjectService { get; set; }

        public ProductService ProductService { get; set; }

        public StorageTable StorageTable { get; set; }

        public StorageLinkedProductTable StorageLinkedProductTable { get; set; }

        public StorageService(ProjectService projService, ProductService prodService, StorageTable sTable, StorageLinkedProductTable slpTable)
        {
            ProjectService = projService;
            ProductService = prodService;
            StorageTable = sTable;
            StorageLinkedProductTable = slpTable;
        }

        /// <summary>
        /// Gets all Storages from database.
        /// </summary>
        /// <returns>Success result where result content is a BasicDataStorage Collection.</returns>
        public async Task<GuardResult> GetAllStorages()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[StorageTable].Connection
                    .QueryAsync<BasicDataStorage>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tStorage;"
                    );
                return Success(result.ToArray());
            }
        }

        /// <summary>
        /// Gets a Storage by its id.
        /// </summary>
        /// <param name="storageId">Storage id.</param>
        public async Task<GuardResult> GetStorage(int storageId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[StorageTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorage>(
                        @"SELECT
                            *
                        FROM 
                            ITIH.tStorage
                        WHERE
                            StorageId = @Id;",
                        new { Id = storageId }
                    );
                return Success(result);
            }
        }

        /// <summary>
        /// Gets a Storage by its matching Project id.
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <returns></returns>
        public async Task<GuardResult> GetStorageFromProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[StorageTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorage>(
                        @"SELECT
                            *
                        FROM 
                            ITIH.tStorage
                        WHERE
                            ProjectId = @Id;",
                        new { Id = projectId }
                    );
                return Success(result);
            }
        }

        /// <summary>
        /// Creates a new Storage.
        /// </summary>
        /// <param name="projectId">Storage Project id.</param>
        public async Task<GuardResult> CreateStorage(ITI.Human.ViewModels.Storage.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                // Checks if Project already exists with this specific project id.
                // If not, returns Failure().
                var doesProjectExist =
                    await Attempt.ToGetElement(ProjectService.GetProject, model.ProjectId, true);

                if (doesProjectExist.Content == null) return Failure(doesProjectExist.Info);

                // Check if Storage already exists with this specific project id.
                // If does, returns Failure().
                var doesStorageExist =
                    await Attempt.ToGetElement(GetStorageFromProject, model.ProjectId, false);

                if (doesStorageExist.Content != null) return Failure(doesStorageExist.Info);

                // Launches creation process.
                return Success(await StorageTable.Create(ctx, 0, model.ProjectId));
            }
        }

        /// <summary>
        /// Gets all Storage Linked Products from database.
        /// </summary>
        public async Task<GuardResult> GetAllStorageLinkedProducts()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[StorageLinkedProductTable].Connection
                    .QueryAsync<BasicDataStorageLinkedProduct>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tStorageLinkedProduct;"
                    );
                return Success(result.ToArray());
            }
        }

        /// <summary>
        /// Gets a Storage Linked Product by its id.
        /// </summary>
        /// <param name="storageLinkedProductId">Storage Linked Product id.</param>
        public async Task<GuardResult> GetStorageLinkedProduct(int storageLinkedProductId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[StorageLinkedProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorageLinkedProduct>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tStorageLinkedProduct
                        WHERE
                            StorageLinkedProductId = @Id;",
                        new { Id = storageLinkedProductId }
                    );
                return Success(result);
            }
        }

        /// <summary>
        /// Gets a Storage Linked Product from a specific Storage.
        /// </summary>
        /// <param name="storageLinkedProductId">Storage Linked Product id.</param>
        /// <param name="storageId">Specific Storage id.</param>
        public async Task<GuardResult> GetStorageLinkedProductFromStorage(int storageLinkedProductId, int storageId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[StorageLinkedProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorageLinkedProduct>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tStorageLinkedProduct
                        WHERE
                            StorageLinkedProductId = @FirstId
                            AND
                            StorageId = @SecondId;",
                        new { FirstId = storageLinkedProductId, SecondId = storageId }
                    );
                return Success(result);
            }
        }

        /// <summary>
        /// Creates a Storage Linked Product.
        /// </summary>
        /// <param name="model">Storage Linked Product creation view model.</param>
        public async Task<GuardResult> CreateLinkedProduct(ITI.Human.ViewModels.Storage.LinkedProduct.CreationViewModel model)
        {
            // Checks if both mentioned Storage & Product exist.
            // If not, returns Failure().
            var doesStorageExist =
                await Attempt.ToGetElement(GetStorage, model.StorageId, true);

            var doesProductExist =
                await Attempt.ToGetElement(ProductService.GetProductById, model.ProductId, true);

            // Since Info properties are the same, doesn't matter which one one display.
            if (((GuardResult)doesStorageExist.Content).Content == null 
            || ((GuardResult)doesProductExist.Content).Content == null)
                return Failure(doesProductExist.Info);

            // Launches creation process.
            using (var ctx = new SqlStandardCallContext())
            {
                return Success(await StorageLinkedProductTable.Create(ctx, 0, model.Stock, model.ProductId, model.UnitPrice, model.Stock));
            }
        }

        /// <summary>
        /// Updates a Storage Linked Product.
        /// </summary>
        /// <param name="model">Storage Linked Product update view model.</param>
        public async Task<GuardResult> UpdateLinkedProduct(UpdateViewModel model)
        {
            // Checks if Storage Linked Product exists.
            // If not, returns Failure().
            var doesSLPExist =
                await Attempt.ToGetElement(GetStorageLinkedProduct, model.StorageLinkedProductId, true);

            if (((GuardResult)doesSLPExist.Content).Content == null) return Failure(doesSLPExist.Info);

            // Launches update process.
            using (var ctx = new SqlStandardCallContext())
            {
                var now = DateTime.UtcNow;

                var result1 = 
                    await StorageLinkedProductTable.UpdateUnitPrice(ctx, 0, now, model.StorageLinkedProductId, model.UnitPrice);

                var result2 =
                    await StorageLinkedProductTable.UpdateStock(ctx, 0, now, model.StorageLinkedProductId, model.Stock);

                return Success(new bool[2] { result1, result2 });
            }
        }
    }
}
