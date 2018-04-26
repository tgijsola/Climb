using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public class LeagueRepository : DbRepository<League>, ILeagueRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LeagueRepository(ApplicationDbContext dbContext)
            : base(dbContext.Leagues)
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

            dbSet.Add(league);
            await dbContext.SaveChangesAsync();

            return league;
        }
    }
}