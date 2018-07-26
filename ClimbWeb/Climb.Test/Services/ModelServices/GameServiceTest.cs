using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Requests.Games;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Services.ModelServices
{
    [TestFixture]
    public class GameServiceTest
    {
        private GameService testObj;
        private ApplicationDbContext dbContext;
        private ICdnService cdnService;

        [SetUp]
        public void SetUp()
        {
            dbContext = DbContextUtility.CreateMockDb();
            cdnService = Substitute.For<ICdnService>();

            testObj = new GameService(dbContext, cdnService);
        }

        [Test]
        public async Task Create_Valid_ReturnGame()
        {
            var request = new CreateRequest("GameName", 1, 2, true);

            var game = await testObj.Create(request);

            Assert.IsNotNull(game);
        }

        [Test]
        public void Create_NameTaken_BadRequestException()
        {
            var game = GameUtility.Create(dbContext, 1, 1);
            var request = new CreateRequest(game.Name, 1, 2, true);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Create_InvalidMaxCharacters_BadRequestException(int maxCharacters)
        {
            var request = new CreateRequest("GameName", maxCharacters, 2, true);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Create_InvalidMaxMatchPoints_BadRequestException(int maxPoints)
        {
            var request = new CreateRequest("GameName", 1, maxPoints, true);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [Test]
        public void Create_NameTaken_BadRequest()
        {
            var game = GameUtility.Create(dbContext, 0, 0);
            var request = new CreateRequest(game.Name, 1, 1, true);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.Create(request));
        }

        [Test]
        public async Task AddCharacter_Valid_Character()
        {
            var game = GameUtility.Create(dbContext, 0, 0);
            var imageFile = Substitute.For<IFormFile>();

            var character = await testObj.AddCharacter(game.ID, null, "Char1", imageFile);

            Assert.IsNotNull(character);
        }

        [Test]
        public void AddCharacter_NoGame_NotFoundException()
        {
            var imageFile = Substitute.For<IFormFile>();

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddCharacter(0, null, "Char1", imageFile));
        }

        [Test]
        public void AddCharacter_NameTaken_ConflictException()
        {
            var game = GameUtility.Create(dbContext, 1, 0);
            var imageFile = Substitute.For<IFormFile>();

            Assert.ThrowsAsync<ConflictException>(() => testObj.AddCharacter(game.ID, null, game.Characters[0].Name, imageFile));
        }

        [Test]
        public void AddCharacter_NewCharacterNoImageKey_NullArgumentException()
        {
            var game = GameUtility.Create(dbContext, 0, 0);

            Assert.ThrowsAsync<ArgumentNullException>(() => testObj.AddCharacter(game.ID, null, "Char1", null));
        }

        [Test]
        public async Task AddCharacter_NewCharacter_UploadImage()
        {
            var game = GameUtility.Create(dbContext, 1, 0);
            var imageFile = Substitute.For<IFormFile>();

            await testObj.AddCharacter(game.ID, null, "Char1", imageFile);

            await cdnService.Received(1).UploadImageAsync(imageFile, ClimbImageRules.CharacterPic);
        }

        [Test]
        public async Task AddCharacter_OldCharacterNoImage_ImageKeyNotUpdated()
        {
            var game = GameUtility.Create(dbContext, 1, 0);
            var imageKey = game.Characters[0].ImageKey;

            var character = await testObj.AddCharacter(game.ID, game.Characters[0].ID, "Char1", null);

            Assert.AreEqual(imageKey, character.ImageKey);
        }

        [Test]
        public async Task AddCharacter_OldCharacterNewImage_OldImageDeleted()
        {
            var game = GameUtility.Create(dbContext, 1, 0);
            var imageKey = game.Characters[0].ImageKey;
            var imageFile = Substitute.For<IFormFile>();

            await testObj.AddCharacter(game.ID, game.Characters[0].ID, "Char1", imageFile);

            await cdnService.Received(1).DeleteImageAsync(imageKey, ClimbImageRules.CharacterPic);
        }

        [Test]
        public async Task AddCharacter_OldCharacterNewImage_NewImageUploaded()
        {
            var game = GameUtility.Create(dbContext, 1, 0);
            var imageFile = Substitute.For<IFormFile>();

            await testObj.AddCharacter(game.ID, game.Characters[0].ID, "Char1", imageFile);

            await cdnService.Received(1).UploadImageAsync(imageFile, ClimbImageRules.CharacterPic);
        }

        [Test]
        public async Task AddCharacter_OldCharacterNewImage_ImageKeySaved()
        {
            var game = GameUtility.Create(dbContext, 1, 0);
            var imageFile = Substitute.For<IFormFile>();
            const string imageKey = "key";
            cdnService.UploadImageAsync(imageFile, ClimbImageRules.CharacterPic).Returns(imageKey);

            var character = await testObj.AddCharacter(game.ID, game.Characters[0].ID, "Char1", imageFile);

            Assert.AreEqual(imageKey, character.ImageKey);
        }

        [Test]
        public async Task AddCharacter_OldCharacterNoImage_ValuesUpdated()
        {
            const string name = "NewName";
            var game = GameUtility.Create(dbContext, 1, 0);

            var character = await testObj.AddCharacter(game.ID, game.Characters[0].ID, name, null);

            Assert.AreEqual(name, character.Name);
        }

        [Test]
        public void AddCharacter_HasCharacterIDButNoCharacter_NotFoundException()
        {
            var game = GameUtility.Create(dbContext, 0, 0);
            var imageFile = Substitute.For<IFormFile>();

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddCharacter(game.ID, 1, "Char1", imageFile));
        }

        [Test]
        public async Task AddStage_Valid_Stage()
        {
            var game = GameUtility.Create(dbContext, 0, 0);

            var stage = await testObj.AddStage(game.ID, null, "Stage1");

            Assert.IsNotNull(stage);
        }

        [Test]
        public void AddStage_NoGame_NotFoundException()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddStage(0, null, "Stage1"));
        }

        [Test]
        public void AddStage_NameTaken_ConflictException()
        {
            var game = GameUtility.Create(dbContext, 0, 1);

            Assert.ThrowsAsync<ConflictException>(() => testObj.AddStage(game.ID, null, game.Stages[0].Name));
        }
        
        [Test]
        public void AddStage_HasStageIDButNoStage_NotFoundException()
        {
            var game = GameUtility.Create(dbContext, 0, 0);

            Assert.ThrowsAsync<NotFoundException>(() => testObj.AddStage(game.ID, 1, "Stage1"));
        }

        [Test]
        public async Task AddStage_OldStage_ValuesUpdated()
        {
            const string name = "NewName";
            var game = GameUtility.Create(dbContext, 0, 1);

            var stage = await testObj.AddStage(game.ID, game.Stages[0].ID, name);

            Assert.AreEqual(name, stage.Name);
        }
    }
}