using API.Services.Storage;
using API.Services.User;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Fork.ViewModels.Order;
using Fork.ViewModels.Order.Payment;
using Fork.ViewModels.Product.Ordered;
using Fork.ViewModels.Storage;
using Fork.ViewModels.Storage.LinkedProduct;
using Fork.ViewModels.User;
using Stall.Guard.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static API.Services.Helper.ResultFactory;

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

        public UserBalanceService UserBalanceService { get; set; }

        public OrderedProductTable OrderedProductTable { get; set; }

        public OrderedProductService(SLPService slpService, OrderService oService,
            OrderDueServices oDServices, UserBalanceService uBService, OrderedProductTable oPTable)
        {
            OrderService = oService;
            OrderDueServices = oDServices;
            SLPService = slpService;
            UserBalanceService = uBService;
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
        public async Task<GuardResult> GuardedUpdateCurrentState(int userId, int orderedProductId, State currentState)
        {
            // Checks if Ordered Product exists.
            // If not, returns Failure().
            var doesOPExist =
                await Get(orderedProductId);

            if (doesOPExist == null) return Failure(
                string.Format("No Ordered Product with id {0} was found.", orderedProductId)
            );

            var result = await UpdateCurrentState(userId, orderedProductId, currentState);
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
        public async Task<GuardResult> GuardedUpdatePaymentState(int actorId, int userId, int orderedProductId, Payment paymentState, int amount)
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

            BasicDataUserBalance CastIntoUserBalance(object obj)
                => (BasicDataUserBalance)obj;

            bool result = false;
            switch (paymentState)
            {
                case Payment.Paid:
                    if (doesOPExist.PaymentState == Payment.Credited)
                        return Failure("Cannot pay this way. A credit has been made for this ordered product.");

                    amount = doesOPExist.UnitPrice;
                    
                    if (doesPSExist.Content == null)
                    {
                        result = await UpdatePaymentState(userId, orderedProductId, paymentState, amount);
                    }
                    break;

                case Payment.Unpaid:
                    if (doesOPExist.PaymentState == Payment.Credited)
                        return Failure("Cannot unpay this way. A credit has been made for this ordered product.");

                    var orderFinalPaid = CastIntoOrderFinalDue(orderFinalDue.Content).Info.Paid;
                    if (orderFinalPaid > 0)
                    {
                        if (orderFinalPaid >= doesOPExist.UnitPrice)
                            amount = -(doesOPExist.UnitPrice);

                        var orderedProductsPaymentStateReferences =
                            await OrderDueServices.GuardedGetAllOrderPaymentsFromOrderedProduct(orderedProductId);

                        if (CastIntoOrderPaymentEnumerable(orderedProductsPaymentStateReferences.Content).AsList().Count > 0)
                        {
                            var deletion = await OrderDueServices.GuardedDeleteOrderPayments(userId, orderedProductId);
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

                        if (doesOPExist.PaymentState == Payment.Paid)
                            return Failure("This ordered product has already been paid.");

                        // Checks if the ordered product can be credited or not.
                        var slp = await SLPService.GuardedGet(doesOPExist.StorageLinkedProductId);
                        if (!CastIntoSLP(slp.Content).CreditState)
                        {
                            return Failure(
                                string.Format("StorageLinkedProduct with id {0} cannot be credited.", doesOPExist.StorageLinkedProductId)
                            );
                        }

                        // Retrieves concerned storage.
                        var storage = CastIntoStorage(
                            (await SLPService.StorageService.GuardedGetFromOrderedProduct(doesOPExist)
                        ).Content);

                        // Prepares update function for the following step.
                        async Task<GuardResult> UpdateBalance(int uBId, int amnt)
                        {
                            if (amnt >= 0) return Failure("Amount must be negative.");
                            return await UserBalanceService.GuardedUpdateUserBalance(uBId, amnt);
                        }
                        // Creates a new User Balance if it doesn't exist,
                        // then edits it or directly edits the already existing one.
                        var userBalance = await UserBalanceService.GuardedGetFromUser(userId);

                        if (userBalance.Code == Status.Failure)
                        {
                            var createdBalance = await UserBalanceService.GuardedCreateUserBalance(userId, storage.ProjectId);

                            await UpdateBalance(
                                (int)createdBalance.Content,
                                -(amount)
                            );
                        }
                        else
                        {
                            await UpdateBalance(
                                CastIntoUserBalance(userBalance.Content).UserBalanceId,
                                -(amount)
                            );
                        }

                        // Creates a new Order Credit.
                        var createdCredit = await OrderDueServices.GuardedCreateOrderCredit(
                            userId,
                            storage.ProjectId,
                            userId,
                            amount
                        );
                        // Updates Ordered Product payment state (sets it to Payment.Credited).
                        using (var ctx = new SqlStandardCallContext())
                        {
                            var date = DateTime.Now;
                            var updated = await OrderedProductTable.UpdatePaymentState(
                                ctx, userId, date.AddHours(1), orderedProductId, 
                                CastIntoOrderFinalDue(orderFinalDue.Content).Info.OrderFinalDueId,
                                paymentState, 0
                            );
                        }

                        result = ((int)createdCredit.Content > 0) ? true : false;
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

        private async Task<bool> UpdateCurrentState(int userId, int orderedProductId, State currentState)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var date = DateTime.Now;
                return await OrderedProductTable.UpdateCurrentState(ctx, userId, date.AddHours(1), orderedProductId, currentState);
            }
        }

        private async Task<bool> UpdatePaymentState(int userId, int orderedProductId, Payment paymentState, int amount)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var orderedProduct = await Get(orderedProductId);
                var orderFinalDue = await OrderDueServices.GuardedGetFinalDueFromOrder(orderedProduct.OrderId);

                var date = DateTime.Now;
                return await OrderedProductTable.UpdatePaymentState(ctx, userId, date.AddHours(1), orderedProductId,
                    ((DetailedDataOrderFinalDue)orderFinalDue.Content).Info.OrderFinalDueId, paymentState, amount);
            }
        }
    }
}
