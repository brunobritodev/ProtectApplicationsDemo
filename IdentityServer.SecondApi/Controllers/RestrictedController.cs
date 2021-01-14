// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Bogus;
using Bogus.Extensions;
using IdentityServer.SecondApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.SecondApi.Controllers
{
    [ApiController]
    [Route("restricted"), Authorize(Policy = "SalesEmployeeOnly")]
    public class RestrictedController : Controller
    {
        [HttpGet("my-sales")]
        public IActionResult MySales()
        {
            var faker = new Faker();
            var producs = faker.Make(faker.Random.Int(1, 10), () => faker.Commerce.Product());
            var orderIds = faker.Random.Int(1, 50000);
            var testOrders = new Faker<Order>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode
                .StrictMode(true)
                //OrderId is deterministic
                .RuleFor(o => o.OrderId, f => orderIds++)
                //Pick some fruit from a basket
                .RuleFor(o => o.Item, f => f.PickRandom(producs))
                //A random quantity from 1 to 10
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
                //A nullable int? with 80% probability of being null.
                //The .OrNull extension is in the Bogus.Extensions namespace.
                .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f))
                .RuleFor(o => o.Price, f => f.Commerce.Price());

            return Ok(testOrders.Generate(faker.Random.Int(1,10)));
        }

    }

}