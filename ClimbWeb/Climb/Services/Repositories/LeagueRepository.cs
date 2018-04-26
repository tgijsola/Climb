using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LeagueRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<League> Create(string name, int gameID)
        {
            var league = new League
            {
                Name = name,
                GameID = gameID,
            };

            dbContext.Add(league);
            await dbContext.SaveChangesAsync();

            return league;
        }
    }
}