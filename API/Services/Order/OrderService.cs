using API.Models.Order;
using API.Models.Product.Ordered;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.ResultFactory;

namespace API.Services.Order
{
    /// <summary>
    /// Handles any database interaction concerning Orders.
    /// </summary>
    public class OrderService
    {
        public OrderTable OrderTable { get; set; }

        public OrderService(OrderTable table)
        {
            OrderTable = table;
        }

        /// <summary>
        /// Gets user's all detailed orders. 
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>Success result where result content is a list of DetailedDataOrder.</returns>
        public async Task<GuardResult> GetDetailedOrdersOfUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var basicData = await ctx[OrderTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            tOrder
                        WHERE
                            UserId = @id",
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
                                        v.OrderedProducts
                                    WHERE
                                        OrderId = @id",
                                    new { id = data.OrderId }
                                )
                        }
                    );
                }

                return Success(ordersByUser);
            }
        }

        /// <summary>
        /// Gets a detailed order by its id.
        /// </summary>
        /// <param name="orderId">Product id.</param>
        /// <returns>Success result where result content is a single DetailedDataOrder.</returns>
        public async Task<GuardResult> GetDetailedOrder(int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var basicData = await ctx[OrderTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            tOrder
                        WHERE
                            OrderId = @id",
                        new { id = orderId }
                    );

                var detailedData = new DetailedDataOrder {
                    OrderInfo = basicData,
                    Products = await ctx[OrderTable].Connection
                        .QueryAsync<BasicDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                v.OrderedProducts
                            WHERE
                                OrderId = @id",
                            new { id = orderId }
                        )
                };

                return Success(detailedData);
            }
        }
    }
}
