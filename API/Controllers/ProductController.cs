using API.Services.Helper.Guard;
using API.Services.Product;
using Fork.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.Services.Auth;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private APIGuard Guard { get; }

        public AuthCheckService AuthCheckService { get; set; }

        private ProductService Service { get; }

        public ProductController(AuthCheckService aCService, ProductService service)
        {
            Guard = new APIGuard();
            AuthCheckService = aCService;
            Service = service;
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

        [HttpGet("i/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var isAuthenticated =
                AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var check =
                Guard.IsAdmissible(nameof(productId), productId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedGet(productId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpPost("n")]
        public async Task<IActionResult> GetByName([FromBody] ProductNameGettingViewModel product)
        {
            var isAuthenticated =
                AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            var check =
                Guard.IsAdmissible(nameof(product.Name), product.Name);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedGetByName(product.Name);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreationViewModel model)
        {
            var isAuthenticated =
                AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

            Dictionary<string, string> modelStrAnalysis = new Dictionary<string, string>
            {
                { nameof(model.Name), model.Name },
                { nameof(model.Desc), model.Desc }
            };
            var check = Guard.IsAdmissible(modelStrAnalysis);

            if (check.Code == Status.Success)
            {
                var isUserIsWhoHeSaidHeWas =
                    AuthCheckService.CheckCurrentUserIdentity(HttpContext, model.UserId);
                if (isUserIsWhoHeSaidHeWas.Code == Status.Failure) return Forbid();

                var result = await Service.GuardedCreate(model);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateViewModel model)
        {
            var isAuthenticated =
               AuthCheckService.CheckUserAuthenticationLevel(HttpContext);
            if (isAuthenticated.Code == Status.Failure) return Forbid();

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
                    var isUserIsWhoHeSaidHeWas =
                        AuthCheckService.CheckCurrentUserIdentity(HttpContext, model.UserId);
                    if (isUserIsWhoHeSaidHeWas.Code == Status.Failure) return Forbid();

                    var result = await Service.GuardedUpdate(model);
                    if (result.Code == Status.Failure) return BadRequest(result.Info);

                    return Ok(result.Content);
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }
    }
}
