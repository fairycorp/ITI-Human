﻿using API.Services.Project;
using CK.SqlServer;
using Dapper;
using Fork.Data;
using Fork.ViewModels.Order;
using Fork.ViewModels.Order.Payment;
using Fork.ViewModels.Product.Ordered;
using Fork.ViewModels.User;
using Stall.Guard.System;
using System;
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
        public ProjectService ProjectService { get; set; }

        public OrderFinalDueTable OrderFinalDueTable { get; set; }

        public OrderPaymentTable OrderPaymentTable { get; set; }

        public OrderCreditTable OrderCreditTable { get; set; }

        public UserBalanceTable UserBalanceTable { get; set; }

        public OrderDueServices(ProjectService pService, OrderFinalDueTable oFDTable,
            OrderPaymentTable oPTable, OrderCreditTable oCTable, UserBalanceTable uBTable)
        {
            ProjectService = pService;
            OrderFinalDueTable = oFDTable;
            OrderPaymentTable = oPTable;
            OrderCreditTable = oCTable;
            UserBalanceTable = uBTable;
        }

        /// <summary>
        /// Gets a User Balance from a User's id and a Project id.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataUserBalance"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetUserBalance(UserBalanceGettingViewModel model)
        {
            var result = await GetUserBalance(model);
            if (result == null) return Failure(
                string.Format("No Balance with userId {0} and projectId {1} was found.", model.UserId, model.ProjectId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets all User Balances from a project.
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataUserBalance"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetAllUserBalanceFromProject(int projectId)
        {
            var result = await GetAllUserBalanceFromProject(projectId);
            if (result == null) return Failure(
                string.Format("No Balance with projectId {0} was found.", projectId)
            );

            return Success(result);
        }

        /// <summary>
        /// Updates a User's Balance.
        /// </summary>
        /// <param name="model">Matching model.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedUpdateUserBalance(UserBalanceUpdateViewModel model)
        {
            var result = await UpdateUserBalance(model.UserBalanceId, model.Amount);
            if (!result) return Failure("Error in update process.");

            return Success(result);
        }

        /// <summary>
        /// Gets all Credits from specific User and Project.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// Success result where result content is a list of <see cref="BasicDataOrderCredit"/> 
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetUserCreditsFromProject(UserCreditGettingViewModel model)
        {
            var result = await GetUserCreditsFromProject(model);
            if (result == null) return Failure(
                string.Format("No Credit with userId {0} and projectId {1} was found.", model.UserId, model.ProjectId)
            );

            return Success(result);
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
        /// Updates an Order Final Due.
        /// </summary>
        /// <param name="orderFinalDueId">Order Final Due's id.</param>
        /// <param name="amount">Paid amount.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedUpdateFinalDue(int orderFinalDueId, int amount)
        {
            var doesOrderFinalDueExist =
                await GetOrderFinalDue(orderFinalDueId);

            if (doesOrderFinalDueExist == null) return Failure(
                string.Format("No Due with orderFinalDueId {0} was found.", orderFinalDueId)
            );

            var result = await UpdateFinalDue(orderFinalDueId, amount);
            if (!result) return Failure("No update was proceeded.");

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
        /// Gets the first or default Order Payment from an Ordered Product.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product's id.</param>
        /// <returns>
        /// Success result where result content is a single <see cref="BasicDataOrderPayment"/>
        /// or Failure result if element does not exist in db.
        /// </returns>
        public async Task<GuardResult> GuardedGetSingleOrderPaymentFromOrderedProduct(int orderedProductId)
        {
            var result = await GetSingleOrderPaymentFromOrderedProduct(orderedProductId);
            if (result == null) return Failure(
                string.Format("No OrderPayment with orderedProductId {0} was found", orderedProductId)
            );

            return Success(result);
        }

        /// <summary>
        /// Gets all Order Payments from an Ordered Product.
        /// </summary>
        /// <param name="orderedProductId">Ordered Product's id.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedGetAllOrderPaymentsFromOrderedProduct(int orderedProductId)
        {
            var result = await GetAllOrderPaymentsFromOrderedProduct(orderedProductId);
            if (result == null) return Failure(
                string.Format("No OrderPayment with orderedProductId {0} was found", orderedProductId)
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

        /// <summary>
        /// Deletes all Order Payments of an Ordered Product.
        /// </summary>
        /// <param name="actorId">Actor's id.</param>
        /// <param name="orderedProductId">Ordered Product's id.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedDeleteOrderPayments(int actorId, int orderedProductId)
        {
            var result = await DeleteOrderPayments(actorId, orderedProductId);
            if (!result) return Failure("No deletion was proceeded.");

            return Success(result);
        }

        /// <summary>
        /// Creates a new Order Credit.
        /// </summary>
        /// <param name="actorId">Actor's id.</param>
        /// <param name="projectId">Project's id.</param>
        /// <param name="userId">User's id.</param>
        /// <param name="amount">Credited amount.</param>
        /// <returns></returns>
        public async Task<GuardResult> GuardedCreateOrderCredit(int actorId, int projectId, int userId, int amount)
        {
            var doesProjectExist =
                await ProjectService.GuardedGet(projectId);

            if (doesProjectExist.Code == Status.Failure) return Failure(doesProjectExist.Info);

            var date = DateTime.Now;
            var result = await CreateOrderCredit(actorId, projectId, userId, amount, date.AddHours(1));
            if (result == 0) return Failure("Error in creation process.");

            return Success(result);
        }

        // --------------------------------------------------------------------------------------------

        private async Task<BasicDataOrderFinalDue> GetOrderFinalDue(int orderFinalDue)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderFinalDueTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderFinalDue>(
                        "SELECT * FROM FRK.tOrderFinalDue WHERE OrderFinalDueId = @id",
                        new { id = orderFinalDue }
                    );
            }
        }

        private async Task<DetailedDataOrderFinalDue> GetFinalDueFromOrder(int orderId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                var info = await ctx[OrderFinalDueTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderFinalDue>(
                        "SELECT * FROM FRK.tOrderFinalDue WHERE OrderId = @id",
                        new { id = orderId }
                    );

                var payments = await ctx[OrderPaymentTable].Connection
                    .QueryAsync<BasicDataOrderPayment>(
                        "SELECT * FROM FRK.tOrderPayment WHERE OrderFinalDueId = @id",
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

        private async Task<bool> UpdateFinalDue(int orderFinalDueId, int amount)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderFinalDueTable.Update(ctx, 0, orderFinalDueId, amount);
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
                            FRK.tOrderPayment
                        WHERE
                            OrderId = @id",
                        new { id = orderId }
                    );
            }
        }

        private async Task<BasicDataOrderPayment> GetSingleOrderPaymentFromOrderedProduct(int orderedProductId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderPaymentTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataOrderPayment>(
                        "SELECT * FROM FRK.tOrderPayment WHERE OrderedProductId = @id",
                        new { id = orderedProductId }
                    );
            }
        }

        private async Task<IEnumerable<BasicDataOrderPayment>> GetAllOrderPaymentsFromOrderedProduct(int orderedProductId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderPaymentTable].Connection
                    .QueryAsync<BasicDataOrderPayment>(
                        "SELECT * FROM FRK.tOrderPayment WHERE OrderedProductId = @id",
                        new { id = orderedProductId }
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
                            FRK.tOrderPayment
                        WHERE
                            OrderPaymentId = @id",
                        new { id = orderPaymentId }
                    );
            }
        }

        private async Task<bool> DeleteOrderPayments(int userId, int orderedProductId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderPaymentTable.Delete(ctx, userId, orderedProductId);
            }
        }

        private async Task<int> CreateOrderCredit(int actorId, int projectId, int userId, int amount, DateTime moment)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await OrderCreditTable.Create(ctx, actorId, projectId, userId, amount, moment);
            }
        }

        private async Task<BasicDataUserBalance> GetUserBalance(int userBalanceId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserBalanceTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataUserBalance>(
                        "SELECT * FROM FRK.tUserBalance WHERE UserBalanceId = @id",
                        new { id = userBalanceId }
                    );
            }
        }

        private async Task<IEnumerable<BasicDataUserBalance>> GetAllUserBalanceFromProject(int projectId)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserBalanceTable].Connection
                    .QueryAsync<BasicDataUserBalance>(
                        "SELECT * FROM FRK.vUserBalance WHERE ProjectId = @id;",
                        new { id = projectId }
                    );
            }
        }

        private async Task<BasicDataUserBalance> GetUserBalance(UserBalanceGettingViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[UserBalanceTable].Connection
                    .QueryFirstOrDefaultAsync<BasicDataUserBalance>(
                        "SELECT * FROM FRK.vUserBalance WHERE UserId = @uId AND ProjectId = @pId;",
                        new { uId = model.UserId, pId = model.ProjectId }
                    );
            }
        }

        private async Task<IEnumerable<BasicDataOrderCredit>> GetUserCreditsFromProject(UserCreditGettingViewModel model)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await ctx[OrderCreditTable].Connection
                    .QueryAsync<BasicDataOrderCredit>(
                        "SELECT * FROM FRK.tOrderCredit WHERE UserId = @uId AND ProjectId = @pId;",
                        new { uId = model.UserId, pId = model.ProjectId }
                    );
            }
        }

        private async Task<bool> UpdateUserBalance(int userBalanceId, int amount)
        {
            using (var ctx = new SqlStandardCallContext())
            {
                return await UserBalanceTable.Update(ctx, 0, userBalanceId, amount);
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
