using API.Services.Helper;
using API.ViewModels.Order;
using API.ViewModels.Product.Ordered;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Order
{
    /// <summary>
    /// Handles any database interaction concerning Orders.
    /// </summary>
    public class OrderService
    {
        public OrderTable OrderTable { get; set; }

        public OrderedProductTable OrderedProductTable { get; set; }

        public OrderService(OrderTable oTable, OrderedProductTable oPTable)
        {
            OrderTable = oTable;
            OrderedProductTable = oPTable;
        }

        /// <summary>
        /// Gets all detailed Orders.
        /// </summary>
        /// <returns>Success result where result content is a list of DetailedDataOrder.</returns>
        public async Task<GuardResult> GuardedGetAllDetailedOrders()
            => Success(await GetAllDetailedOrders());

        /// <summary>
        /// Gets user's all detailed Orders. 
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>Success result where result content is a list of DetailedDataOrder.</returns>
        public async Task<GuardResult> GuardedGetDetailedOrdersFromUser(int userId)
            => Success(await GetDetailedOrdersFromUser(userId));

        /// <summary>
        /// Gets a detailed Order by its id.
        /// </summary>
        /// <param name="orderId">Product id.</param>
        /// <returns>Success result where result content is a single DetailedDataOrder.</returns>
        public async Task<GuardResult> GuardedGetDetailedOrder(int orderId)
            => Success(await GetDetailedOrder(orderId));

        /// <summary>
        /// Updates a detailed Order.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result content is null OR Failure result in case element does not exist in DB.</returns>
        public async Task<GuardResult> UpdateDetailedOrder(ViewModels.Order.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesExist = 
                    await Attempt.ToGetElement(GetDetailedOrder, model.Info.OrderId, true);

                if (doesExist.Code == Status.Success)
                {
                    var entirelyDelivered = true;

                    foreach (var product in model.Products)
                    {
                        // TODO ASAP: Change ActorId to use the current UserId instead of 0
                        var hasBeenDelivered =
                            OrderedProductTable.Update(ctx, 0, product.OrderedProductId, product.HasBeenDelivered);

                        if (!hasBeenDelivered)
                            entirelyDelivered = false;
                    }

                    if (entirelyDelivered)
                    {
                        var hasBeenEntirelyDelivered =
                            OrderTable.Update(ctx, 0, model.Info.OrderId, true);
                    }
                    return Success(null);
                }
                return Failure(doesExist.Info);
            }
        }

        // --------------------------------------------------------------------------------------------

        private async Task<List<DetailedDataOrder>> GetAllDetailedOrders()
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var basicData = await ctx[OrderTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrder;"
                    );

                List<DetailedDataOrder> ordersList = new List<DetailedDataOrder>();
                foreach (var data in basicData)
                {
                    ordersList.Add(
                        new DetailedDataOrder
                        {
                            OrderInfo = data,
                            Products = await ctx[OrderTable].Connection
                                .QueryAsync<BasicDataOrderedProduct>(
                                    @"SELECT
                                        *
                                    FROM
                                        ITIH.vOrderedProducts v
                                    WHERE
                                        v.OrderId = @id;",
                                    new { id = data.OrderId }
                                )
                        }
                    );
                }
                return ordersList;
            }
        }

        private async Task<List<DetailedDataOrder>> GetDetailedOrdersFromUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var basicData = await ctx[OrderTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrder
                        WHERE
                            UserId = @id;",
                        new { id = userId }
                    );

                List<DetailedDataOrder> ordersByUser = new List<DetailedDataOrder>();
                foreach (var data in basicData)
                {
                    ordersByUser.Add(new DetailedDataOrder
                    {
                        OrderInfo = data,
                        Products = await ctx[OrderTable].Connection
                                .QueryAsync<BasicDataOrderedProduct>(
                                    @"SELECT
                                        *
                                    FROM
                                        ITIH.vOrderedProducts
                                    WHERE
                                        OrderId = @id;",
                                    new { id = data.OrderId }
                                )
                    }
                    );
                }
                return ordersByUser;
            }
        }

        private async Task<DetailedDataOrder> GetDetailedOrder(int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var basicData = await ctx[OrderTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrder
                        WHERE
                            OrderId = @id;",
                        new { id = orderId }
                    );

                var detailedData = new DetailedDataOrder
                {
                    OrderInfo = basicData,
                    Products = await ctx[OrderTable].Connection
                        .QueryAsync<BasicDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts
                            WHERE
                                OrderId = @id;",
                            new { id = orderId }
                        )
                };
                return detailedData;
            }
        }
    }
}
