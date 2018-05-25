using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
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
            if(!await dbContext.Games.AnyAsync(g => g.ID == gameID))
            {
                throw new NotFoundException(typeof(Game), gameID);
            }

            if(await dbContext.Leagues.AnyAsync(l => l.Name == name))
            {
                throw new ConflictException(typeof(League), nameof(League.Name), name);
            }

            var league = new League(gameID, name);

            dbContext.Add(league);
            await dbContext.SaveChangesAsync();

            return league;
        }

        public async Task<LeagueUser> Join(int leagueID, string userID)
        {
            if(!await dbContext.Leagues.AnyAsync(l => l.ID == leagueID))
            {
                throw new NotFoundException(typeof(League), leagueID);
            }

            if(!await dbContext.Users.AnyAsync(u => u.Id == userID))
            {
                throw new NotFoundException(typeof(ApplicationUser), userID);
            }

            var leagueUser = await dbContext.LeagueUsers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(lu => lu.UserID == userID);
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