using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Sets;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    public class SetService : ISetService
    {
        private readonly ApplicationDbContext dbContext;

        public SetService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Update(int setID, IReadOnlyList<MatchForm> matchForms)
        {
            var set = await dbContext.Sets
                .Include(s => s.Matches).ThenInclude(m => m.MatchCharacters)
                .FirstOrDefaultAsync(s => s.ID == setID);

            if(set.Matches.Count > 0)
            {
                dbContext.RemoveRange(set.Matches.SelectMany(m => m.MatchCharacters));
                dbContext.RemoveRange(set.Matches);
                await dbContext.SaveChangesAsync();
                set.Matches.Clear();
            }

            dbContext.Sets.Update(set);

            for(var i = 0; i < matchForms.Count; i++)
            {
                var matchForm = matchForms[i];
                var match = new Match(setID, i, matchForm.Player1Score, matchForm.Player2Score, matchForm.StageID);
                dbContext.Matches.Add(match);

                for(var j = 0; j < matchForm.Player1Characters.Length; j++)
                {
                    AddCharacter(match, matchForm.Player1Characters[j], set.Player1ID);
                    AddCharacter(match, matchForm.Player2Characters[j], set.Player2ID);
                }
            }

            await dbContext.SaveChangesAsync();

            void AddCharacter(Match match, int characterID, int playerID)
            {
                var character = new MatchCharacter(match.ID, characterID, playerID);
                dbContext.MatchCharacters.Add(character);
            }
        }
    }
}