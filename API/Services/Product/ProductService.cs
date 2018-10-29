using API.ViewModels.Product;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using Stall.Guard.System;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Product
{
    /// <summary>
    /// Handles any database interaction concerning Products.
    /// </summary>
    public class ProductService
    {
        public ProductTable ProductTable { get; }

        public ProductService(ProductTable table)
        {
            ProductTable = table;
        }

        /// <summary>
        /// Gets all Products from database.
        /// </summary>
        /// <returns>Success result where result content is a BasicDataProduct Collection.</returns>
        public async Task<GuardResult> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var result = await ctx[ProductTable].Connection
                    .QueryAsync<BasicDataProduct>(
                        @"SELECT
                            *
                        FROM ITIH.tProduct;"
                    );
                return Success(result.ToArray());
            }
        }

        /// <summary>
        /// Gets a particular Product by its id.
        /// </summary>
        /// <param name="productId">Product id.</param>
        /// <returns>Success result where result content is a single BasicDataProduct.</returns>
        public async Task<GuardResult> GetById(int productId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return Success(
                    await ctx[ProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProduct>(
                        @"SELECT
                            *
                        FROM ITIH.tProduct
                        WHERE ProductId = @id;",
                        new { id = productId }
                    )
                );
            }
        }

        /// <summary>
        /// Gets a particular Product by its name.
        /// </summary>
        /// <param name="productName">Product name.</param>
        /// <returns>Success result where result content is a single BasicDataProduct.</returns>
        public async Task<GuardResult> GetByName(string productName)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return Success(
                    await ctx[ProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProduct>(
                        @"SELECT
                            *
                        FROM ITIH.tProduct
                        WHERE [Name] = @nM;",
                        new { nM = productName }
                    )
                );
            }
        }
    }
}
