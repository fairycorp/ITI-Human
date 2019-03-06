using API.Services.Classroom;
using API.Services.Storage;
using API.Services.User;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Fork.ViewModels.Order;
using Fork.ViewModels.Product.Ordered;
using Fork.ViewModels.Storage;
using Fork.ViewModels.Storage.LinkedProduct;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Order
{
    /// <summary>
    /// Handles interactions with Orders.
    /// </summary>
    public class OrderService
    {
        public StorageService StorageService { get; set; }

        public SLPService SLPService { get; set; }

        public OrderDueServices OrderDueServices { get; set; }

        public ClassroomService ClassroomService { get; set; }

        public UserService UserService { get; set; }

        public OrderTable OrderTable { get; set; }

        private OrderedProductTable OrderedProductTable { get; set; }

        private OrderPaymentTable OrderPaymentTable { get; set; }

        public OrderService(StorageService sService, SLPService slpService,
            OrderDueServices oDServices, ClassroomService cService, 
            UserService uService, OrderTable oTable, OrderedProductTable oPTable,
            OrderPaymentTable oPayTable)
        {
            StorageService = sService;
            SLPService = slpService;
            OrderDueServices = oDServices;
            ClassroomService = cService;
            UserService = uService;
            OrderTable = oTable;
            OrderedProductTable = oPTable;
            OrderPaymentTable = oPayTable;
        }

        /// <summary>
        /// Gets all detailed Orders.
        /// </summary>
        /// <returns>
        /// Success result where result content is a list of <see cref="DetailedDataOrder"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAll()
        {
            var result = await GetAll();
            if (result == null) return Failure("Not a single Order was found.");

            return Success(result);
        }

        /// <summary>
        /// Gets user's all detailed Orders. 
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="DetailedDataOrder"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromUser(int userId)
        {
            var result = await GetFromUser(userId);

            if (result == null) return Failure(
                string.Format("No Order with userId {0} was found.", userId)
            );
            return Success(result);
        }

        /// <summary>
        /// Gets all detailed Orders from a project.
        /// </summary>
        /// <param name="projectId">Order's Project id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="DetailedDataOrder"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetFromProject(int projectId)
        {
            var result = await GetFromProject(projectId);

            if (result == null) return Failure(
                string.Format("No Order with projectId {0} was found.", projectId)
            );
            return Success(result);
        }

        /// <summary>
        /// Gets a detailed Order by its id.
        /// </summary>
        /// <param name="orderId">Order id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="DetailedDataOrder"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGet(int orderId)
        {
            var result = await Get(orderId);

            if (result == null) return Failure(
                string.Format("No Order with id {0} was found.", orderId)
            );
            return Success(result);
        }

        /// <summary>
        /// Creates a new detailed Order.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a <see cref="BasicDataOrder.OrderId"/>
        /// or Failure result if element has not been created.
        /// </returns>
        public async Task<GuardResult> GuardedCreate(Fork.ViewModels.Order.CreationViewModel model)
        {
            // Checks if classroom id & storage id are not 0.
            if (model.StorageId == 0 || model.UserId == 0)
                return Failure("StorageId/UserId cannot be 0.");


            // Checks if storage exists (has to) and returns failure in case it doesn't.
            var doesStorageExist =
                await StorageService.GuardedGet(model.StorageId);

            if (doesStorageExist.Code == Status.Failure)
                return Failure(doesStorageExist.Info);


            // Checks if mentionned Ordered Products exist in the mentionned Storage.
            var existingStorageProducts = 
                await SLPService.GuardedGetAllFromStorage(model.StorageId);

            IEnumerable<BasicDataStorageSLP> CastIntoEnumerableSLP(object obj)
                => (IEnumerable<BasicDataStorageSLP>)obj;

            List<int> id = new List<int>();
            foreach (var existingStorageProduct in CastIntoEnumerableSLP(existingStorageProducts.Content))
                id.Add(existingStorageProduct.StorageLinkedProductId);

            foreach (var orderedProduct in model.Products)
                if (!id.Contains(orderedProduct.StorageLinkedProductId))
                {
                    return Failure(string.Format(
                        "Product with StorageLinkedProductId {0} is not referenced by Storage with id {1}.",
                        orderedProduct.StorageLinkedProductId, model.StorageId)
                    );
                }


            // Checks if user exists (has to) and returns failure in case he doesn't.
            var doesUserExist =
                await UserService.GuardedGet(model.UserId);

            if (doesUserExist.Code == Status.Failure)
                return Failure(doesUserExist.Info);


            // Checks if classroom exists (has to) and returns failure in case it doesn't.
            var doesClassroomExist =
                await ClassroomService.GuardedGet(model.ClassroomId);

            if (doesClassroomExist.Code == Status.Failure)
                return Failure(doesClassroomExist.Info);


            // Checks if each one of the products of the list exists in mentionned storage.
            foreach (var product in model.Products)
            {
                // Checks if StorageLinkedProduct id & Quantity id are not 0.
                if (product.StorageLinkedProductId == 0 || product.Quantity == 0)
                    return Failure("StorageLinkedProductId/Quantity cannot be 0.");

                var productDoesExist =
                    await SLPService.GuardedGetFromStorage(product.StorageLinkedProductId, model.StorageId);

                if (productDoesExist.Code == Status.Failure)
                    return Failure(string.Format(
                        "Storage Linked Product with id '{0}' in Storage with id '{1}' does not exist in database.",
                        product.StorageLinkedProductId,
                        model.StorageId));
            }


            // Launches the creation process.
            if (doesUserExist.Code == Status.Success)
            {
                var result = await Create(model);
                var detailedOrder = await Get(result);
                await OrderDueServices.GuardedCreateFinalDue(detailedOrder);

                return (result > 0) ? Success(result) : Failure("Error in creation process.");
            }
            return Failure("User does not exist.");
        }

        /// <summary>
        /// Updates an Order global Current State.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedUpdateCurrentState(OrderCurrentStateUpdateViewModel model)
        {
            var doesOrderExist = await Get(model.OrderId);
            if (doesOrderExist == null) return Failure(
                string.Format("No Order with id {0} was found.", model.OrderId)
            );

            var result = await UpdateCurrentState(model);
            if (result == false) return Failure("Error in update process.");

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<List<DetailedDataOrder>> GetAll()
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
                    var detailedData = new DetailedDataOrder
                    {
                        Info = data,
                        Products = await ctx[OrderTable].Connection
                        .QueryAsync<DetailedDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts v
                            WHERE
                                v.OrderId = @id
                            AND
                                v.CurrentState != 4",
                            new { id = data.OrderId }
                        )
                    };
                    detailedData.Info.Total = CalculateOrderTotal(detailedData.Products);

                    ordersList.Add(detailedData);
                }
                return ordersList;
            }
        }

        private async Task<List<DetailedDataOrder>> GetFromUser(int userId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var basicData = await ctx[OrderTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vOrders
                        WHERE
                            UserId = @id;",
                        new { id = userId }
                    );

                List<DetailedDataOrder> ordersByUser = new List<DetailedDataOrder>();
                foreach (var data in basicData)
                {
                    var detailedData = new DetailedDataOrder
                    {
                        Info = data,
                        Products = await ctx[OrderTable].Connection
                        .QueryAsync<DetailedDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts
                            WHERE
                                OrderId = @id;",
                            new { id = data.OrderId }
                        )
                    };
                    detailedData.Info.Total = CalculateOrderTotal(detailedData.Products);

                    ordersByUser.Add(detailedData);
                }
                return ordersByUser;
            }
        }

        private async Task<IEnumerable<DetailedDataOrder>> GetFromProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                // Retrieves storageId from projectId.
                var storage = await StorageService.GuardedGetFromProject(projectId);

                var basicData = await ctx[OrderTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vOrders
                        WHERE
                            StorageId = @id
                            AND
                            CurrentState != 3
                            AND
                            CurrentState != 4;",
                        new { id = ((BasicDataStorage)storage.Content).StorageId }
                    );

                List<DetailedDataOrder> ordersFromProject = new List<DetailedDataOrder>();
                foreach (var data in basicData)
                {
                    var detailedData = new DetailedDataOrder
                    {
                        Info = data,
                        Products = await ctx[OrderTable].Connection
                        .QueryAsync<DetailedDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts
                            WHERE
                                OrderId = @id
                            AND
                                CurrentState != 4;",
                            new { id = data.OrderId }
                        )
                    };
                    detailedData.Info.Total = CalculateOrderTotal(detailedData.Products);

                    foreach (var product in detailedData.Products)
                    {
                        var state = await ctx[OrderTable].Connection
                            .QueryFirstOrDefaultAsync<Payment>(
                                @"SELECT
                                    PaymentState
                                FROM
                                    ITIH.vOrderedProducts
                                WHERE
                                    OrderedProductId = @id;",
                                new { id = product.OrderedProductId }
                            );

                        var amounts = await ctx[OrderPaymentTable].Connection
                            .QueryAsync<int>(
                                @"SELECT
                                    Amount
                                FROM
                                    ITIH.tOrderPayment
                                WHERE
                                    OrderedProductId = @id;",
                                new { id = product.OrderedProductId }
                            );

                        var total = 0;
                        foreach (var amount in amounts)
                            total += amount;

                        product.Payment = new DetailedDataOrderedProduct.PaymentState
                        {
                            State = state,
                            Amount = total
                        };
                    }

                    ordersFromProject.Add(detailedData);
                }
                return ordersFromProject;
            }
        }

        private async Task<DetailedDataOrder> Get(int orderId)
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
                    Info = basicData,
                    Products = await ctx[OrderTable].Connection
                        .QueryAsync<DetailedDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts
                            WHERE
                                OrderId = @id
                            AND
                                CurrentState != 4;",
                            new { id = orderId }
                        )
                };
                return detailedData;
            }
        }

        private async Task<int> Create(Fork.ViewModels.Order.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var date = DateTime.Now;
                var order =
                await OrderTable.Create(ctx, model.UserId, model.StorageId, model.UserId, model.ClassroomId, date.AddHours(1));

                foreach (var product in model.Products)
                {
                    var orderedProduct =
                        await OrderedProductTable.Create(ctx, model.UserId, order, product.StorageLinkedProductId, product.Quantity);

                    // Updates matching SLP Stock.
                    BasicDataStorageSLP CastIntoSLP(object obj)
                        => (BasicDataStorageSLP)obj;
                    var slp = await SLPService.GuardedGet(product.StorageLinkedProductId);

                    if (CastIntoSLP(slp.Content).Stock == 0) return 0;

                    var update = new UpdateViewModel
                    {
                        StorageLinkedProductId = CastIntoSLP(slp.Content).StorageLinkedProductId,
                        UnitPrice = CastIntoSLP(slp.Content).UnitPrice,
                        Stock = CastIntoSLP(slp.Content).Stock - product.Quantity
                    };
                    await SLPService.GuardedUpdate(update);

                    // In case of an insertion problem, one have to clean the whole order up.
                    if (orderedProduct == 0)
                    {
                        var alreadyOrdered = await Get(order);

                        foreach (var alreadyOrderedProduct in alreadyOrdered.Products)
                        {
                            await OrderedProductTable.Delete(ctx, 1, alreadyOrderedProduct.OrderedProductId);

                            update.Stock += alreadyOrderedProduct.Quantity;
                            await SLPService.GuardedUpdate(update);
                        }
                        await OrderTable.Delete(ctx, 1, order);
                        return 0;
                    }
                }
                return order;
            }
        }

        private async Task<bool> UpdateCurrentState(OrderCurrentStateUpdateViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderTable.Update(ctx, model.UserId, model.OrderId, model.CurrentState);
            }
        }

        private async Task<bool> DeleteOrder(Fork.ViewModels.Order.DeletionViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderTable.Delete(ctx, model.ActorId, model.OrderId);
            }
        }

        private async Task<bool> DeleteOrderedProduct(Fork.ViewModels.Product.Ordered.DeletionViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderedProductTable.Delete(ctx, model.ActorId, model.OrderedProductId);
            }
        }

        private static int CalculateOrderTotal(IEnumerable<DetailedDataOrderedProduct> products)
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
