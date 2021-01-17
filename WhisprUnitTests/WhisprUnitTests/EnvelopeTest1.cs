using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace WhisprUnitTests
{
    [TestClass]
    public class EnvelopeTest
    {
        Envelope testEnvelope = new Envelope();

        [TestMethod]
        public void TestValidtoSend()
        {
            Assert.IsTrue(testEnvelope.IsValidToSend);
        }

        [TestMethod]
        public void TestEncode()
        {
            Envelope env1 = new Envelope();
            User u = new User();
            env1.Message = new ConnectToChatRequest(u);
            Assert.IsNotNull(env1.Encode());
        }

        /*[Test]
        public void TestDecode()
        {
            Envelope env1 = new Envelope();
            env1.Message = new ConnectToChatRequest(true);

        }*/

    }

}
