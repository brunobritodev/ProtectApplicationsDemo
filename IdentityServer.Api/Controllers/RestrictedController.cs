// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Controllers
{
    [ApiController]
    [Route("restricted")]
    public class RestrictedController : Controller
    {

        [Authorize, HttpGet]
        public IActionResult Index()
        {
            return Ok("Must logged in to see this info");
        }

        [Authorize(Roles = "Managers"), HttpGet("role-based")]
        public IActionResult RoleBased()
        {
            return Ok("Only manager can see");
        }

        [Authorize(Policy = "ManagerOnly"), HttpGet("claim-based")]
        public IActionResult ClaimBased()
        {
            return Ok("Must have claim EmployeeFuction and value Manager");
        }

        [Authorize(Policy = "ManagerFromSalesDepartment"), HttpGet("policy-based")]
        public IActionResult PolicyBased()
        {
            return Ok("Must have claim EmployeeFuction and value Manager and from Sales Department");
        }

    }
}