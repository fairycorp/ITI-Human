using API.Services.Classroom;
using API.Services.Helper.Guard;
using ITI.Human.ViewModels.Classroom;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ClassroomController : Controller
    {
        public APIGuard Guard { get; }

        public ClassroomService ClassroomService { get; }

        public ClassroomController(ClassroomService service)
        {
            Guard = new APIGuard();
            ClassroomService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok((await ClassroomService.GuardedGetAll()).Content);

        [HttpGet("{classroomId}")]
        public async Task<IActionResult> GetClassroomNameById(int classroomId)
        {
            var check =
                Guard.IsAdmissible(nameof(classroomId), classroomId);

            if (check.Code == Status.Success)
            {
                var result = await ClassroomService.GuardedGet(classroomId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }
    }
}
