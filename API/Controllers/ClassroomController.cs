using API.Services.Classroom;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ClassroomController : Controller
    {
        private ClassroomService Service { get; set; }

        public ClassroomController(ClassroomService cService)
        {
            Service = cService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Service.GuardedGetAll();
            if (result.Code == Status.Failure) return BadRequest(result.Info);

            return Ok(result.Content);
        }
    }
}
