using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whispr.ChatServer;
using Whispr.Client;
using Communication;
using Communication.MessageClasses;
using Whispr.KeyServer;
using Whispr.RegistryServer;
using Whispr.Client.Conversations;
using Whispr.ChatServer.Conversations;
using System.Collections.Generic;
using Communication.MessageClasses.Components;
using Conversations;

namespace WhisprUnitTests
{
    [TestClass]
    public class ConversationTests : AppProcess
    {
        readonly IPEndPoint testEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

        [TestMethod]
        public void ClientConversations()
        {
            // Test factory constructor
            ClientConversationFactory f = new ClientConversationFactory();
            Assert.IsNotNull(f);
            f.Initialize();

            // Test process initiator conversations
            ConnectToChatInitiator test1 = f.CreateFromConversationType<ConnectToChatInitiator>();
            Assert.IsNotNull(test1);
            test1.RemoteEndPoint = testEndpoint;
            test1.Launch();
            Assert.AreEqual(test1.RemoteEndPoint, testEndpoint);
            // What to assert on launch? Ask in code review

            ListServersRequestInitiator test2 = f.CreateFromConversationType<ListServersRequestInitiator>();
            Assert.IsNotNull(test2);
            test2.RemoteEndPoint = testEndpoint;
            test2.Launch();
            Assert.AreEqual(test2.RemoteEndPoint, testEndpoint);

            LoginRequestInitiator test3 = f.CreateFromConversationType<LoginRequestInitiator>();
            Assert.IsNotNull(test3);
            test3.RemoteEndPoint = testEndpoint;
            test3.Launch();
            Assert.AreEqual(test3.RemoteEndPoint, testEndpoint);

            PublicKeysRequestInitiator test4 = f.CreateFromConversationType<PublicKeysRequestInitiator>();
            Assert.IsNotNull(test4);
            test4.RemoteEndPoint = testEndpoint;
            test4.Launch();
            Assert.AreEqual(test4.RemoteEndPoint, testEndpoint);

            RegisterKeyRequestInitiator test5 = f.CreateFromConversationType<RegisterKeyRequestInitiator>();
            Assert.IsNotNull(test5);
            test5.RemoteEndPoint = testEndpoint;
            test5.Launch();
            Assert.AreEqual(test5.RemoteEndPoint, testEndpoint);


            // Test process responder conversations
            //   There are none to test

        }

        [TestMethod]
        public void ChatServerConversations()
        {
            ChatServerConversationFactory f;
            f = new ChatServerConversationFactory();
            Assert.IsNotNull(f);
            f.Initialize();

            // Test process initiator conversations
            HeartbeatInitiator test1 = f.CreateFromConversationType<HeartbeatInitiator>();
            Assert.IsNotNull(test1);
            test1.RemoteEndPoint = testEndpoint;
            test1.Launch();

            RegisterServerInitiator test2 = f.CreateFromConversationType<RegisterServerInitiator>();
            Assert.IsNotNull(test2);
            test2.RemoteEndPoint = testEndpoint;
            test2.Launch();
            Assert.AreEqual(test2.RemoteEndPoint, testEndpoint);

            // Test process responder conversations
            Envelope env1 = new Envelope();
            User u = new User();
            env1.Message = new ConnectToChatRequest(u);
            Responder resp1 = f.CreateFromEnvelope(env1);
            Assert.IsNotNull(resp1);
            Assert.AreEqual(resp1.ConversationId.ToString(), env1.Message.ConversationId.ToString());

            Envelope env2 = new Envelope();
            env2.Message = new HeartBeat(10000);
            Responder resp2 = f.CreateFromEnvelope(env2);
            Assert.IsNotNull(resp2);
            Assert.AreEqual(resp2.ConversationId.ToString(), env2.Message.ConversationId.ToString());
        }

        [TestMethod]
        public void RegistryServerConversations()
        {
            ConversationFactory f;
            f = new RegistryServerConversationFactory();
            HeartbeatInitiator registeredEndpointHeartbeat = f.CreateFromConversationType<HeartbeatInitiator>();
            Assert.IsNotNull(f);
            f.Initialize();

            // Test Register Server Initiated Conversations
            HeartbeatInitiator test1 = f.CreateFromConversationType<HeartbeatInitiator>();
            Assert.IsNotNull(test1);
            test1.RemoteEndPoint = testEndpoint;
            test1.Launch();
            Assert.AreEqual(test1.RemoteEndPoint, testEndpoint);

            // Test Responder Conversations
            Envelope env1 = new Envelope();
            env1.Message = new RegisterServerRequest(new Server());
            Responder resp1 = f.CreateFromEnvelope(env1);
            Assert.IsNotNull(resp1);
            Assert.AreEqual(resp1.ConversationId.ToString(), env1.Message.ConversationId.ToString());
        }

        [TestMethod]
        public void KeyServerConversations()
        {
            ConversationFactory f;
            f = new KeyServerConversationFactory();
            Assert.IsNotNull(f);
            f.Initialize();

            // Test Responder Conversations
            Envelope env1 = new Envelope();
            List<string> testList = new List<string>();
            testList.Add("test_username");
            env1.Message = new PublicKeysRequest(testList);
            Responder resp1 = f.CreateFromEnvelope(env1);
            Assert.IsNotNull(resp1);
            Assert.AreEqual(resp1.ConversationId.ToString(), env1.Message.ConversationId.ToString());

            Envelope env2 = new Envelope();
            env2.Message = new RegisterKeyRequest("test_user", new UserKey("test_user", "test_key"));
            Responder resp2 = f.CreateFromEnvelope(env2);
            Assert.IsNotNull(resp2);
            Assert.AreEqual(resp2.ConversationId.ToString(), env2.Message.ConversationId.ToString());

            Envelope env3 = new Envelope();
            env3.Message = new LoginRequest("test_username", "test_password");
            Responder resp3 = f.CreateFromEnvelope(env3);
            Assert.IsNotNull(resp3);
            Assert.AreEqual(resp3.ConversationId.ToString(), env3.Message.ConversationId.ToString());
        }
    }
}
