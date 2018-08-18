using System;
using System.Collections.Generic;
using Climb.Models;

namespace Climb.Services
{
    public class RoundRobinScheduler : ScheduleFactory
    {
        protected override HashSet<Set> GenerateScheduleInternal(Season season)
        {
            var participants = season.Participants.ToArray();
            var sets = new HashSet<Set>(participants.Length);
            for(var i = 0; i < participants.Length - 1; i++)
            {
                var player1 = participants[i];
                for(var j = i + 1; j < participants.Length; j++)
                {
                    var player2 = participants[j];
                    var dueDate = DateTime.Now;
                    var set = new Set{
                        LeagueID = season.LeagueID,
                        SeasonID = season.ID,
                        DueDate = dueDate,
                        Player1ID = player1.LeagueUserID,
                        Player2ID = player2.LeagueUserID,
                        SeasonPlayer1ID = player1.ID,
                        SeasonPlayer2ID = player2.ID,
                    };
                    sets.Add(set);
                }
            }

            return sets;
        }
    }
}