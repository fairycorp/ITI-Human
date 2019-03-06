using Fork.ViewModels.Product;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Product
{
    /// <summary>
    /// Handles interactions with Products.
    /// </summary>
    public class ProductService
    {
        public ProductTable ProductTable { get; }

        public ProductService(ProductTable table)
        {
            ProductTable = table;
        }

        /// <summary>
        /// Gets all Products.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataProduct"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            var result = await GetAll();
            if (result == null) return Failure("Not a single Product was found.");

            return Success(result);
        }

        /// <summary>
        /// Gets a Product by its id.
        /// </summary>
        /// <param name="productId">Product's id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataProduct"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int productId)
        {
            var result = await GetById(productId);

            if (result == null) return Failure(
                string.Format("No Product with id {0} was found.", productId)
            );
            return Success(result);
        }

        /// <summary>
        /// Gets a Product by its name.
        /// </summary>
        /// <param name="productName">Product's name.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataProduct"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetByName(string productName)
        {
            var result = await GetByName(productName);

            if (result == null) return Failure(
                string.Format("No Product with name {0} was found.", productName)
            );
            return Success(result);
        }

        /// <summary>
        /// Creates a new Product.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="BasicDataProduct.ProductId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedCreate(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                // Checks if a Product already exists with this specific Product name.
                // If does, returns Failure().
                var doesProductExist = await GuardedGetByName(model.Name);
                if (doesProductExist.Content != null) return Failure(doesProductExist.Info);

                var result = await Create(model);
                if (result == 0) return Failure("Error in creation process.");

                return Success(result);
            }
        }

        /// <summary>
        /// Updates a Product.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="bool"/> that represents update state,
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedUpdate(UpdateViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                // Checks if a Product already exsists with this specific Product name.
                // If not, returns Failure().
                var doesProductExist = await GuardedGetByName(model.Name);

                if (doesProductExist.Content == null) return Failure(doesProductExist.Info);

                // Retrieves current product to fulfill model missing properties.
                var currentProduct = doesProductExist.Content;
                model.Name = model.Name ?? ((BasicDataProduct)currentProduct).Name;
                model.Desc = model.Desc ?? ((BasicDataProduct)currentProduct).Desc;

                // Launches update process.
                var result = await Update(model);
                if (result == false) return Failure("No update was proceeded.");

                return Success(result);
            }
        }

        // --------------------------------------------------------------------------------------------

        private async Task<IEnumerable<BasicDataProduct>> GetAll()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return (await ctx[ProductTable].Connection
                    .QueryAsync<BasicDataProduct>(
                        @"SELECT
                            *
                        FROM ITIH.tProduct;"
                    )).ToArray();
            }
        }

        private async Task<BasicDataProduct> GetById(int productId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[ProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProduct>(
                        @"SELECT
                            *
                        FROM ITIH.tProduct
                        WHERE ProductId = @id;",
                        new { id = productId }
                    );
            }
        }

        private async Task<BasicDataProduct> GetByName(string productName)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[ProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataProduct>(
                        @"SELECT
                            *
                        FROM ITIH.tProduct
                        WHERE [Name] = @nM;",
                        new { nM = productName }
                );
            }
        }

        private async Task<int> Create(CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ProductTable.Create(ctx, 0, model.Name, model.Desc, model.Url);
            }
        }

        private async Task<bool> Update(UpdateViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ProductTable.Update(ctx, 0, model.ProductId, model.Name, model.Desc, model.Url);
            }
        }
    }
}
