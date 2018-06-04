﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Models;
using Climb.Requests.Games;
using Climb.Services.ModelServices;
using Climb.ViewModels.Games;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class GameController : BaseController<GameController>
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService, ApplicationDbContext dbContext, ILogger<GameController> logger, UserManager<ApplicationUser> userManager)
            : base(logger, userManager, dbContext)
        {
            this.gameService = gameService;
        }

        [HttpGet("games/home/{gameID:int}")]
        public async Task<IActionResult> Home(int gameID)
        {
            var user = await GetViewUserAsync();
            var game = await dbContext.Games
                .Include(g => g.Characters).AsNoTracking()
                .Include(g => g.Stages).AsNoTracking()
                .FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return NotFound();
            }

            var viewModel = new HomeViewModel(user, game);
            return View(viewModel);
        }

        [HttpGet("games/characters/add/{gameID:int}")]
        public async Task<IActionResult> CharacterAdd(int gameID)
        {
            var user = await GetViewUserAsync();
            var game = await dbContext.Games
                .FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return NotFound();
            }

            var viewModel = new CharacterAddViewModel(user, game);
            return View(viewModel);
        }

        [HttpPost("games/characters/add")]
        public async Task<IActionResult> CharacterAddPost(AddCharacterRequest request)
        {
            try
            {
                var character = await gameService.AddCharacter(request);
                return CodeResultAndLog(HttpStatusCode.Created, character, $"New character {character.Name} created.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("/api/v1/games/{gameID:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Game))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string))]
        public async Task<IActionResult> Get(int gameID)
        {
            var game = await dbContext.Games
                .Include(g => g.Characters).AsNoTracking()
                .Include(g => g.Stages).AsNoTracking()
                .FirstOrDefaultAsync(g => g.ID == gameID);
            if(game == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find Game with ID '{gameID}'.");
            }

            return CodeResult(HttpStatusCode.OK, game);
        }

        [HttpPost("/api/v1/games/create")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Game))]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), "Game name is taken.")]
        public async Task<IActionResult> Create(CreateRequest request)
        {
            try
            {
                var game = await gameService.Create(request);
                return CodeResultAndLog(HttpStatusCode.Created, game, "Game created.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpPost("/api/v1/addCharacter")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Character))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Could not find game.")]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), "Character name is taken.")]
        public async Task<IActionResult> AddCharacter(AddCharacterRequest request)
        {
            try
            {
                var character = await gameService.AddCharacter(request);
                return CodeResultAndLog(HttpStatusCode.Created, character, $"New character {character.Name} created.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpPost("/api/v1/addStage")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(Stage))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Could not find game.")]
        [SwaggerResponse(HttpStatusCode.Conflict, typeof(string), "Stage name is taken.")]
        public async Task<IActionResult> AddStage(AddStageRequest request)
        {
            try
            {
                var stage = await gameService.AddStage(request);
                return CodeResultAndLog(HttpStatusCode.Created, stage, $"New stage {stage.Name} created.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, request);
            }
        }

        [HttpGet("/api/v1/games")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<Game>))]
        public async Task<IActionResult> ListAll()
        {
            var games = await dbContext.Games.ToListAsync();

            return CodeResult(HttpStatusCode.OK, games);
        }
    }
}