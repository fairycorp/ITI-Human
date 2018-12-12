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

        public UserReferenceTooltipService TooltipService { get; set; }

        public UserService UserService { get; }

        public UserController(UserReferenceTooltipService uRTService, UserService service)
        {
            Guard = new APIGuard();
            UserService = service;
            TooltipService = uRTService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await UserService.GuardedGetAll();
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
                var result = await UserService.GuardedGet(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpGet("tooltip/{userId}")]
        public async Task<IActionResult> GetReferenceTooltip(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
            {
                var result = await TooltipService.GuardedGet(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }
    }
}
