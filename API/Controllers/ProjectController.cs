using API.Services.Auth;
using API.Services.Helper.Guard;
using API.Services.Project;
using ITI.Human.ViewModels.Project;
using ITI.Human.ViewModels.Project.Member;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        public APIGuard Guard { get; set; }

        public AuthCheckService AuthCheckService { get; set; }

        public ProjectService Service { get; set; }

        public ProjectController(AuthCheckService aCService, ProjectService pService)
        {
            Guard = new APIGuard();
            AuthCheckService = aCService;
            Service = pService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var result = await Service.GuardedGetAll();
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
        }

        [HttpGet("u/{userId}")]
        public async Task<IActionResult> GetAllFromUser(int userId)
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var result = await Service.GuardedGetAllFromUser(userId);
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ITI.Human.ViewModels.Project.CreationViewModel model)
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var intAnalysis = new Dictionary<string, int>
            {
                { nameof(model.ActorId), model.ActorId },
                { nameof(model.SemesterId), model.SemesterId }
            };
            var check1 = Guard.IsAdmissible(intAnalysis);

            if (check1.Code == Status.Success)
            {
                var strAnalysis = new Dictionary<string, string>
                {
                    { nameof(model.Name), model.Name },
                    { nameof(model.Headline), model.Headline },
                    { nameof(model.Pitch), model.Pitch }
                };
                var check2 = Guard.IsAdmissible(strAnalysis);

                if (check2.Code == Status.Success)
                {
                    var result = await Service.GuardedCreate(model);
                    if (result.Code == Status.Failure) return BadRequest(result.Info);

                    return Ok(result.Content);
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }

        [HttpPost("member/add")]
        public async Task<IActionResult> AddMember([FromBody] ITI.Human.ViewModels.Project.Member.CreationViewModel model)
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var intAnalysis = new Dictionary<string, int>
            {
                { nameof(model.UserId), model.UserId },
                { nameof(model.ProjectId), model.ProjectId }
            };
            var check = Guard.IsAdmissible(intAnalysis);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedAddMember(model);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result);
            }
            return BadRequest(check.Info);
        }

        [HttpDelete("member/delete")]
        public async Task<IActionResult> RemoveMember([FromBody] DeletionViewModel model)
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var intAnalysis = new Dictionary<string, int>
            {
                { nameof(model.ActorId), model.ActorId },
                { nameof(model.ProjectMemberId), model.ProjectMemberId }
            };
            var check = Guard.IsAdmissible(intAnalysis);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedRemoveMember(model);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }
    }
}
