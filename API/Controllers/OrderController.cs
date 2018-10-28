using API.Services.Helper;
using API.Services.Order;
using API.ViewModels.Order;
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
        public async Task<IActionResult> GetAllDetailedOrders()
            => Ok((await Service.GuardedGetAllDetailedOrders()).Content);

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllDetailedOrdersOfUser(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
                return Ok((await Service.GuardedGetDetailedOrdersFromUser(userId)).Content);

            return BadRequest(check.Info);
        }

        [HttpGet("i/{orderId}")]
        public async Task<IActionResult> GetDetailedOrder(int orderId)
        {
            var check =
                Guard.IsAdmissible(nameof(orderId), orderId);

            if (check.Code == Status.Success)
                return Ok((await Service.GuardedGetDetailedOrder(orderId)).Content);

            return BadRequest(check.Info);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDetailedOrder(CreationViewModel model)
        {
            Dictionary<string, int> modelInfoAnalysis = new Dictionary<string, int>
            {
                { nameof(model.Info.OrderId), model.Info.OrderId },
                { nameof(model.Info.UserId), model.Info.UserId },
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
                        { nameof(product.ProductId), product.ProductId },
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
                            return Ok((await Service.UpdateDetailedOrder(model)).Content);
                        
                        return BadRequest(check3.Info);
                    }
                    return BadRequest(check2.Info);
                }
            }
            return BadRequest(check1.Info);
        }
    }
}
