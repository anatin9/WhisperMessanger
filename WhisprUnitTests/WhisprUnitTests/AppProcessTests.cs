using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whispr.ChatServer;
using Communication;

namespace WhisprUnitTests
{
    [TestClass]
    public class ComAppProcessTests : AppProcess
    {
        [TestMethod]
        public void TestAppProcess()
        {
            ConversationFactory factory = new ChatServerConversationFactory();
            StartCommSubsystem(factory, 1024, 2024);
            Assert.IsNotNull(MyCommSubsystem);
            Assert.IsTrue(MyCommSubsystem.Port >= 1024);
            Assert.IsTrue(MyCommSubsystem.Port <= 2024);
            Assert.IsNotNull(MyCommSubsystem.Conversations);
        }
    }
}
