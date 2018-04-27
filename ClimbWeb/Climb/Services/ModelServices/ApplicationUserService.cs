﻿using System.Threading.Tasks;
using Climb.Data;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext dbContext;

        public ApplicationUserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}