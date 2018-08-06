using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext dbContext;

        public OrganizationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Organization> AddLeague(int organizationID, int leagueID)
        {
            var organization = await dbContext.Organizations
                .Include(o => o.Leagues)
                .FirstOrDefaultAsync(o => o.ID == organizationID);
            if(organization == null)
            {
                throw new NotFoundException(typeof(Organization), organizationID);
            }

            var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                throw new NotFoundException(typeof(League), leagueID);
            }

            if(league.OrganizationID != null)
            {
                return organization;
            }

            dbContext.Update(organization);
            organization.Leagues.Add(league);
            await dbContext.SaveChangesAsync();

            return organization;
        }
    }
}