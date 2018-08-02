﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Climb.ViewModels;
using Climb.ViewModels.Organizations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class OrganizationController : BaseController<OrganizationController>
    {
        public OrganizationController(ILogger<OrganizationController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
            : base(logger, userManager, dbContext)
        {
        }

        [HttpGet("organizations")]
        public async Task<IActionResult> Index()
        {
            var user = await GetViewUserAsync();
            var organizations = await dbContext.Organizations
                .Include(l => l.Owners).AsNoTracking()
                .Include(l => l.Members).AsNoTracking()
                .Include(l => l.Leagues).AsNoTracking()
                .ToArrayAsync();

            var viewModel = new IndexViewModel(user, organizations);
            return View(viewModel);
        }

        [HttpGet("organizations/home/{organizationID:int}")]
        public async Task<IActionResult> Home(int organizationID)
        {
            var user = await GetViewUserAsync();
            var organization = await dbContext.Organizations
                .Include(l => l.Leagues).AsNoTracking()
                .Include(l => l.Members).AsNoTracking()
                .FirstOrDefaultAsync(l => l.ID == organizationID);

            //var viewModel = new HomeViewModel(user, league);
            var viewModel = new BaseViewModel(user);

            return View(viewModel);
        }

        [HttpPost("organizations/update")]
        public async Task<IActionResult> UpdatePost(int? id, string name)
        {
            var user = await GetViewUserAsync();

            var organization = new Organization
            {
                Name = name,
                DateCreated = DateTime.Now,
                Owners = new List<ApplicationUser>(),
            };

            var owner = new OrganizationUser
            {
                Organization = organization,
                User = user,
            };

            dbContext.Add(organization);
            dbContext.Add(owner);
            await dbContext.SaveChangesAsync();

            dbContext.Update(organization);
            organization.Owners.Add(user);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Home", new {organizationID = organization.ID});
        }
    }
}