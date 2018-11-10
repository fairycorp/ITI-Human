using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Services.Product
{
    public class StorageService
    {
        public StorageTable StorageTable { get; set; }

        public StorageLinkedProductTable StorageLinkedProductTable { get; set; }

        public StorageService(StorageTable sTable, StorageLinkedProductTable slpTable)
        {
            StorageTable = sTable;
            StorageLinkedProductTable = slpTable;
        }

        /// <summary>
        /// Gets all Storages from database.
        /// </summary>
        /// <returns>Success result where result content is a BasicDataStorage Collection.</returns>
        public async Task<IEnumerable<BasicDataStorage>> GetAllStorages()
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
                return result.ToArray();
            }
        }

        /// <summary>
        /// Gets a Storage by its id.
        /// </summary>
        /// <param name="storageId">Storage id.</param>
        public async Task<BasicDataStorage> GetStorage(int storageId)
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
                return result;
            }
        }

        /// <summary>
        /// Gets all Storage Linked Products from database.
        /// </summary>
        public async Task<IEnumerable<BasicDataStorageLinkedProduct>> GetAllStorageLinkedProducts()
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
                return result.ToArray();
            }
        }

        /// <summary>
        /// Gets a Storage Linked Product by its id.
        /// </summary>
        /// <param name="storageLinkedProductId">Storage Linked Product id.</param>
        public async Task<BasicDataStorageLinkedProduct> GetStorageLinkedProduct(int storageLinkedProductId)
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
                return result;
            }
        }

        /// <summary>
        /// Gets a Storage Linked Product from a specific Storage.
        /// </summary>
        /// <param name="storageLinkedProductId">Storage Linked Product id.</param>
        /// <param name="storageId">Specific Storage id.</param>
        public async Task<BasicDataStorageLinkedProduct> GetStorageLinkedProductFromStorage(int storageLinkedProductId, int storageId)
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
                return result;
            }
        }
    }
}
