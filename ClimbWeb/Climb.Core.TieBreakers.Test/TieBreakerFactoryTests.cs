using Climb.Core.TieBreakers.Internal;
using NUnit.Framework;

namespace Climb.Core.TieBreakers.Test
{
    [TestFixture]
    public class TieBreakerFactoryTests
    {
        private TieBreaker testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new TieBreaker();
        }
    }
}