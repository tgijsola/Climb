using System;
using System.Net;
using Climb.Controllers;
using Climb.Exceptions;
using Climb.Test.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Climb.Test.Controllers
{
    public class FakeBaseController : BaseController<FakeBaseController>
    {
        public FakeBaseController(ILogger<FakeBaseController> logger)
            : base(logger)
        {
        }

        public new IActionResult GetExceptionResult(Exception exception, object request)
        {
            return base.GetExceptionResult(exception, request);
        }
    }

    [TestFixture]
    public class BaseControllerTest
    {
        private FakeBaseController testObj;

        [SetUp]
        public void SetUp()
        {
            var logger = Substitute.For<ILogger<FakeBaseController>>();

            testObj = new FakeBaseController(logger);
        }

        [TestCase(typeof(NotFoundException), HttpStatusCode.NotFound)]
        [TestCase(typeof(BadRequestException), HttpStatusCode.BadRequest)]
        [TestCase(typeof(ConflictException), HttpStatusCode.Conflict)]
        [TestCase(typeof(Exception), HttpStatusCode.InternalServerError)]
        public void GetExceptionResult_NotFound_NotFound(Type exceptionType, HttpStatusCode statusCode)
        {
            var exception = Activator.CreateInstance(exceptionType) as Exception;
            var result = testObj.GetExceptionResult(exception, null);

            ControllerUtility.AssertStatusCode(result, statusCode);
        }
    }
}