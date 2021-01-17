using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Communication.MessageClasses;
using Communication;
using System.Collections.Generic;
using Communication.MessageClasses.Components;
using System.Net;
using Whispr.Client.Components;
using System.Security.Cryptography;

namespace WhisprUnitTests
{
    [TestClass]
    public class MessageTests
    {
        
        [TestMethod]
        public void TestMessageGettersSetters()
        {
            // Test Message constructor

            // Setup
            Message m1 = new SendMessageRequest("my group id", "Hello World", "Me");
            Message m2 = new SendMessageRequest("my group id", "Hello World", "Me");

            // Tests
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(SendMessageRequest)));
            Assert.AreNotEqual(m1.MessageId, m2.MessageId);

            // Setup
            Server s = new Server();
            s.ChatServerEndpoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"), 49);
            s.Hostname = "TestHostName";
            s.ActiveUsers = 14;
            Message m3 = new RegisterServerRequest(s);
            Message m4 = new RegisterServerRequest(s);

            // Tests
            Assert.IsNotNull(m3.Server);
            Assert.IsNotNull(m4.Server);
            Assert.AreEqual(m3.Server.Hostname, m4.Server.Hostname);
            Assert.AreEqual(m3.Server.ChatServerEndpoint, m4.Server.ChatServerEndpoint);
            Assert.AreEqual(m3.Server.ActiveUsers, m4.Server.ActiveUsers);
            Assert.AreEqual(m3.MessageType, m4.MessageType);
            Assert.AreNotEqual(m3.MessageId, m4.MessageId);

            // Setup
            List<Tuple<string, UserKey>> k = new List<Tuple<string, UserKey>>();
            k.Add(new Tuple<string, UserKey>("508", new UserKey("Bob", "BobsKey")));
            k.Add(new Tuple<string, UserKey>("509", new UserKey("Joe", "JoesKey")));
            Message m5 = new PublicKeysResponse(k);

            // Tests
            Assert.IsNotNull(m5.Keys);
            Assert.AreEqual(m5.Keys[0].Item2.Username, "Bob");
            Assert.AreEqual(m5.Keys[1].Item2.Username, "Joe");
            Assert.AreEqual(m5.Keys[0].Item2.Publickey, "BobsKey");
            Assert.AreEqual(m5.Keys[1].Item2.Publickey, "JoesKey");

            // Setup
            m1 = new RegisterKeyRequest("Bob", new UserKey("Bob", "BobsKey"));
            m1.UserKey = new UserKey("NotBob", "NotBobsKey");

            // Tests
            Assert.AreEqual(m1.UserKey.Username, "NotBob");
            Assert.AreEqual(m1.UserKey.Publickey, "NotBobsKey");

            // Setup
            m1 = new Acknowledge(ResponseCodes.SUCCESS, "You win!");
            m1.Code = ResponseCodes.FAIL;
            m1.ResponseMessage = "You lose!";

            // Tests
            Assert.AreEqual(m1.Code, ResponseCodes.FAIL);
            Assert.AreEqual(m1.ResponseMessage, "You lose!");

            // Setup
            List<string> users = new List<string>();
            users.Add("Billy");
            users.Add("Joe");
            users.Add("Bob");
            m1 = new PublicKeysRequest(users);

            // Tests
            Assert.IsNotNull(m1.Users);
            Assert.AreEqual(m1.Users[0], "Billy");
            Assert.AreEqual(m1.Users[1], "Joe");
            Assert.AreEqual(m1.Users[2], "Bob");
            m1.Users.RemoveAt(0);
            Assert.AreEqual(m1.Users[0], "Joe");
            Assert.AreEqual(m1.Users[1], "Bob");

            // Setup
            m1 = new HeartBeat(300);

            // Tests
            Assert.AreEqual(m1.KeepAlive, 300);
            m1.KeepAlive = 600;
            Assert.AreEqual(m1.KeepAlive, 600);

            // Setup
            m1 = new NewMessagesRequest("MyId", "Last message ID");

            // Tests
            Assert.AreEqual(m1.LastMessage, "Last message ID");
            m1.LastMessage = "";
            Assert.AreEqual(m1.LastMessage, "");

            // Setup
            List<EncryptedMessage> list = new List<EncryptedMessage>();
            EncryptedMessage cm1 = new EncryptedMessage();
            cm1.MessageId = "message id 1";
            cm1.SenderId = "Bob";
            cm1.Timestamp = 101010101010;
            cm1.PlainText = "Dead men tell no tales";
            cm1.GroupId = "My special group";
            list.Add(cm1);
            m1 = new NewMessagesResponse(list);

