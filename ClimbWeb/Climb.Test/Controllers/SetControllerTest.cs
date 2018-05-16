using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Controllers;
using Climb.Data;
using Climb.Exceptions;
using Climb.Extensions;
using Climb.Models;
using Climb.Requests.Sets;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    [TestFixture]
    public class SetControllerTest
    {
        private SetController testObj;
        private ISetService setService;
        private ApplicationDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            setService = Substitute.For<ISetService>();
            dbContext = DbContextUtility.CreateMockDb();
            var logger = Substitute.For<ILogger<SetController>>();

            testObj = new SetController(dbContext, setService, logger);
        }

        [Test]
        public async Task Submit_ServiceException_ServerError()
        {
            setService.Update(0, null).ThrowsForAnyArgs<Exception>();
            var set = SetUtility.Create(dbContext);
            var request = new SubmitRequest {SetID = set.ID, Matches = new MatchForm[0]};

            var result = await testObj.Submit(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Submit_NoSet_NotFound()
        {
            setService.Update(0, null).ThrowsForAnyArgs<NotFoundException>();
            var request = new SubmitRequest {SetID = 0, Matches = new MatchForm[0]};

            var result = await testObj.Submit(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Submit_BadRequest_BadRequest()
        {
            setService.Update(0, null).ThrowsForAnyArgs<BadRequestException>();
            var request = new SubmitRequest {SetID = 0, Matches = new MatchForm[0]};

            var result = await testObj.Submit(request);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_HasSet_Ok()
        {
            var set = SetUtility.Create(dbContext);

            var result = await testObj.Get(set.ID);
            var resultObj = result.GetObject<Set>();

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.OK);
            Assert.AreEqual(set.ID, resultObj.ID);
        }

        [Test]
        public async Task Get_NoSet_NotFound()
        {
            var result = await testObj.Get(0);

            ControllerUtility.AssertStatusCode(result, HttpStatusCode.NotFound);
        }
    }
}