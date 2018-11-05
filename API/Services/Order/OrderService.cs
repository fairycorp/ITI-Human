using API.Services.Helper;
using API.Services.Product;
using API.ViewModels.Order;
using API.ViewModels.Product.Ordered;
using CK.DB.Actor;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.User;
using Stall.Guard.System;
using System;
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
        public ProductService ProductService { get; set; }

        public OrderTable OrderTable { get; set; }

        public OrderedProductTable OrderedProductTable { get; set; }

        public UserTable UserTable { get; set; }

        public OrderService(ProductService pService, OrderTable oTable, OrderedProductTable oPTable, UserTable uTable)
        {
            OrderTable = oTable;
            OrderedProductTable = oPTable;
            UserTable = uTable;
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
        /// Creates a new detailed Order.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result content is int OR Failure result in case insertion process failed.</returns>
        public async Task<GuardResult> GuardedCreateDetailedOrder(ViewModels.Order.CreationViewModel model)
        {
            // Checks if classroom id is not 0.
            if (model.ClassroomId == 0)
                return Failure("ClassroomId cannot be 0.");

            // Checks if user exists (has to) and returns failure in case he doesn't.
            var doesUserExist =
                await Attempt.ToGetElement(GetUser, model.UserId, true);

            if (doesUserExist.Code == Status.Failure)
                return Failure(doesUserExist.Info);

            // Checks if each one of the products of the list exists.
            foreach (var product in model.Products)
            {
                var productDoesExist =
                    await Attempt.ToGetElement(ProductService.GetById, product.ProductId, true);

                if (productDoesExist == null)
                    return Failure(string.Format("Product with id '{0}' does not exist in database.", product.ProductId));
            }

            // Launches the creation process.
            if (doesUserExist.Code == Status.Success)
            {
                var result = await CreateDetailedOrder(model);
                return (result > 0) ? Success(result) : Failure("Error in creation process.");
            }
            return Failure("User does not exist.");
        }

        /// <summary>
        /// Updates a detailed Order delivery state.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result content is null OR Failure result in case element does not exist in DB.</returns>
        public async Task<GuardResult> UpdateDetailedOrderDeliveryState(DeliveryStateUpdateViewModel model)
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
                            await OrderedProductTable.Update(ctx, 0, product.OrderedProductId, product.HasBeenDelivered);

                        if (!hasBeenDelivered)
                            entirelyDelivered = false;
                    }

                    if (entirelyDelivered)
                    {
                        var hasBeenEntirelyDelivered =
                            OrderTable.Update(ctx, 0, model.Info.OrderId, State.Finished, true);
                    }
                    return Success(null);
                }
                return Failure(doesExist.Info);
            }
        }

        /// <summary>
        /// Updates a detailed Order current state (NotStarted, Underway...).
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result content is boolean OR Failure result in case element does not exist in DB.</returns>
        public async Task<GuardResult> GuardedUpdateDetailedOrderCurrentState(BasicDataOrder model)
        {
            var result = await UpdateDetailedOrderCurrentState(model);
            return (result) ? Success(result) : Failure("Error in update process.");
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
                            ITIH.vOrders;"
                    );

                List<DetailedDataOrder> ordersList = new List<DetailedDataOrder>();
                foreach (var data in basicData)
                {
                    var totalPrice = 0;

                    var detailedData = new DetailedDataOrder();
                    detailedData.OrderInfo = data;
                    detailedData.Products = await ctx[OrderTable].Connection
                        .QueryAsync<BasicDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts v
                            WHERE
                                v.OrderId = @id;",
                            new { id = data.OrderId }
                        );


                    foreach (var product in detailedData.Products)
                    {
                        totalPrice += product.Price * product.Amount;
                    }
                    detailedData.OrderInfo.Total = totalPrice;

                    ordersList.Add(detailedData);
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

        private async Task<int> CreateDetailedOrder(ViewModels.Order.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var order =
                await OrderTable.Create(ctx, model.UserId, model.UserId, model.ClassroomId, DateTime.Now);

                foreach (var product in model.Products)
                {
                    var orderedProduct =
                        await OrderedProductTable.Create(ctx, model.UserId, order, product.ProductId, product.Amount);

                    // In case of an insertion problem, one have to clean the whole order up.
                    if (orderedProduct == 0)
                    {
                        var alreadyOrdered = await GetDetailedOrder(order);

                        foreach (var alreadyOrderedProduct in alreadyOrdered.Products)
                        {
                            await OrderedProductTable.Delete(ctx, 0, alreadyOrderedProduct.OrderedProductId);
                        }
                        await OrderTable.Delete(ctx, 0, order);
                        return 0;
                    }
                }
                return order;
            }
        }

        private async Task<bool> UpdateDetailedOrderCurrentState(BasicDataOrder model)
        {
            var doesExist =
                    await Attempt.ToGetElement(GetDetailedOrder, model.OrderId, true);

            if (doesExist.Code == Status.Success)
            {
                using (var ctx = new SqlStandardCallContext())
                {
                    return await OrderTable.Update(ctx, 0, model.OrderId, model.CurrentState, model.HasBeenEntirelyDelivered);
                }
            }
            return false;
        }

        private async Task<UserBasicData> GetUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserTable].Connection
                    .QueryFirstOrDefaultAsync<UserBasicData>(
                        @"SELECT
                            UserId
                        FROM
                            CK.tUser
                        WHERE
                            UserId = @id",
                        new { id = userId }
                    );
            }
        }
    }
}
