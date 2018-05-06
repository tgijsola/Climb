using Climb.Models;

namespace Climb.Responses.Seasons
{
    public class GetResponse
    {
        public readonly Season season;
        public readonly League league;

        public GetResponse(Season season)
        {
            this.season = season;
            this.league = season.League;
        }
    }
}