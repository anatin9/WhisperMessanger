using Communication;
using Whispr.ChatServer;
using Whispr.KeyServer;
using Whispr.RegistryServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WhisprUnitTests
{
    [TestClass]
    public class CommSubsystemUnitTest
    {
        [TestMethod]
        public void TestChatCommSubsystem()
        {
            ConversationFactory chatFactory = new ChatServerConversationFactory();
            CommSubsystem chatSubsystem = new CommSubsystem(chatFactory, 1024, 2024);
            Assert.IsNotNull(chatSubsystem);
        }

        [TestMethod]
        public void TestKeyCommSubsystem()
        {
            ConversationFactory keyFactory = new KeyServerConversationFactory();
            CommSubsystem keySubsystem = new CommSubsystem(keyFactory, 1024, 2024);
            Assert.IsNotNull(keySubsystem);
        }

        [TestMethod]
        public void TestRegistryCommSubsystem()
        {
            ConversationFactory registryFactory = new RegistryServerConversationFactory();
            CommSubsystem registrySubsytem = new CommSubsystem(registryFactory, 1024, 2024);
            Assert.IsNotNull(registrySubsytem);
        }

        [TestMethod]
        public void TestClientCommSubsystem()
        {
            ConversationFactory clientFactory = new RegistryServerConversationFactory();
            CommSubsystem clientSubsytem = new CommSubsystem(clientFactory, 1024, 2024);
            Assert.IsNotNull(clientSubsytem);
        }

        [TestMethod]
        public void TestGetPublicIP()
        {
            Assert.IsNotNull(CommSubsystem.GetPublicIP());
        }

    }

}
