using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    public class LeagueService : ILeagueService
    {
        private readonly ApplicationDbContext dbContext;

        public LeagueService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<League> Create(string name, int gameID)
        {
            var league = new League(gameID, name);

            dbContext.Add(league);
            await dbContext.SaveChangesAsync();

            return league;
        }

        public async Task<LeagueUser> Join(int leagueID, string userID)
        {
            var leagueUser = await dbContext.LeagueUsers.FirstOrDefaultAsync(lu => lu.UserID == userID);
            if(leagueUser != null)
            {
                leagueUser.HasLeft = false;
            }
            else
            {
                leagueUser = new LeagueUser(leagueID, userID);
                dbContext.Add(leagueUser);
            }


            await dbContext.SaveChangesAsync();

            return leagueUser;
        }
    }
}