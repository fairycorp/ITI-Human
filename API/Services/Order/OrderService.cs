using API.Services.Classroom;
using API.Services.Helper;
using API.Services.Product;
using API.Services.Storage;
using API.Services.User;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Product.Ordered;
using ITI.Human.ViewModels.Storage.LinkedProduct;
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

        public OrderedProductService OrderedProductService { get; set; }

        public OrderDueServices OrderDueServices { get; set; }

        public ClassroomService ClassroomService { get; set; }

        public UserService UserService { get; set; }

        public OrderTable OrderTable { get; set; }

        public OrderService(StorageService sService, SLPService slpService,
            OrderedProductService oPService, OrderDueServices oDServices,
            ClassroomService cService, UserService uService, OrderTable oTable)
        {
            StorageService = sService;
            SLPService = slpService;
            OrderedProductService = oPService;
            OrderDueServices = oDServices;
            ClassroomService = cService;
            UserService = uService;
            OrderTable = oTable;
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
        public async Task<GuardResult> GuardedCreate(ITI.Human.ViewModels.Order.CreationViewModel model)
        {
            // Checks if classroom id & storage id are not 0.
            if (model.StorageId == 0 || model.UserId == 0)
                return Failure("StorageId/UserId cannot be 0.");


            // Checks if storage exists (has to) and returns failure in case it doesn't.
            var doesStorageExist =
                await Attempt.ToGetElement(StorageService.GuardedGet, model.StorageId, true);

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

        // NEEDS TO BE FULLY REWORKED !
        // TODO: Each update type must have its own matching method.
        /// <summary>
        /// Updates a detailed Order delivery state.
        /// </summary>
        /// <param name="model">Matching model.</param>
        public async Task<GuardResult> GuardedUpdate(ITI.Human.ViewModels.Order.DetailedDataOrder model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var doesOrderExist = 
                    await Attempt.ToGetElement(Get, model.Info.OrderId, true);

                if (doesOrderExist.Code == Status.Success)
                {
                    Dictionary<string, GuardResult> dataUpdates = new Dictionary<string, GuardResult>();
                    var entirelyDelivered = true;

                    foreach (var product in model.Products)
                    {
                        var doesOPExist =
                            await OrderedProductService.GuardedGet(product.OrderedProductId);

                        if (doesOPExist.Code == Status.Success)
                        {
                            dataUpdates.Add(
                                string.Format("Has OrderedProduct n°{0} Current State been updated?", product.OrderedProductId),
                                await OrderedProductService.GuardedUpdateCurrentState(product.OrderedProductId, product.CurrentState)
                            );

                            var orderFinalDueId = await ctx[OrderDueServices.OrderFinalDueTable].Connection
                                .QueryFirstOrDefaultAsync<int>(
                                    "SELECT OrderFinalDueId FROM ITIH.tOrderFinalDue WHERE OrderId = @Id",
                                    new { Id = model.Info.OrderId }
                                );

                            var orderFinalPaid = await ctx[OrderDueServices.OrderFinalDueTable].Connection
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
                                        await ctx[OrderDueServices.OrderPaymentTable].Connection
                                            .QueryFirstOrDefaultAsync<int>(
                                                "SELECT OrderPaymentId FROM ITIH.tOrderPayment WHERE OrderedProductId = @Id;",
                                                new { Id = product.OrderedProductId }
                                            );
                                    if (doesPaymentStateExist == 0)
                                    {
                                        dataUpdates.Add(
                                            string.Format("Has OrderedProduct n°{0} Payment State been updated?", product.OrderedProductId),
                                            await OrderedProductService.GuardedUpdatePaymentState(
                                                product.OrderedProductId,product.Payment.State, product.Payment.Amount
                                            )
                                        );
                                    }
                                    break;

                                case Payment.Unpaid:
                                    if (orderFinalPaid > 0)
                                    {
                                        if (orderFinalPaid >= product.UnitPrice)
                                            product.Payment.Amount = -(product.UnitPrice);

                                        var isOrderedProductReferencedInPaymentTable =
                                            await ctx[OrderDueServices.OrderFinalDueTable].Connection
                                                .QueryAsync<int>("SELECT OrderPaymentId FROM ITIH.tOrderPayment WHERE OrderedProductId = @Id",
                                                new { Id = product.OrderedProductId });

                                        if (isOrderedProductReferencedInPaymentTable.AsList().Count >= 1)
                                        {
                                            await OrderDueServices.OrderPaymentTable.Delete(ctx, 0, product.OrderedProductId);
                                            await OrderDueServices.OrderFinalDueTable.Update(ctx, 0, orderFinalDueId, product.Payment.Amount);
                                        }

                                    }
                                    else product.Payment.Amount = 0;
                                    break;

                                case Payment.Credited:
                                    if (product.Payment.Amount > 0)
                                    {
                                        if (product.Payment.Amount >= product.UnitPrice)
                                            return Failure("A Credit cannot be superior or equal to the ordered product unit price.");

                                        // Checks if the ordered product can be credited or not.
                                        var canBeCredited = await ctx[SLPService.SLPTable].Connection
                                            .QueryFirstOrDefaultAsync<bool>(
                                            "SELECT CreditState FROM ITIH.tStorageLinkedProduct WHERE StorageLinkedProductId = @Id",
                                            new { Id = product.StorageLinkedProductId }
                                        );

                                        if (!canBeCredited) return Failure(
                                                string.Format("StorageLinkedProduct n°{0} cannot be credited.", product.StorageLinkedProductId)
                                            );

                                        // Checks if the ordered product is already referenced in Credit table.
                                        var isOrderedProductReferencedInCreditTable =
                                            await ctx[OrderedProductService.OrderedProductTable].Connection
                                                .QueryAsync<int>("SELECT OrderCreditId FROM ITIH.tOrderCredit WHERE OrderedProductId = @Id",
                                                new { Id = product.OrderedProductId });


                                        if (isOrderedProductReferencedInCreditTable.AsList().Count == 0)
                                        {
                                            var created = await OrderDueServices.OrderCreditTable.Create(ctx, 0, product.OrderedProductId, product.Payment.Amount, DateTime.UtcNow);
                                            dataUpdates.Add(
                                                string.Format("Has a credit been created for Ordered Product n°{0}", product.OrderedProductId),
                                                Success(created)
                                            );
                                        }
                                    }
                                    else return Failure("When Payment State is 'Credited', 'Amount' must be filled in.");
                                    break;
                            }

                            var currentState = await ctx[OrderedProductService.OrderedProductTable].Connection
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

                    return Success("WIP update result");
                }
                return Failure(doesOrderExist.Info);
            }
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

        private async Task<IEnumerable<DetailedDataOrder>> GetFromProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                // Retrieves storageId from projectId.
                var storageId = await StorageService.GuardedGetFromProject(projectId);

                var basicData = await ctx[OrderTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.tOrder
                        WHERE
                            StorageId = @id;",
                        new { id = storageId.Content }
                    );

                List<DetailedDataOrder> ordersFromProject = new List<DetailedDataOrder>();
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

        private async Task<int> Create(ITI.Human.ViewModels.Order.CreationViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var order =
                await OrderTable.Create(ctx, model.UserId, model.StorageId, model.UserId, model.ClassroomId, DateTime.UtcNow);

                foreach (var product in model.Products)
                {
                    var orderedProduct =
                        await OrderedProductService.OrderedProductTable.Create(ctx, model.UserId, order, product.StorageLinkedProductId, product.Quantity);

                    // Updates matching SLP Stock.
                    BasicDataStorageSLP CastIntoSLP(object obj)
                        => (BasicDataStorageSLP)obj;
                    var slp = await SLPService.GuardedGet(product.StorageLinkedProductId);

                    if (CastIntoSLP(slp.Content).Stock == 0) return 0;

                    var update = new ITI.Human.ViewModels.Storage.LinkedProduct.UpdateViewModel
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
                            await OrderedProductService.OrderedProductTable.Delete(ctx, 0, alreadyOrderedProduct.OrderedProductId);

                            update.Stock += alreadyOrderedProduct.Quantity;
                            await SLPService.GuardedUpdate(update);
                        }
                        await OrderTable.Delete(ctx, 0, order);
                        return 0;
                    }
                }
                return order;
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
