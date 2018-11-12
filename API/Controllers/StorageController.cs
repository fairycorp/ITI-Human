using API.Services.Helper.Guard;
using API.Services.Product;
using ITI.Human.ViewModels.Storage.LinkedProduct;
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

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateViewModel model)
        {
            if (model == null) return BadRequest();

            var check1 =
                Guard.IsAdmissible(model.UnitPrice);

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
                    return Ok(await Service.UpdateLinkedProduct(model));
                }
                return BadRequest(check2.Info);
            }
            return BadRequest(check1.Info);
        }
    }
}
