using System;

namespace Climb.Models
{
    public class SetRequest
    {
        public int ID { get; set; }
        public int RequesterID { get; set; }
        public int ChallengedID { get; set; }
        public DateTime DateCreated { get; set; }

        public LeagueUser Requester { get; set; }
        public LeagueUser Challenged { get; set; }
    }
}
