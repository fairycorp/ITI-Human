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
        public async Task<GuardResult> GetProductById(int productId)
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
        public async Task<GuardResult> GetProductByName(string productName)
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

        /// <summary>
        /// Creates a new Product.
        /// </summary>
        /// <param name="model">Product creation view model.</param>
        /// <returns>Success result where result content is null OR Failure result in case element does not exist in DB.</returns>
        public async Task<GuardResult> CreateProduct(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return Success(
                    await ProductTable.Create(ctx, 0, model.Name, model.Desc)
                );
            }
        }
    }
}
