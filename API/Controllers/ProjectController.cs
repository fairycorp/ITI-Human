using API.Services.Auth;
using API.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        public AuthCheckService AuthCheckService { get; set; }

        public ProjectService Service { get; set; }

        public ProjectController(AuthCheckService aCService, ProjectService pService)
        {
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
    }
}
