using API.Services.Product;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Storage.LinkedProduct;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Storage
{
    /// <summary>
    /// Handles interactions with Storage Linked Products.
    /// </summary>
    public class SLPService
    {
        public ProductService ProductService { get; set; }
        
        public StorageService StorageService { get; set; }

        public StorageLinkedProductTable SLPTable { get; set; }

        public SLPService(ProductService pService, StorageService sService,
            StorageLinkedProductTable slpTable)
        {
            ProductService = pService;
            StorageService = sService;
            SLPTable = slpTable;
        }

        /// <summary>
        /// Gets all Storage Linked Products.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataStorageSLP"/> 
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            var result = await GetAll();
            if (result == null) return Failure("Not a single StorageLinkedProduct was found.");

            return Success(result);
        }

        /// <summary>
        /// Gets a SLP by its id.
        /// </summary>
        /// <param name="slpId">SLP's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataStorageSLP"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int slpId)
        {
            var result = await Get(slpId);
            if (result == null) return Failure(
                string.Format("No StorageLinkedProduct with id {0} was found", slpId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets all SLP from a specific Storage.
        /// </summary>
        /// <param name="storageId">Storage's id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataStorageSLP"/> 
        /// or Failure result if no element exists in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAllFromStorage(int storageId)
        {
            var result = await GetAllFromStorage(storageId);
            if (result == null) return Failure(
                string.Format("No StorageLinkedPorduct was found in Storage with id {0}", storageId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets a SLP by its Storage id.
        /// </summary>
        /// <param name="slpId">SLP's id.</param>
        /// <param name="storageId">Storage's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataStorageSLP"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromStorage(int slpId, int storageId)
        {
            var result = await GetFromStorage(slpId, storageId);
            if (result == null) return Failure(
                string.Format("No StorageLinkedProduct with id {0} and from Storage with id {1} was found.",
                slpId, storageId)
            );

            return Success(result);
        }

        /// <summary>
        /// Creates a new Storage Linked Product.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="BasicDataStorageSLP.StorageLinkedProductId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedCreate(CreationViewModel model)
        {
            // Checks if both mentioned Storage & Product exist.
            // If not, returns Failure().
            var doesStorageExist =
                await StorageService.GuardedGet(model.StorageId);

            var doesProductExist =
                await ProductService.GuardedGet(model.ProductId);

            // Since Info properties are the same, doesn't matter which one one display.
            if (doesStorageExist.Content == null || doesProductExist.Content == null)
                return Failure("Either Storage/Product doesn't exist in database.");

            // Checks if a SLP already exists with that specific Product id in that specific Storage.
            var slpList =
                await GetAllFromStorage(model.StorageId);

            foreach (var product in slpList)
            {
                if (product.ProductId == model.ProductId)
                    return Failure(string.Format("Product with id {0} already exists in the Storage with id {1}.",
                        model.ProductId, model.StorageId));
            }

            var result = await Create(model);
            if (result == 0) return Failure("Error in creation process.");

            return Success(result);
        }

        /// <summary>
        /// Updates a SLP.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="bool[]"/> that gathers all update states,
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedUpdate(UpdateViewModel model)
        {
            // Checks if Storage Linked Product exists.
            // If not, returns Failure().
            var doesSLPExist =
                await Get(model.StorageLinkedProductId);

            if (doesSLPExist == null) return Failure(
                string.Format("No StorageLinkedProduct with id {0} was found.", model.StorageLinkedProductId)
            );

            return Success(Update(model));
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataStorageSLP>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return (await ctx[SLPTable].Connection
                    .QueryAsync<BasicDataStorageSLP>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vStorageLinkedProducts;"
                    )).ToArray();
            }
        }

        private async Task<BasicDataStorageSLP> Get(int slpId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[SLPTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorageSLP>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vStorageLinkedProducts
                        WHERE
                            StorageLinkedProductId = @id;",
                        new { id = slpId }
                    );
            }
        }

        private async Task<IEnumerable<BasicDataStorageSLP>> GetAllFromStorage(int storageId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[SLPTable].Connection
                    .QueryAsync<BasicDataStorageSLP>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vStorageLinkedProducts
                        WHERE
                            StorageId = @Id;",
                        new { Id = storageId }
                    );
            }
        }

        private async Task<BasicDataStorageSLP> GetFromStorage(int slpId, int storageId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[SLPTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataStorageSLP>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vStorageLinkedProducts
                        WHERE
                            StorageLinkedProductId = @firstId
                            AND
                            StorageId = @secondId;",
                        new { firstId = slpId, secondId = storageId }
                    );
            }
        }

        private async Task<int> Create(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await SLPTable.Create(ctx, model.UserId, model.StorageId, model.ProductId, model.UnitPrice, model.Stock);
            }
        }

        private async Task<bool[]> Update(UpdateViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var now = DateTime.UtcNow;

                var result1 =
                    await SLPTable.UpdateUnitPrice(ctx, model.UserId, now, model.StorageLinkedProductId, model.UnitPrice);

                var result2 =
                    await SLPTable.UpdateStock(ctx, model.UserId, now, model.StorageLinkedProductId, model.Stock);

                return new bool[2] { result1, result2 };
            }
        }
    }
}
