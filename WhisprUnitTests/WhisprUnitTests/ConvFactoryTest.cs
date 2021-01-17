using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;
using Whispr.ChatServer;
using Whispr.KeyServer;
using Whispr.RegistryServer;
using Whispr.Client;

namespace WhisprUnitTests
{
    [TestClass]
    public class ConvFactoryTest
    {

        [TestMethod]
        public void TestChatDatatMembers()
        {
            ConversationFactory testFactory = new ChatServerConversationFactory();
            Assert.AreEqual(3, testFactory.DefaultMaxRetries);
            Assert.AreEqual(3000, testFactory.DefaultTimeout);
        }


        [TestMethod]
        public void TestKeyDatatMembers()
        {
            ConversationFactory testFactory = new KeyServerConversationFactory();
            Assert.AreEqual(3, testFactory.DefaultMaxRetries);
            Assert.AreEqual(3000, testFactory.DefaultTimeout);
        }

        [TestMethod]
        public void TestRegistryDatatMembers()
        {
            ConversationFactory testFactory = new RegistryServerConversationFactory();
            Assert.AreEqual(3, testFactory.DefaultMaxRetries);
            Assert.AreEqual(3000, testFactory.DefaultTimeout);
        }


        [TestMethod]
        public void TestClientDatatMembers()
        {
            ConversationFactory testFactory = new ClientConversationFactory();
            Assert.AreEqual(3, testFactory.DefaultMaxRetries);
            Assert.AreEqual(3000, testFactory.DefaultTimeout);
        }
    }

}

