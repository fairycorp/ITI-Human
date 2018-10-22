using API.Models.Product;
using API.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private StdGuard Guard { get; }
        private ProductService Service { get; }

        public ProductController(ProductService service)
        {
            Guard = new StdGuard();
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
            => Ok(await Service.GetAll());

        [HttpGet("id/{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            var check = 
                Guard.IsAdmissible(nameof(productId), productId);

            if (check.Code == Status.Success)
                return Ok(await Service.Get(productId));

            return BadRequest(check.Info);
        }
    }
}
