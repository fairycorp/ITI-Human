using API.Services.Helper.Guard;
using API.Services.User;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        public APIGuard Guard { get; }

        public UserService Service { get; }

        public UserController(UserService service)
        {
            Guard = new APIGuard();
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Service.GuardedGetAll();
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedGet(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }
    }
}
