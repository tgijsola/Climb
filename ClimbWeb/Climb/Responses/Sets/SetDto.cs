using System;
using System.ComponentModel.DataAnnotations;
using Climb.Models;

namespace Climb.Responses.Sets
{
    public class SetDto
    {
        public readonly int id;
        public readonly int leagueID;
        public readonly int? seasonID;
        public readonly int gameID;
        public readonly int player1ID;
        public readonly int player2ID;
        public readonly int? player1Score;
        public readonly int? player2Score;
        public readonly DateTime dueDate;
        public readonly DateTime? updatedDate;
        [Required]
        public readonly MatchDto[] matches;

        private SetDto(Set set, MatchDto[] matches, int gameID)
        {
            id = set.ID;
            leagueID = set.LeagueID;
            seasonID = set.SeasonID;
            player1ID = set.Player1ID;
            player2ID = set.Player2ID;
            player1Score = set.Player1Score;
            player2Score = set.Player2Score;
            dueDate = set.DueDate;
            updatedDate = set.UpdatedDate;
            this.matches = matches;
            this.gameID = gameID;
        }

        public static SetDto Create(Set set, int gameID)
        {
            var matches = new MatchDto[set.Matches.Count];
            for(var i = 0; i < matches.Length; i++)
            {
                matches[i] = new MatchDto(set.Matches[i], set.Player1ID);
            }

            return new SetDto(set, matches, gameID);
        }
    }
}