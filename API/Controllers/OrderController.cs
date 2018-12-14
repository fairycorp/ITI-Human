using API.Services.Auth;
using API.Services.Helper.Guard;
using API.Services.Order;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Order.Payment;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        public APIGuard Guard { get; }

        public AuthCheckService AuthCheckService { get; set; }

        public OrderService OrderService { get; }

        public OrderDueServices OrderDueServices { get; }

        public OrderedProductService OrderedProductService { get; set; }

        public OrderController(AuthCheckService aCService, OrderService oService, OrderDueServices oDServices, OrderedProductService oPService)
        {
            Guard = new APIGuard();
            AuthCheckService = aCService;
            OrderService = oService;
            OrderDueServices = oDServices;
            OrderedProductService = oPService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await OrderService.GuardedGetAll();
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllDetailedOrdersOfUser(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
            {
                var result = await OrderService.GuardedGetFromUser(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetAllDetailedOrdersFromProject(int projectId)
        {
            var check =
                Guard.IsAdmissible(nameof(projectId), projectId);

            if (check.Code == Status.Success)
            {
                var result = await OrderService.GuardedGetFromProject(projectId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpGet("i/{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            var check =
                Guard.IsAdmissible(nameof(orderId), orderId);

            if (check.Code == Status.Success)
            {
                var result = await OrderService.GuardedGet(orderId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpGet("d/{orderId}")]
        public async Task<IActionResult> GetDetailedOrderFinalDue(int orderId)
        {
            var check =
                Guard.IsAdmissible(nameof(orderId), orderId);

            if (check.Code == Status.Success)
            {
                var result = await OrderDueServices.GuardedGetFinalDueFromOrder(orderId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreationViewModel model)
        {
            if (model != null)
            {
                var modelBasicIntAnalyis = new Dictionary<string, int>
                {
                    { nameof(model.UserId), model.UserId },
                    { nameof(model.ClassroomId), model.ClassroomId }
                };
                var basicCheck =
                    Guard.IsAdmissible(modelBasicIntAnalyis);

                if (basicCheck.Code == Status.Success)
                {
                    Dictionary<string, int> modelIntAnalysis = new Dictionary<string, int>();
                    var floor = 0;
                    foreach (var product in model.Products)
                    {
                        modelIntAnalysis.Add(BuildKey(nameof(product.StorageLinkedProductId), floor), product.StorageLinkedProductId);
                        modelIntAnalysis.Add(BuildKey(nameof(product.Quantity), floor), product.Quantity);
                        floor++;
                    }
                    var check =
                        Guard.IsAdmissible(modelIntAnalysis);

                    if (check.Code == Status.Success)
                    {
                        var result = await OrderService.GuardedCreate(model);
                        if (result.Code == Status.Failure) return BadRequest(result.Info);

                        return Ok(result.Content);
                    }
                    return BadRequest(check.Info);
                }
                return BadRequest(basicCheck.Info);
            }
            return BadRequest();
        }

        [HttpPut("paymentState")]
        public async Task<IActionResult> UpdatePaymentState([FromBody] IEnumerable<PaymentStateUpdateViewModel> models)
        {
            if (models != null)
            {
                var results = new List<object>();
                var floor = 0;
                foreach (var model in models)
                {
                    var modelIntAnalysis = new Dictionary<string, int>
                    {
                        { BuildKey(nameof(model.OrderedProductId), floor), model.OrderedProductId },
                        { BuildKey(nameof(model.PaymentState.State), floor), (int)model.PaymentState.State },
                        { BuildKey(nameof(model.PaymentState.Amount), floor), model.PaymentState.Amount }
                    };
                    var check =
                        Guard.IsAdmissible(modelIntAnalysis);

                    if (check.Code == Status.Success)
                    {
                        var result = await OrderedProductService.GuardedUpdatePaymentState(
                            model.UserId, model.OrderedProductId, model.PaymentState.State, model.PaymentState.Amount
                        );
                        if (result.Code == Status.Failure) results.Add(result.Info);
                        else results.Add(result.Content);
                    }
                    else return BadRequest(check.Info);
                    floor++;
                }
                return Ok(results);
            }
            return BadRequest();
        }

        [HttpPut("currentState")]
        public async Task<IActionResult> UpdateCurrentState([FromBody] IEnumerable<CurrentStateUpdateViewModel> models)
        {
            if (models != null)
            {
                var results = new List<object>();
                var floor = 0;
                foreach (var model in models)
                {
                    var modelIntAnalysis = new Dictionary<string, int>
                    {
                        { BuildKey(nameof(model.OrderedProductId), floor), model.OrderedProductId },
                        { BuildKey(nameof(model.CurrentState), floor), (int)model.CurrentState },
                    };
                    var check =
                        Guard.IsAdmissible(modelIntAnalysis);

                    if (check.Code == Status.Success)
                    {
                        var result = await OrderedProductService.GuardedUpdateCurrentState(
                            model.OrderedProductId, model.CurrentState
                        );
                        if (result.Code == Status.Failure) results.Add(result.Info);
                        else results.Add(result.Content);
                    }
                    else return BadRequest(check.Info);
                    floor++;
                }
                return Ok(results);
            }
            return BadRequest();
        }

        private static string BuildKey(string name, int index)
            => string.Format("{0} (at floor {1})", name, index);

    }
}
