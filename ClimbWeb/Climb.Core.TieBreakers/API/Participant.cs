using System;
using System.Collections.Generic;

namespace Climb.Core.TieBreakers
{
    public class Participant : IComparable<Participant>
    {
        private readonly Dictionary<int, int> beatenOpponents = new Dictionary<int, int>();

        public int UserID { get; }
        public int LeaguePoints { get; }
        public int SeasonPoints { get; }
        public DateTime JoinDate { get; }
        public int Wins { get; private set; }
        public decimal TieBreakerPoints { get; internal set; }

        public Participant(int userID, int leaguePoints, int seasonPoints, DateTime joinDate)
        {
            UserID = userID;
            LeaguePoints = leaguePoints;
            SeasonPoints = seasonPoints;
            JoinDate = joinDate;
        }

        public int CompareTo(Participant other)
        {
            int seasonPointsCompare = other.SeasonPoints.CompareTo(SeasonPoints);
            return seasonPointsCompare != 0 ? seasonPointsCompare : other.TieBreakerPoints.CompareTo(TieBreakerPoints);
        }

        public void AddWin(int opponentID)
        {
            ++Wins;
            if(beatenOpponents.ContainsKey(opponentID))
            {
                beatenOpponents[opponentID]++;
            }
            else
            {
                beatenOpponents[opponentID] = 1;
            }
        }

        public int TimesBeatenOpponent(int opponentID)
        {
            beatenOpponents.TryGetValue(opponentID, out var wins);
            return wins;
        }
    }
}