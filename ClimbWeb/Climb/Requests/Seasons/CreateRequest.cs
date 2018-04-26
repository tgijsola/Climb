using System;

namespace Climb.Requests.Seasons
{
    public class CreateRequest
    {
        public int LeagueID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}