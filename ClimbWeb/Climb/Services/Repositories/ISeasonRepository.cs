﻿using System;
using System.Threading.Tasks;
using Climb.Models;

namespace Climb.Services.Repositories
{
    public interface ISeasonRepository
    {
        Task<Season> Create(int leagueID, DateTime start, DateTime end);
    }
}