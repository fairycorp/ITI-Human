using API.Services.Classroom;
using API.Services.Helper;
using API.Services.Product;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Product.Ordered;
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
using ITI.Human.ViewModels.Storage.LinkedProduct;

namespace API.Services.Order
{
    /// <summary>
    /// Handles any database interaction concerning Orders.
    /// </summary>
    public class OrderService
    {
        public StorageService StorageService { get; set; }

        public ClassroomService ClassroomService { get; set; }

        public OrderTable OrderTable { get; set; }

        public StorageLinkedProductTable StorageLinkedProductTable { get; set; }

        public OrderedProductTable OrderedProductTable { get; set; }

        public OrderFinalDueTable OrderFinalDueTable { get; set; }

        public OrderPaymentTable OrderPaymentTable { get; set; }

        public OrderCreditTable OrderCreditTable { get; set; }

        public UserTable UserTable { get; set; }

        public OrderService(
            StorageService sService, ClassroomService cService, OrderTable oTable, 
            StorageLinkedProductTable sLPTable, OrderedProductTable oPTable,
            OrderFinalDueTable oFDTable, OrderPaymentTable oPayTable, 
            OrderCreditTable oCTable, UserTable uTable)
        {
            StorageService = sService;
            ClassroomService = cService;
            OrderTable = oTable;
            StorageLinkedProductTable = sLPTable;
            OrderedProductTable = oPTable;
            OrderFinalDueTable = oFDTable;
            OrderPaymentTable = oPayTable;
            OrderCreditTable = oCTable;
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
        /// <returns>Success result where result content is a list of DetailedDataOrder or Failure result if not one element was found in db.</returns>
        public async Task<GuardResult> GuardedGetDetailedOrdersFromUser(int userId)
        {
            var result = await GetDetailedOrdersFromUser(userId);

            if (result == null) return Failure(
                string.Format("Not one single element was foubd in db. | in {0}", nameof(GuardedGetDetailedOrdersFromUser))
            );
            return Success(result);
        }

        /// <summary>
        /// Gets a detailed Order by its id.
        /// </summary>
        /// <param name="orderId">Order id.</param>
        /// <returns>Success result where result content is a single DetailedDataOrder or Failure result if element does not exist in db.</returns>
        public async Task<GuardResult> GuardedGetDetailedOrder(int orderId)
        {
            var result = await GetDetailedOrder(orderId);

            if (result == null) return Failure(
                string.Format("Element does not exist in database. | in {0}", nameof(GuardedCreateDetailedOrder))
            );
            return Success(result);
        }

        /// <summary>
        /// Gets an Ordered Product by its id.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product id.</param>
        /// <returns>Success result where result content is a single BasicDataOrderedProduct or Failure result if element does not exist in db.</returns>
        public async Task<GuardResult> GuardedGetOrderedProduct(int orderedProductId)
        {
            var result = await GetOrderedProduct(orderedProductId);

            if (result == null) return Failure(
                string.Format("Element does not exist in database. | in {0}", nameof(GuardedGetOrderedProduct))
            );
            return Success(result);
        }

        /// <summary>
        /// Creates a new detailed Order.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result content is int OR Failure result in case insertion process failed.</returns>
        public async Task<GuardResult> GuardedCreateDetailedOrder(ITI.Human.ViewModels.Order.CreationViewModel model)
        {
            // Checks if classroom id & storage id are not 0.
            if (model.StorageId == 0 || model.UserId == 0)
                return Failure("StorageId/UserId cannot be 0.");


            // Checks if storage exists (has to) and returns failure in case it doesn't.
            var doesStorageExist =
                await Attempt.ToGetElement(StorageService.GetStorage, model.StorageId, true);

            if (doesStorageExist.Code == Status.Failure)
                return Failure(doesStorageExist.Info);


            // Checks if mentionned Ordered Products exist in the mentionned Storage.
            var existingStorageProducts = 
                await StorageService.GetAllStorageLinkedProductsFromStorage(model.StorageId);

            IEnumerable<BasicDataStorageLinkedProduct> CastIntoEnumerableSLP(object obj)
                => (IEnumerable<BasicDataStorageLinkedProduct>)obj;

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
                await Attempt.ToGetElement(GetUser, model.UserId, true);

            if (doesUserExist.Code == Status.Failure)
                return Failure(doesUserExist.Info);


            // Checks if classroom exists (has to) and returns failure in case it doesn't.
            var doesClassroomExist =
                await Attempt.ToGetElement(ClassroomService.GetClassroom, model.ClassroomId, true);

            if (doesClassroomExist.Code == Status.Failure)
                return Failure(doesClassroomExist.Info);


            // Checks if each one of the products of the list exists in mentionned storage.
            foreach (var product in model.Products)
            {
                // Checks if StorageLinkedProduct id & Quantity id are not 0.
                if (product.StorageLinkedProductId == 0 || product.Quantity == 0)
                    return Failure("StorageLinkedProductId/Quantity cannot be 0.");

                var productDoesExist =
                    await Attempt.ToGetElement(StorageService.GetStorageLinkedProductFromStorage, product.StorageLinkedProductId, model.StorageId, true);

                if (productDoesExist.Code == Status.Failure)
                    return Failure(string.Format(
                        "Storage Linked Product with id '{0}' in Storage with id '{1}' does not exist in database.",
                        product.StorageLinkedProductId,
                        model.StorageId));
            }


            // Launches the creation process.
            if (doesUserExist.Code == Status.Success)
            {
                var result = await CreateDetailedOrder(model);
                var detailedOrder = await GetDetailedOrder(result);
                await CreateOrderFinalDue(detailedOrder);

                return (result > 0) ? Success(result) : Failure("Error in creation process.");
            }
            return Failure("User does not exist.");
        }

        /// <summary>
        /// Updates a detailed Order delivery state.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>Success result where result content is null OR Failure result in case element does not exist in DB.</returns>
        public async Task<GuardResult> UpdateDetailedOrder(ITI.Human.ViewModels.Order.UpdateViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesOrderExist = 
                    await Attempt.ToGetElement(GetDetailedOrder, model.Info.OrderId, true);

                if (doesOrderExist.Code == Status.Success)
                {
                    Dictionary<string, bool> dataUpdates = new Dictionary<string, bool>();
                    var entirelyDelivered = true;

                    foreach (var product in model.Products)
                    {
                        var doesOPExist =
                            await Attempt.ToGetElement(GetOrderedProduct, product.OrderedProductId, true);

                        if (doesOPExist.Code == Status.Success)
                        {
                            dataUpdates.Add(
                                string.Format("Has OrderedProduct n°{0} Current State been updated?", product.OrderedProductId),
                                await OrderedProductTable.UpdateCurrentState(ctx, 0, DateTime.UtcNow, product.OrderedProductId, product.CurrentState)
                            );

                            var orderFinalDueId = await ctx[OrderFinalDueTable].Connection
                                .QueryFirstOrDefaultAsync<int>(
                                    "SELECT OrderFinalDueId FROM ITIH.tOrderFinalDue WHERE OrderId = @Id",
                                    new { Id = model.Info.OrderId }
                                );

                            var orderFinalPaid = await ctx[OrderFinalDueTable].Connection
                                .QueryFirstOrDefaultAsync<double>(
                                    "SELECT Paid FROM ITIH.tOrderFinalDue WHERE OrderFinalDueId = @Id",
                                    new { Id = orderFinalDueId }
                                );

                            switch (product.Payment.State)
                            {
                                case Payment.Paid:
                                    product.Payment.Amount = product.UnitPrice;

                                    // Looks for an existing Payment State in database.
                                    var doesPaymentStateExist =
                                        await ctx[OrderPaymentTable].Connection
                                            .QueryFirstOrDefaultAsync<int>(
                                                "SELECT OrderPaymentId FROM ITIH.tOrderPayment WHERE OrderedProductId = @Id;",
                                                new { Id = product.OrderedProductId }
                                            );
                                    if (doesPaymentStateExist == 0)
                                    {
                                        dataUpdates.Add(
                                            string.Format("Has OrderedProduct n°{0} Payment State been updated?", product.OrderedProductId),
                                            await OrderedProductTable.UpdatePaymentState(ctx, 0, DateTime.UtcNow, product.OrderedProductId,
                                            orderFinalDueId, product.Payment.State, product.Payment.Amount)
                                        );
                                    }
                                    break;

                                case Payment.Unpaid:
                                    if (orderFinalPaid > 0)
                                    {
                                        if (orderFinalPaid >= product.UnitPrice)
                                            product.Payment.Amount = -(product.UnitPrice);

                                        var isOrderedProductReferencedInPaymentTable =
                                            await ctx[OrderPaymentTable].Connection
                                                .QueryAsync<int>("SELECT OrderPaymentId FROM ITIH.tOrderPayment WHERE OrderedProductId = @Id",
                                                new { Id = product.OrderedProductId });

                                        if (isOrderedProductReferencedInPaymentTable.AsList().Count >= 1)
                                        {
                                            await OrderPaymentTable.Delete(ctx, 0, product.OrderedProductId);
                                            await OrderFinalDueTable.Update(ctx, 0, orderFinalDueId, product.Payment.Amount);
                                        }

                                    }
                                    else product.Payment.Amount = 0;
                                    break;

                                case Payment.Credited:
                                    if (product.Payment.Amount > 0)
                                    {
                                        if (product.Payment.Amount > product.UnitPrice)
                                            return Failure("A Credit cannot be superior to the ordered product unit price.");

                                        // Checks if the ordered product can be credited or not.
                                        var canBeCredited = await ctx[StorageLinkedProductTable].Connection
                                            .QueryFirstOrDefaultAsync<bool>(
                                            "SELECT CreditState FROM ITIH.tStorageLinkedProduct WHERE StorageLinkedProductId = @Id",
                                            new { Id = product.StorageLinkedProductId }
                                        );

                                        if (!canBeCredited) return Failure(
                                                string.Format("StorageLinkedProduct n°{0} cannot be credited.", product.StorageLinkedProductId)
                                            );

                                        // Checks if the ordered product is already referenced in Credit table.
                                        var isOrderedProductReferencedInCreditTable =
                                            await ctx[OrderCreditTable].Connection
                                                .QueryAsync<int>("SELECT OrderCreditId FROM ITIH.tOrderCredit WHERE OrderedProductId = @Id",
                                                new { Id = product.OrderedProductId });

                                        if (isOrderedProductReferencedInCreditTable.AsList().Count == 0)
                                        {
                                            await OrderCreditTable.Create(ctx, 0, product.OrderedProductId, product.Payment.Amount, DateTime.UtcNow);
                                            dataUpdates.Add(
                                                string.Format("Has a credit been created for Ordered Product n°{0}", product.OrderedProductId),
                                                true
                                            );
                                        }
                                    }
                                    else return Failure("When Payment State is 'Credited', 'Amount' must be filled in.");
                                    break;
                            }

                            var currentState = await ctx[OrderedProductTable].Connection
                                .QueryFirstOrDefaultAsync<State>(
                                    "SELECT CurrentState FROM ITIH.vOrderedProducts WHERE OrderedProductId = @Id",
                                    new { Id = product.OrderedProductId }
                                );

                            if (currentState != State.Delivered) entirelyDelivered = false;
                        }
                        else return Failure(doesOPExist.Info);
                    }

                    if (entirelyDelivered)
                    {
                        var hasBeenEntirelyDelivered =
                            OrderTable.Update(ctx, 0, model.Info.OrderId, State.Delivered);
                    }

                    var successReturn = false;
                    foreach (var update in dataUpdates)
                        if (update.Value == true)
                            successReturn = true;

                    return successReturn
                        ? Success(dataUpdates) 
                        : Failure("No update was proceeded.");
                }
                return Failure(doesOrderExist.Info);
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
            return (result == model.CurrentState) ? Success(result) : Failure("Error in update process.");
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
                    var detailedData = new DetailedDataOrder
                    {
                        Info = data,
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
                    };
                    detailedData.Info.Total = CalculateOrderTotal(detailedData.Products);

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
                        .QueryAsync<BasicDataOrderedProduct>(
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
                    Info = basicData,
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

        private async Task<int> CreateDetailedOrder(ITI.Human.ViewModels.Order.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var order =
                await OrderTable.Create(ctx, model.UserId, model.StorageId, model.UserId, model.ClassroomId, DateTime.UtcNow);

                foreach (var product in model.Products)
                {
                    var orderedProduct =
                        await OrderedProductTable.Create(ctx, model.UserId, order, product.StorageLinkedProductId, product.Quantity);

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

        private async Task<State> UpdateDetailedOrderCurrentState(BasicDataOrder model)
        {
            var doesExist =
                    await Attempt.ToGetElement(GetDetailedOrder, model.OrderId, true);

            if (doesExist.Code == Status.Success)
            {
                using (var ctx = new SqlStandardCallContext())
                {
                    return await OrderTable.Update(ctx, 0, model.OrderId, model.CurrentState);
                }
            }
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderTable].Connection
                    .QueryFirstOrDefaultAsync<State>(
                        "SELECT CurrentState FROM ITIH.tOrder WHERE OrderId = @OrderId;"
                    );
            }
        }

        private async Task<BasicDataOrderedProduct> GetOrderedProduct(int orderedProductId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderedProductTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderedProduct>(
                        "SELECT * FROM ITIH.vOrderedProducts WHERE OrderedProductId = @Id",
                        new { Id = orderedProductId }
                    );
            }
        }

        private async Task<int> CreateOrderFinalDue(DetailedDataOrder order)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                //var storage = StorageService.GetStorageFromOrder(order.Info.OrderId);
                return await OrderFinalDueTable.Create(ctx, 0, order.Info.OrderId, CalculateOrderTotal(order.Products), 0);
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
