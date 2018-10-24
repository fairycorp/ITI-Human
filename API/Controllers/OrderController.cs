using API.Services.Order;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
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
            => Ok((await Service.GetAllDetailedOrders()).Content);

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllDetailedOrdersOfUser(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
                return Ok((await Service.GetDetailedOrdersOfUser(userId)).Content);

            return BadRequest(check.Info);
        }

        [HttpGet("i/{orderId}")]
        public async Task<IActionResult> GetDetailedOrder(int orderId)
        {
            var check =
                Guard.IsAdmissible(nameof(orderId), orderId);

            if (check.Code == Status.Success)
                return Ok((await Service.GetDetailedOrder(orderId)).Content);

            return BadRequest(check.Info);
        }
    }
}
