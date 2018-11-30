using System;
using API.Services.Helper.Guard;
using API.Services.Product;
using ITI.Human.ViewModels.Storage.LinkedProduct;
using ITI.Human.ViewModels.Storage;
using Microsoft.AspNetCore.Mvc;
using Stall.Guard.System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Services.User;

namespace API.Controllers
{
    [Route("[controller]")]
    public class UserController
    {
        public APIGuard Guard { get; }

        public UserService Service { get; }

        public UserController(UserService service)
        {
            Guard = new APIGuard();
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok((await Service.GetAllUsers()).Content);

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllDetailedOrdersOfUser(int userId)
        {
            var check =
                Guard.IsAdmissible(nameof(userId), userId);

            if (check.Code == Status.Success)
            {
                var result = await Service.GuardedGetDetailedOrdersFromUser(userId);
                if (result.Code == Status.Failure) return BadRequest(result.Info);

                return Ok(result.Content);
            }

            return BadRequest(check.Info);
        }


    }
}
