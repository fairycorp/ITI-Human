using API.Services.Helper.Guard;
using API.Services.Product;
using API.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private APIGuard Guard { get; }
        private ProductService Service { get; }

        public ProductController(ProductService service)
        {
            Guard = new APIGuard();
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreationViewModel model)
        {
            Dictionary<string, string> modelStrAnalysis = new Dictionary<string, string>
            {
                { nameof(model.Name), model.Name },
                { nameof(model.Desc), model.Desc }
            };
            var check1 = Guard.IsAdmissible(modelStrAnalysis);

            if (check1.Code == Status.Success)
            {
                var check2 = Guard.IsAdmissible(model.Price);

                if (check2.Code == Status.Success)
                {
                    return Ok((await Service.Create(model)).Content);
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }
    }
}
