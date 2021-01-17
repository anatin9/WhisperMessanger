using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;

namespace WhisprUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void testConstruction()
        {
            Envelope env = new Envelope();
            Assert.IsTrue(env.IsValidToSend);

        }
    }
}
