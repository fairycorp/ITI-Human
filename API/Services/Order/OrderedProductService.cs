using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Order.Payment;
using ITI.Human.ViewModels.Product.Ordered;
using Stall.Guard.System;
using System;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Order
{
    /// <summary>
    /// Handles interactions with Ordered Products.
    /// </summary>
    public class OrderedProductService
    {
        public OrderDueServices OrderDueServices { get; set; }

        public OrderedProductTable OrderedProductTable { get; set; }

        public OrderedProductService(OrderDueServices oDServices,
            OrderedProductTable oPTable)
        {
            OrderDueServices = oDServices;
            OrderedProductTable = oPTable;
        }

        /// <summary>
        /// Gets an Ordered Product by its id.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataOrderedProduct"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int orderedProductId)
        {
            var result = await Get(orderedProductId);

            if (result == null) return Failure(
                string.Format("No OrderedProduct with id {0} was found.", orderedProductId)
            );
            return Success(result);
        }

        /// <summary>
        /// Gets an Ordered Product from a specific Order.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product's id.</param>
        /// <param name="orderId">Order's id.w/param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataOrderedProduct"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromOrder(int orderedProductId, int orderId)
        {
            var result = await GetFromOrder(orderedProductId, orderId);
            if (result == null) return Failure(
                string.Format("No OrderedProduct with id {0} and with OrderId {1} was found.",
                orderedProductId, orderId)
            );

            return Success(result);
        }

        /// <summary>
        /// Updates an Ordered Product Current State.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product's id.</param>
        /// <param name="currentState">Ordered Product's new wanted current state.</param>
        /// <returns>
        /// Success result where result content is a <see cref="bool"/> that indicates if an update was made
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedUpdateCurrentState(int orderedProductId, State currentState)
        {
            // Checks if Ordered Product exists.
            // If not, returns Failure().
            var doesOPExist =
                await Get(orderedProductId);

            if (doesOPExist == null) return Failure(
                string.Format("No Ordered Product with id {0} was found.", orderedProductId)
            );

            var result = await UpdateCurrentState(orderedProductId, currentState);
            if (!result) return Failure("No update was proceeded.");

            return Success(result);
        }

        /// <summary>
        /// Updates an Ordered Product Payment State.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product's id.</param>
        /// <param name="paymentState">Ordered Product's new wanted payment state.</param>
        /// <returns>
        /// Success result where result content is a <see cref="bool"/> that indicates if an update was made
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedUpdatePaymentState(int orderedProductId, Payment paymentState, int amount)
        {
            // Checks if Ordered Product exists.
            // If not, returns Failure().
            var doesOPExist =
                await Get(orderedProductId);

            if (doesOPExist == null) return Failure(
                string.Format("No Ordered Product with id {0} was found.", orderedProductId)
            );

            var result = await UpdatePaymentState(orderedProductId, paymentState, amount);
            if (!result) return Failure("No update was proceeded.");

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<BasicDataOrderedProduct> Get(int orderedProductId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderedProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderedProduct>(
                        "SELECT * FROM ITIH.vOrderedProducts WHERE OrderedProductId = @id;",
                        new { id = orderedProductId }
                    );
            }
        }

        private async Task<BasicDataOrderedProduct> GetFromOrder(int orderedProductId, int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderedProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderedProduct>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrderedProduct
                        WHERE
                            OrderedProductId = @firstId
                            AND
                            OrderId = @secondId",
                        new { firstId = orderedProductId, secondId = orderId }
                    );
            }
        }

        private async Task<bool> UpdateCurrentState(int orderedProductId, State currentState)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderedProductTable.UpdateCurrentState(ctx, 0, DateTime.UtcNow, orderedProductId, currentState);
            }
        }

        private async Task<bool> UpdatePaymentState(int orderedProductId, Payment paymentState, int amount)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var orderedProduct = await Get(orderedProductId);
                var orderFinalDue = await OrderDueServices.GuardedGetFinalDueFromOrder(orderedProduct.OrderId);

                return await OrderedProductTable.UpdatePaymentState(ctx, 0, DateTime.UtcNow, orderedProductId,
                    ((DetailedDataOrderFinalDue)orderFinalDue.Content).Info.OrderFinalDueId, paymentState, amount);
            }
        }
    }
}
