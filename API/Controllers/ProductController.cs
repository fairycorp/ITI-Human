using API.Services.Helper.Guard;
using API.Services.Product;
using ITI.Human.ViewModels.Product;
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
                return Ok((await Service.GetProductById(productId)).Content);

            return BadRequest(check.Info);
        }

        [HttpPost("n")]
        public async Task<IActionResult> GetByName([FromBody] ProductNameGettingViewModel product)
        {
            var check =
                Guard.IsAdmissible(nameof(product.Name), product.Name);

            if (check.Code == Status.Success)
                return Ok((await Service.GetProductByName(product.Name)).Content);

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
            var check = Guard.IsAdmissible(modelStrAnalysis);

            if (check.Code == Status.Success)
            {
                return Ok((await Service.CreateProduct(model)).Content);
            }
            return BadRequest(check.Info);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateViewModel model)
        {
            var check1 = 
                Guard.IsAdmissible(nameof(model.ProductId), model.ProductId);

            if (check1.Code == Status.Success)
            {
                Dictionary<string, string> modelStrAnalysis = new Dictionary<string, string>
                {
                    { nameof(model.Name), model.Name },
                    { nameof(model.Desc), model.Desc }
                };
                var check2 = Guard.IsAdmissible(modelStrAnalysis);

                if (check2.Code == Status.Success)
                {
                    return Ok(await Service.Updateproduct(model));
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }
    }
}
