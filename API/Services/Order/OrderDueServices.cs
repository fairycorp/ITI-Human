using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Order.Payment;
using ITI.Human.ViewModels.Product.Ordered;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Order
{
    /// <summary>
    /// Handles interactions with Order Final Dues, Payments and Credits.
    /// </summary>
    public class OrderDueServices
    {
        public OrderFinalDueTable OrderFinalDueTable { get; set; }

        public OrderPaymentTable OrderPaymentTable { get; set; }

        public OrderCreditTable OrderCreditTable { get; set; }

        public OrderDueServices(OrderFinalDueTable oFDTable,
            OrderPaymentTable oPTable, OrderCreditTable oCTable)
        {
            OrderFinalDueTable = oFDTable;
            OrderPaymentTable = oPTable;
            OrderCreditTable = oCTable;
        }

        /// <summary>
        /// Gets a detailed Order Final Due.
        /// </summary>
        /// <param name="orderId">Order's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="DetailedDataOrderFinalDue"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFinalDueFromOrder(int orderId)
        {
            var result = await GetFinalDueFromOrder(orderId);

            if (result == null) return Failure(
                string.Format("No Due with orderId {0} was found.", orderId)
            );
            return Success(result);
        }

        /// <summary>
        /// Creates a new Order Final Due.
        /// </summary>
        /// <param name="order">Matching order.</param>
        /// <returns>
        /// <returns>
        /// Success result where result content is a <see cref="BasicDataOrderFinalDue.OrderFinalDueId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedCreateFinalDue(DetailedDataOrder order)
        {
            var result = await CreateFinalDue(order);
            if (result == 0) return Failure("Error in creation process.");

            return Success(result);
        }

        /// <summary>
        /// Gets all Order Payments from a specific Order.
        /// </summary>
        /// <param name="orderId">Order's id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataOrderPayment"/>
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAllPaymentsFromOrder(int orderId)
        {
            var result = await GetAllPaymentsFromOrder(orderId);
            if (result == null) return Failure(
                string.Format("No OrderPayment with orderId {0} was found", orderId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets an Order Payment by its id.
        /// </summary>
        /// <param name="orderPaymentId">Order Payment's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataOrderPayment"/>
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetOrderPayment(int orderPaymentId)
        {
            var result = await GetOrderPayment(orderPaymentId);
            if (result == null) return Failure(
                string.Format("No OrderPayment with id {0} was found.", orderPaymentId)
            );

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<DetailedDataOrderFinalDue> GetFinalDueFromOrder(int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var info = await ctx[OrderFinalDueTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderFinalDue>(
                        "SELECT * FROM ITIH.tOrderFinalDue WHERE OrderId = @id",
                        new { id = orderId }
                    );

                var payments = await ctx[OrderPaymentTable].Connection
                    .QueryAsync<BasicDataOrderPayment>(
                        "SELECT * FROM ITIH.tOrderPayment WHERE OrderFinalDueId = @id",
                        new { id = info.OrderFinalDueId }
                    );

                return new DetailedDataOrderFinalDue
                {
                    Info = info,
                    Payments = payments
                };
            }
        }

        private async Task<int> CreateFinalDue(DetailedDataOrder order)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderFinalDueTable.Create(ctx, 0, order.Info.OrderId, CalculateOrderTotal(order.Products), 0);
            }
        }

        private async Task<IEnumerable<BasicDataOrderPayment>> GetAllPaymentsFromOrder(int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderPaymentTable].Connection
                    .QueryAsync<BasicDataOrderPayment>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrderPayment
                        WHERE
                            OrderId = @id",
                        new { id = orderId }
                    );
            }
        }

        private async Task<BasicDataOrderPayment> GetOrderPayment(int orderPaymentId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderPaymentTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderPayment>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrderPayment
                        WHERE
                            OrderPaymentId = @id",
                        new { id = orderPaymentId }
                    );
            }
        }

        private static int CalculateOrderTotal(IEnumerable<BasicDataOrderedProduct> products)
        {
            int total = 0;
            foreach (var product in products)
            {
                total += product.UnitPrice;
            }
            return total;
        }
    }
}
