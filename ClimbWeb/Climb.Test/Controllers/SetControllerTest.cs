using Climb.Controllers;
using Climb.Data;
using Climb.Services.ModelServices;
using Climb.Test.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
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

            testObj = new SetController(setService, dbContext, logger);
        }
    }
}