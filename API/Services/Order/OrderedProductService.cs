using API.Services.Storage;
using CK.SqlServer;
using Dapper;
using ITI.Human.Data;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Order.Payment;
using ITI.Human.ViewModels.Product.Ordered;
using ITI.Human.ViewModels.Storage;
using ITI.Human.ViewModels.Storage.LinkedProduct;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;
using static ITI.Human.ViewModels.Product.Ordered.DetailedDataOrderedProduct;

namespace API.Services.Order
{
    /// <summary>
    /// Handles interactions with Ordered Products.
    /// </summary>
    public class OrderedProductService
    {
        public OrderService OrderService { get; set; }

        public OrderDueServices OrderDueServices { get; set; }

        public SLPService SLPService { get; set; }

        public OrderedProductTable OrderedProductTable { get; set; }

        public OrderedProductService(SLPService slpService, OrderService oService,
            OrderDueServices oDServices, OrderedProductTable oPTable)
        {
            OrderService = oService;
            OrderDueServices = oDServices;
            SLPService = slpService;
            OrderedProductTable = oPTable;
        }

        /// <summary>
        /// Gets an Ordered Product by its id.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="DetailedDataOrderedProduct"/> 
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
        /// Success result where result content is a single <see cref="DetailedDataOrderedProduct"/> 
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
        public async Task<GuardResult> GuardedUpdatePaymentState(int userId, int orderedProductId, Payment paymentState, int amount)
        {
            // Checks if Ordered Product exists.
            // If not, returns Failure().
            var doesOPExist =
                await Get(orderedProductId);

            if (doesOPExist == null) return Failure(
                string.Format("No Ordered Product with id {0} was found.", orderedProductId)
            );

            // Retrieves Payment State existence.
            var doesPSExist = 
                await OrderDueServices.GuardedGetSingleOrderPaymentFromOrderedProduct(orderedProductId);

            // Retrieves order's final due.
            var orderFinalDue =
                        await OrderDueServices.GuardedGetFinalDueFromOrder(doesOPExist.OrderId);

            // Cast functions declaration.
            IEnumerable<BasicDataOrderPayment> CastIntoOrderPaymentEnumerable(object obj)
                => (IEnumerable<BasicDataOrderPayment>)obj;

            DetailedDataOrderFinalDue CastIntoOrderFinalDue(object obj)
                => (DetailedDataOrderFinalDue)obj;

            BasicDataStorageSLP CastIntoSLP(object obj)
                => (BasicDataStorageSLP)obj;

            BasicDataStorage CastIntoStorage(object obj)
                => (BasicDataStorage)obj;

            bool result = false;
            switch (paymentState)
            {
                case Payment.Paid:
                    amount = doesOPExist.UnitPrice;
                    
                    if (doesPSExist.Content == null)
                    {
                        result = await UpdatePaymentState(orderedProductId, paymentState, amount);
                    }
                    break;

                case Payment.Unpaid:
                    
                    var orderFinalPaid = CastIntoOrderFinalDue(orderFinalDue.Content).Info.Paid;
                    if (orderFinalPaid > 0)
                    {
                        if (orderFinalPaid >= doesOPExist.UnitPrice)
                            amount = -(doesOPExist.UnitPrice);

                        var orderedProductsPaymentStateReferences =
                            await OrderDueServices.GuardedGetAllOrderPaymentsFromOrderedProduct(orderedProductId);

                        if (CastIntoOrderPaymentEnumerable(orderedProductsPaymentStateReferences.Content).AsList().Count > 0)
                        {
                            var deletion = await OrderDueServices.GuardedDeleteOrderPayments(orderedProductId);
                            var update = await OrderDueServices.GuardedUpdateFinalDue(CastIntoOrderFinalDue(orderFinalDue.Content).Info.OrderFinalDueId, amount);
                            result = (deletion.Code == Status.Success || update.Code == Status.Success) ? true : false;
                        }
                    }
                    else amount = 0;
                    break;

                case Payment.Credited:
                    if (amount > 0)
                    {
                        if (doesOPExist.PaymentState == Payment.Credited)
                            return Failure("A credit has already been made for this ordered product.");

                        if (amount >= doesOPExist.UnitPrice)
                            return Failure("A Credit cannot be superior or equal to the ordered product unit price.");

                        // Checks if the ordered product can be credited or not.
                        var slp = await SLPService.GuardedGet(doesOPExist.StorageLinkedProductId);
                        if (!CastIntoSLP(slp.Content).CreditState)
                        {
                            return Failure(
                                string.Format("StorageLinkedProduct with id {0} cannot be credited.", doesOPExist.StorageLinkedProductId)
                            );
                        }

                        var storage = CastIntoStorage(
                            (await SLPService.StorageService.GuardedGetFromOrderedProduct(doesOPExist)
                        ).Content);
                        var created = await OrderDueServices.GuardedCreateOrderCredit(
                            storage.ProjectId,
                            userId,
                            amount
                        );
                        using (var ctx = new SqlStandardCallContext())
                        {
                            var updated = await OrderedProductTable.UpdatePaymentState(
                                ctx, 0, DateTime.UtcNow, orderedProductId, 
                                CastIntoOrderFinalDue(orderFinalDue.Content).Info.OrderFinalDueId,
                                paymentState, 0
                            );
                        }

                        result = ((int)created.Content > 0) ? true : false;
                    }
                    else return Failure("When Payment State is 'Credited', 'Amount' must be filled in.");
                    break;
            }

            return result ? Success(result) : Failure("No update was proceeded.");
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

        private async Task<DetailedDataOrderedProduct> GetFromOrder(int orderedProductId, int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderedProductTable].Connection
                    .QueryFirstOrDefaultAsync<DetailedDataOrderedProduct>(
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
