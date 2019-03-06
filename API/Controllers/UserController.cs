using API.Services.Auth;
using API.Services.Helper.Guard;
using API.Services.User;
using Fork.ViewModels.User.Profile;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        public APIGuard Guard { get; }

        public AuthCheckService AuthCheckService { get; set; }

        public UserReferenceTooltipService TooltipService { get; set; }

        public UserService UserService { get; }

        public UserController(AuthCheckService aCService, 
            UserReferenceTooltipService uRTService, UserService service)
        {
            Guard = new APIGuard();
            AuthCheckService = aCService;
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

        [HttpGet("tooltip")]
        public async Task<IActionResult> GetAllReferenceTooltips()
        {
            var result = await TooltipService.GuardedGetAll();
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
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

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
            {
                var result = await UserService.GuardedGetProfile(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }

        [HttpGet("setup/{userId}")]
        public async Task<IActionResult> GetProfileSetupCompletedState(int userId)
        {
            var isAuthenticated =
                AuthCheckService.CheckUserAuthenticationLevel(HttpContext);

            var check =
                Guard.IsAdmissible(nameof(userId), userId);
            if (check.Code == Status.Failure) return BadRequest(check.Info);

            if (check.Code == Status.Success)
            {
                var isUserIsWhoHeSaidHeWas =
                    AuthCheckService.CheckCurrentUserIdentity(HttpContext, userId);
                if (isUserIsWhoHeSaidHeWas.Code == Status.Failure) return Forbid();

                var result = await UserService.GuardedGetProfileSetupCompletedState(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }

        [HttpPost("setup")]
        public async Task<IActionResult> SetupProfile([FromBody] CreationViewModel model)
        {
            var isAuthenticated =
                AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var intAnalysis = new Dictionary<string, int>
            {
                { nameof(model.UserId), model.UserId },
                { nameof(model.SchoolStatusId), model.SchoolStatusId },
                { nameof(model.SemesterId), model.SemesterId },
            };
            var check1 =
                Guard.IsAdmissible(intAnalysis);

            if (check1.Code == Status.Success)
            {
                var strAnalysis = new Dictionary<string, string>
                {
                    { nameof(model.Firstname), model.Firstname },
                    { nameof(model.Lastname), model.Lastname },
                };
                var check2 =
                    Guard.IsAdmissible(strAnalysis);

                if (check2.Code == Status.Success)
                {
                    var isUserIsWhoHeSaidHeWas =
                        AuthCheckService.CheckCurrentUserIdentity(HttpContext, model.UserId);
                    if (isUserIsWhoHeSaidHeWas.Code == Status.Failure) return Forbid();

                    var result = await UserService.GuardedSetupProfile(model);
                    if (result.Code == Status.Failure) return BadRequest(result.Info);

                    var finalResult = await UserService.GuardedGetProfileSetupCompletedState(model.UserId);
                    if ((bool)finalResult.Content == false) return BadRequest();

                    return Ok(finalResult.Content);
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }
    }
}
