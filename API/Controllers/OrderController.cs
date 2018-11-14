using API.Services.Order;
using ITI.Human.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        public StdGuard Guard { get; }

        public OrderService Service { get; }

        public OrderController(OrderService service)
        {
            Guard = new StdGuard();
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok((await Service.GuardedGetAllDetailedOrders()).Content);

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllDetailedOrdersOfUser(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedGetDetailedOrdersFromUser(userId);
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
                var result = await Service.GuardedGetDetailedOrder(orderId);
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
                    int idx = 0;
                    foreach (var product in model.Products)
                    {
                        idx++;
                        modelIntAnalysis.Add($"{nameof(product.StorageLinkedProductId)}-{idx}", product.StorageLinkedProductId);
                        modelIntAnalysis.Add($"{nameof(product.Quantity)}-{idx}", product.Quantity);
                    }
                    var check =
                        Guard.IsAdmissible(modelIntAnalysis);

                    if (check.Code == Status.Success)
                    {
                        var result = await Service.GuardedCreateDetailedOrder(model);
                        if (result.Code == Status.Failure) return BadRequest(result.Info);

                        return Ok(result.Content);
                    }
                    return BadRequest(check.Info);
                }
                return BadRequest(basicCheck.Info);
            }
            return BadRequest();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] DeliveryStateUpdateViewModel model)
        {
            if (!(model == null))
            {
                if (model.Info == null || model.Products == null)
                    return BadRequest();
            }
            else return BadRequest();

            Dictionary<string, int> modelInfoAnalysis = new Dictionary<string, int>
            {
                { nameof(model.Info.OrderId), model.Info.OrderId },
                { nameof(model.Info.UserId), model.Info.UserId },
                { nameof(model.Info.CurrentState), (int) model.Info.CurrentState }
            };
            var check1 = Guard.IsAdmissible(modelInfoAnalysis);

            if (check1.Code == Status.Success)
            {
                foreach (var product in model.Products)
                {
                    Dictionary<string, int> modelProductIntAnalysis = new Dictionary<string, int>
                    {
                        { nameof(product.OrderedProductId), product.OrderedProductId },
                        { nameof(product.OrderId), product.OrderId },
                        { nameof(product.StorageLinkedProductId), product.StorageLinkedProductId },
                    };
                    var check2 = Guard.IsAdmissible(modelProductIntAnalysis);

                    if (check2.Code == Status.Success)
                    {
                        Dictionary<string, string> modelProductStrAnalysis = new Dictionary<string, string>
                        {
                            { nameof(product.Name), product.Name },
                            { nameof(product.Desc), product.Desc }
                        };
                        var check3 = Guard.IsAdmissible(modelProductStrAnalysis);

                        if (check3.Code == Status.Success)
                        {
                            var result = await Service.UpdateDetailedOrderDeliveryState(model);
                            if (result.Code == Status.Failure) return BadRequest(result.Info);

                            return Ok(result.Content);
                        }

                        return BadRequest(check3.Info);
                    }
                    return BadRequest(check2.Info);
                }
            }
            return BadRequest(check1.Info);
        }
    }
}
