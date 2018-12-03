using API.Services.Project;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        public ProjectService Service { get; set; }

        public ProjectController(ProjectService pService)
        {
            Service = pService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok((await Service.GuardedGetAll()).Content);
    }
}