            // Tests
            Assert.AreEqual(m1.Messages[0].SenderId, "Bob");
            Assert.AreEqual(m1.Messages[0].MessageId, "message id 1");
            Assert.AreEqual(m1.Messages[0].Timestamp, 101010101010);
            Assert.AreEqual(m1.Messages[0].PlainText, "Dead men tell no tales");
            Assert.AreEqual(m1.Messages[0].GroupId, "My special group");
            m1.Messages[0].SenderId = "Joe";
            m1.Messages[0].MessageId = "message id 2";
            m1.Messages[0].Timestamp = 10101;
            m1.Messages[0].PlainText = "Tell tales, dead men do not";
            m1.Messages[0].GroupId = "My not as special group";
            Assert.AreEqual(m1.Messages[0].SenderId, "Joe");
            Assert.AreEqual(m1.Messages[0].MessageId, "message id 2");
            Assert.AreEqual(m1.Messages[0].Timestamp, 10101);
            Assert.AreEqual(m1.Messages[0].PlainText, "Tell tales, dead men do not");
            Assert.AreEqual(m1.Messages[0].GroupId, "My not as special group");

            // Setup
            Server s1 = new Server();
            s1.ChatServerEndpoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 12345);
            s1.ActiveUsers = 38;
            List<Server> l1 = new List<Server>();
            l1.Add(s1);
            m1 = new ListServersResponse(l1);

            // Tests
            Assert.AreEqual(m1.Servers[0].ChatServerEndpoint.ToString(), "192.168.0.1:12345");
            Assert.AreEqual(m1.Servers[0].ActiveUsers, 38);
            m1.Servers[0].ActiveUsers = 43;
            Assert.AreEqual(m1.Servers[0].ActiveUsers, 43);
        }

        [TestMethod]
        public void TestMessageSerializations()
        {
            // Test Serialization

            // Setup
            Message expected = new SendMessageRequest("my group id", "Hello World", "Me");
            string jsontext = expected.JSON();
            Message m = MessageFromJSONFactory.GetMessage(jsontext);

            // Tests
            Trace.WriteLine(m.MessageId);
            Trace.WriteLine(expected.MessageId);
            Assert.IsNotNull(jsontext);
            Assert.IsNotNull(m);
            Assert.IsNotNull(expected);
            Assert.AreEqual(m.MessageId, expected.MessageId);

            // Setup
            Message m1 = new RegisterServerRequest(new Server());
            Message m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.IsNotNull(m1.Server);
            Assert.IsNotNull(m2.Server);
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(RegisterServerRequest)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(RegisterServerRequest)));

            // Setup
            List<Tuple<string, UserKey>> keys = new List<Tuple<string, UserKey>>();
            keys.Add(new Tuple<string, UserKey>("1", new UserKey("Bob", "BobsKey")));
            keys.Add(new Tuple<string, UserKey>("2", new UserKey("Joe", "JoesKey")));
            m1 = new PublicKeysResponse(keys);
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m2.Keys[0].Item2.Publickey, "BobsKey");
            Assert.AreEqual(m2.Keys[1].Item2.Publickey, "JoesKey");
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(PublicKeysResponse)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(PublicKeysResponse)));

            // Setup
            m1 = new RegisterKeyRequest("1", new UserKey("Bob", "BobsKey"));
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());
            Assert.AreEqual(m1.UserKey.Username, m2.UserKey.Username);
            Assert.AreEqual(m1.UserKey.Publickey, m2.UserKey.Publickey);
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m1.MessageType, m2.MessageType);

            // Setup
            m1 = new Acknowledge(ResponseCodes.SUCCESS, "You win!");
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.Code, ResponseCodes.SUCCESS);
            Assert.AreEqual(m1.ResponseMessage, "You win!");

            // Setup
            List<string> users = new List<string>();
            users.Add("Billy");
            users.Add("Joe");
            users.Add("Bob");
            m1 = new PublicKeysRequest(users);
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(PublicKeysRequest)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(PublicKeysRequest)));
            Assert.AreEqual(m1.Users[0], m2.Users[0]);
            Assert.AreEqual(m1.Users[1], m2.Users[1]);
            Assert.AreEqual(m1.Users[2], m2.Users[2]);
            Assert.AreEqual(m1.Users.Count, 3);
            Assert.AreEqual(m1.Users.Count, m2.Users.Count);

            // Setup
            m1 = new HeartBeat(300);
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.KeepAlive, 300);
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(HeartBeat)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(HeartBeat)));
            Assert.AreEqual(m1.MessageId, m2.MessageId);

            // Setup
            m1 = new NewMessagesRequest("MyId", "Last message ID");
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m1.LastMessage, m2.LastMessage);
            Assert.AreEqual(m2.LastMessage, "Last message ID");
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(NewMessagesRequest)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(NewMessagesRequest)));

            // Setup
            List<EncryptedMessage> list = new List<EncryptedMessage>();
            EncryptedMessage cm1 = new EncryptedMessage();
            cm1.MessageId = "message id 1";
            cm1.SenderId = "Bob";
            cm1.Timestamp = 101010101010;
            cm1.PlainText = "Dead men tell no tales";
            cm1.GroupId = "My special group";
            list.Add(cm1);
            m1 = new NewMessagesResponse(list);
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(NewMessagesResponse)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(NewMessagesResponse)));
            Assert.AreEqual(m1.Messages[0].MessageId, m2.Messages[0].MessageId);
            Assert.AreEqual(m1.Messages[0].Timestamp, m2.Messages[0].Timestamp);
            Assert.AreEqual(m1.Messages[0].GroupId, m2.Messages[0].GroupId);
            Assert.AreEqual(m1.Messages[0].SenderId, m2.Messages[0].SenderId);
            Assert.IsNull(m2.Messages[0].PlainText); // Make sure message isn't passed in plain text

            // Setup
            Server s1 = new Server();
            s1.ChatServerEndpoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 12345);
            s1.ActiveUsers = 38;
            List<Server> l1 = new List<Server>();
            l1.Add(s1);
            m1 = new ListServersResponse(l1);
            m2 = MessageFromJSONFactory.GetMessage(m1.JSON());

            // Tests
            Assert.AreEqual(m1.MessageType, MessageTypes.ToInt(typeof(ListServersResponse)));
            Assert.AreEqual(m2.MessageType, MessageTypes.ToInt(typeof(ListServersResponse)));
            Assert.AreEqual(m1.MessageId, m2.MessageId);
            Assert.AreEqual(m1.Servers[0].ChatServerEndpoint, m2.Servers[0].ChatServerEndpoint);
            Assert.AreEqual(m1.Servers[0].ChatServerEndpoint, m2.Servers[0].ChatServerEndpoint);
            Assert.AreEqual(m2.Servers[0].ChatServerEndpoint.ToString(), "192.168.0.1:12345");
            Assert.AreEqual(m1.Servers[0].ActiveUsers, m2.Servers[0].ActiveUsers);
            Assert.AreEqual(m2.Servers[0].ActiveUsers, 38);
        }

        [TestMethod]
        public void TestAESEncryption()
        {
            EncryptionService ES = new EncryptionService();

            string plaintext = "Hello World!";

            Tuple<string, string, string> keyInfo1 = ES.AESEncrypt(plaintext);
            Assert.AreNotEqual(keyInfo1.Item1, plaintext);
            Assert.AreNotEqual(keyInfo1.Item2, plaintext);
            Assert.AreNotEqual(keyInfo1.Item3, plaintext);
            string decrypted1 = ES.AESDecrypt(keyInfo1.Item1, keyInfo1.Item2, keyInfo1.Item3);
            Assert.AreEqual(plaintext, decrypted1);

            Tuple<string, string, string> keyInfo2 = ES.AESEncrypt(plaintext);
            Assert.AreNotEqual(keyInfo2.Item1, plaintext);
            Assert.AreNotEqual(keyInfo2.Item2, plaintext);
            Assert.AreNotEqual(keyInfo2.Item3, plaintext);
            string decrypted2 = ES.AESDecrypt(keyInfo2.Item1, keyInfo2.Item2, keyInfo2.Item3);
            Assert.AreEqual(plaintext, decrypted2);

            // Make sure we are not encrypted the same message the same way twice
            Assert.AreNotEqual(keyInfo1.Item1, keyInfo2.Item1);
            Assert.AreNotEqual(keyInfo1.Item2, keyInfo2.Item2);
            Assert.AreNotEqual(keyInfo1.Item3, keyInfo2.Item3);
        }

        [TestMethod]
        public void TestRSAEncryption()
        {
            EncryptionService ES = new EncryptionService();
            string plaintext = "Hello World!";

            string pubkey = ES.GetRSAPublicKey();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(pubkey);
            string encrypted = ES.RSAEncrypt(plaintext, rsa);
            string decrypted = ES.RSADecrypt(encrypted);
            Assert.AreNotEqual(plaintext, encrypted);
            Assert.AreNotEqual(encrypted, decrypted);
            Assert.AreEqual(decrypted, plaintext);
        }

        [TestMethod]
        public void TestEncryptedMessage()
        {
            EncryptedMessage e1 = new EncryptedMessage();
            e1.PlainText = "Hello World";
            string jsontext = JsonConvert.SerializeObject(e1);
            EncryptedMessage e2 = (EncryptedMessage)JsonConvert.DeserializeObject(jsontext, typeof(EncryptedMessage));
            Assert.AreNotEqual(e1.PlainText, e2.PlainText); // Check for plain text leaking into JSON string
            Assert.IsNull(e2.PlainText);
        }
    }
}
