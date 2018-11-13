using API.Services.Helper.Guard;
using API.Services.Product;
using ITI.Human.ViewModels.Storage.LinkedProduct;
using ITI.Human.ViewModels.Storage;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    public class StorageController : Controller
    {
        public APIGuard Guard { get; }

        public StorageService Service { get; }

        public StorageController(StorageService service)
        {
            Guard = new APIGuard();
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStorages()
            => Ok((await Service.GetAllStorages()).Content);

        [HttpGet("i/{storageId}")]
        public async Task<IActionResult> GetStorage(int storageId)
        {
            var check =
                Guard.IsAdmissible(nameof(storageId), storageId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GetStorage(storageId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetStorageFromProject(int projectId)
        {
            var check =
                Guard.IsAdmissible(nameof(projectId), projectId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GetStorageFromProject(projectId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStorage(
            [FromBody] ITI.Human.ViewModels.Storage.CreationViewModel model)
        {
            if (model.ProjectId == 0) return BadRequest();

            var check1 =
                Guard.IsAdmissible(nameof(model.ProjectId), model.ProjectId);

            if (check1.Code == Status.Success)
            {
                var result = await Service.CreateStorage(model);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check1.Info);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllStorageLinkedProducts()
            => Ok(await Service.GetAllStorageLinkedProducts());

        [HttpGet("products/from/{storageId}")]
        public async Task<IActionResult> GetProductsFromStorage(int storageId)
        {
            var check =
                Guard.IsAdmissible(nameof(storageId), storageId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GetAllStorageLinkedProductsFromStorage(storageId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }
            return BadRequest(check.Info);
        }

        [HttpPost("products/create")]
        public async Task<IActionResult> CreateStorageLinkedProduct(
            [FromBody] ITI.Human.ViewModels.Storage.LinkedProduct.CreationViewModel model)
        {
            if (model.StorageId == 0 || model.ProductId == 0) return BadRequest();

            Dictionary<string, int> modelIntAnalysis = new Dictionary<string, int>
            {
                { nameof(model.StorageId), model.StorageId },
                { nameof(model.ProductId), model.ProductId },
                { nameof(model.Stock), model.Stock }
            };
            var check1 =
                Guard.IsAdmissible(modelIntAnalysis);

            if (check1.Code == Status.Success)
            {
                var check2 =
                    Guard.IsAdmissible(nameof(model.UnitPrice), model.UnitPrice);

                if (check2.Code == Status.Success)
                {
                    var result = await Service.CreateLinkedProduct(model);
                    if (result.Code == Status.Failure) return BadRequest(result.Info);

                    return Ok(result.Content);
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }

        [HttpPut("products/update")]
        public async Task<IActionResult> UpdateStorageLinkedProduct([FromBody] UpdateViewModel model)
        {
            if (model == null) return BadRequest();

            var check1 =
                Guard.IsAdmissible(nameof(model.UnitPrice), model.UnitPrice);

            if (check1.Code == Status.Success && model.UnitPrice != 0 && model.Stock != 0)
            {
                Dictionary<string, int> intAnalysis = new Dictionary<string, int>
                {
                    { nameof(model.StorageLinkedProductId), model.StorageLinkedProductId },
                    { nameof(model.Stock), model.Stock }
                };
                var check2 =
                    Guard.IsAdmissible(intAnalysis);

                if (check2.Code == Status.Success)
                {
                    var result = await Service.UpdateLinkedProduct(model);
                    if (result.Code == Status.Failure) return BadRequest(result.Info);

                    return Ok(result.Content);
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }
    }
}
