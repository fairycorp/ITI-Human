﻿using API.Services.Auth;
using API.Services.Helper.Guard;
using API.Services.Project;
using Fork.ViewModels.Project;
using Fork.ViewModels.Project.Member;
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

        [HttpGet("m/{projectMemberId}")]
        public async Task<IActionResult> GetFromMember(int projectMemberId)
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var result = await Service.GuardedGetFromMember(projectMemberId);
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Fork.ViewModels.Project.CreationViewModel model)
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
                var result = await Service.GuardedCreate(model);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check1.Info);
        }

        [HttpPost("member/add")]
        public async Task<IActionResult> AddMember([FromBody] Fork.ViewModels.Project.Member.CreationViewModel model)
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

                return Ok(result.Content);
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
