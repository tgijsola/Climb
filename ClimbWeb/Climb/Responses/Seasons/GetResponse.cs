using System;
using System.Collections.Generic;
using System.Linq;
using Climb.Models;
using JetBrains.Annotations;

namespace Climb.Responses.Seasons
{
    public class GetResponse
    {
        [UsedImplicitly]
        public int ID { get; }
        [UsedImplicitly]
        public int LeagueID { get; }
        [UsedImplicitly]
        public string LeagueName { get; }
        [UsedImplicitly]
        public int Index { get; }
        [UsedImplicitly]
        public DateTime StartDate { get; }
        [UsedImplicitly]
        public DateTime EndDate { get; }
        [UsedImplicitly]
        public HashSet<LeagueUser> Participants { get; }
        [UsedImplicitly]
        public HashSet<Set> Sets { get; }

        public GetResponse(Season season)
        {
            ID = season.ID;
            LeagueID = season.LeagueID;
            LeagueName = season.League.Name;
            Index = season.Index;
            StartDate = season.StartDate;
            EndDate = season.EndDate;
            Participants = season.Participants.Select(slu => slu.LeagueUser).ToHashSet();
            Sets = season.Sets;
        }
    }
}