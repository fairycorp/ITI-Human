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
            => Ok((await Service.GetAll()).Content);

        [HttpGet("i/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var check =
                Guard.IsAdmissible(nameof(productId), productId);

            if (check.Code == Status.Success)
                return Ok((await Service.GetById(productId)).Content);

            return BadRequest(check.Info);
        }

        [HttpPost("n")]
        public async Task<IActionResult> GetByName([FromBody] ProductNameGettingViewModel product)
        {
            var check =
                Guard.IsAdmissible(nameof(product.Name), product.Name);

            if (check.Code == Status.Success)
                return Ok((await Service.GetByName(product.Name)).Content);

            return BadRequest(check.Info);
        }
    }
}
