﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Models;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Climb.Services.ModelServices
{
    public class LeagueService : ILeagueService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPointService pointService;

        public LeagueService(ApplicationDbContext dbContext, IPointService pointService)
        {
            this.dbContext = dbContext;
            this.pointService = pointService;
        }

        public async Task<League> Create(string name, int gameID)
        {
            if(!await dbContext.Games.AnyAsync(g => g.ID == gameID))
            {
                throw new NotFoundException(typeof(Game), gameID);
            }

            if(await dbContext.Leagues.AnyAsync(l => l.Name == name))
            {
                throw new ConflictException(typeof(League), nameof(League.Name), name);
            }

            var league = new League(gameID, name);

            dbContext.Add(league);
            await dbContext.SaveChangesAsync();

            return league;
        }

        public async Task<LeagueUser> Join(int leagueID, string userID)
        {
            if(!await dbContext.Leagues.AnyAsync(l => l.ID == leagueID))
            {
                throw new NotFoundException(typeof(League), leagueID);
            }

            if(!await dbContext.Users.AnyAsync(u => u.Id == userID))
            {
                throw new NotFoundException(typeof(ApplicationUser), userID);
            }

            var leagueUser = await dbContext.LeagueUsers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(lu => lu.UserID == userID);
            if(leagueUser != null)
            {
                leagueUser.HasLeft = false;
            }
            else
            {
                leagueUser = new LeagueUser(leagueID, userID);
                dbContext.Add(leagueUser);
            }

            await dbContext.SaveChangesAsync();

            return leagueUser;
        }

        public async Task<IReadOnlyList<RankSnapshot>> TakeSnapshots(int leagueID)
        {
            var league = await dbContext.Leagues
                .Include(l => l.Members)
                .FirstOrDefaultAsync(l => l.ID == leagueID);
            if(league == null)
            {
                throw new NotFoundException(typeof(League), leagueID);
            }

            IReadOnlyList<Set> playedSets = await dbContext.Sets.Where(s => s.LeagueID == leagueID && s.IsComplete && !s.IsLocked).ToArrayAsync();

            var pointsPerMember = new Dictionary<int, int>();
            CalculatePointDeltas();
            AssignPoints();
            UpdateRanks();
            var snapshots = CreateSnapshots();
            // TODO: King

            await dbContext.RankSnapshots.AddRangeAsync(snapshots);
            await dbContext.SaveChangesAsync();

            return snapshots;

            void CalculatePointDeltas()
            {
                foreach(var set in playedSets)
                {
                    var player1Won = set.WinnerID == set.Player1ID;
                    var (p1Points, p2Points) = pointService.CalculatePointDeltas(set.Player1.Points, set.Player2.Points, player1Won);
                    // TODO: beginner multiplier
                    if(!pointsPerMember.ContainsKey(set.Player1ID))
                    {
                        pointsPerMember.Add(set.Player1ID, 0);
                    }

                    if(!pointsPerMember.ContainsKey(set.Player2ID))
                    {
                        pointsPerMember.Add(set.Player2ID, 0);
                    }

                    pointsPerMember[set.Player1ID] += p1Points;
                    pointsPerMember[set.Player2ID] += p2Points;

                    set.IsLocked = true;
                }
            }

            void AssignPoints()
            {
                foreach(var member in league.Members)
                {
                    if(pointsPerMember.TryGetValue(member.ID, out var eloDelta))
                    {
                        member.Points += eloDelta;
                        member.Rank = league.Members.Count;
                    }
                }
            }

            void UpdateRanks()
            {
                var activeMembers = league.Members.Where(lu => pointsPerMember.ContainsKey(lu.ID)).OrderByDescending(lu => lu.Points).ToList();
                var rank = 0;
                var lastPoints = -1;
                for(var i = 0; i < activeMembers.Count; i++)
                {
                    var member = activeMembers[i];

                    if(member.Points != lastPoints)
                    {
                        lastPoints = member.Points;
                        rank = i + 1;
                    }

                    member.Rank = rank;
                }
            }

            IReadOnlyList<RankSnapshot> CreateSnapshots()
            {
                var createdDate = DateTime.Now;
                var rankSnapshots = new RankSnapshot[league.Members.Count];
                for(var i = 0; i < league.Members.Count; ++i)
                {
                    var member = league.Members[i];
                    RankSnapshot lastSnapshot = null;
                    if(member.RankSnapshots?.Count > 0)
                    {
                        lastSnapshot = member.RankSnapshots.MaxBy(rs => rs.CreatedDate);
                    }

                    var rankDelta = member.Rank - (lastSnapshot?.Rank ?? 0);
                    var eloDelta = member.Points - (lastSnapshot?.Points ?? 0);
                    var rankSnapshot = new RankSnapshot
                    {
                        LeagueUser = member,
                        Rank = member.Rank,
                        DeltaRank = rankDelta,
                        Points = member.Points,
                        DeltaPoints = eloDelta,
                        CreatedDate = createdDate
                    };
                    rankSnapshots[i] = rankSnapshot;
                }

                return rankSnapshots;
            }
        }
    }
}