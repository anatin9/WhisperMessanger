using System;
using System.Collections.Generic;
using System.Net;
using Whispr.ChatServer.ResourceManagers;
using Whispr.Client;
using Whispr.Client.Conversations;
using Communication;
using Whispr.Communication.Client.Components;
using Communication.MessageClasses.Components;
using Whispr.RegistryServer;
using Whispr.Client.ResourceManagers;
using System.Threading;
using Whispr.Client.Components;

namespace Whispr
{
    public class ClientProgram : AppProcess
    {
        private EncryptedMessageResourceManager EMRM = EncryptedMessageResourceManager.GetInstance();
        private static ConversationFactory factory = new ClientConversationFactory();
        private static UpdateServersListThread updateServersListThread;
        private EncryptionService ES = new EncryptionService();
        private GetNewMessagesThread GetNewMessagesThread;
        public ClientToChatServerHeartbeatThread HeartbeatThread = new ClientToChatServerHeartbeatThread(factory);
        public UserResourceManager ULRM = UserResourceManager.GetInstance();
        public UserKeysUpdateThread UserKeysUpdateThread = new UserKeysUpdateThread(factory);
        public ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();
        public volatile bool isRunning;


        public ClientProgram()
        {
            //StartCommSubsystem(factory, 1027, 49151, 0);
            isRunning = true;
            StartCommSubsystem(factory, 1030, 49151, 0);
            GetNewMessagesThread = new GetNewMessagesThread(factory, ref ES);
            updateServersListThread = new UpdateServersListThread(factory);
            //RegisterPublicKey();

            //GetPublicKeys();

            //Login();
            //GetServers();
            //DoChatStuff();

            //GetPublicKeys();
            //GetUsers();
        }

        public void WaitForConnect()
        {
            while (SLRM.Servers.Count == 0) { Thread.Sleep(500); }
        }

        public void EndConnection()
        {
            isRunning = false;
            EndCommSubsystem();
            HeartbeatThread.EndHeartbeat();
            GetNewMessagesThread.StopThread();
            UserKeysUpdateThread.StopThread();
            updateServersListThread.StopThread();
        }

        public void Login(string user, string password)
        {
            RegisterPublicKey();
            LoginRequestInitiator c = factory.CreateFromConversationType<LoginRequestInitiator>();
            c.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1026); // TODO - Make dynamic
            c.Username = user;
            c.Password = password;
            c.Launch();
        }


        private void DoChatStuff()
        {
            Console.WriteLine("Doing chat stuff...");
            while (true)
            {
                Console.WriteLine("Doing chat stuff...");
            }
        }

        public void RegisterPublicKey()
        {
            RegisterKeyRequestInitiator c = factory.CreateFromConversationType<RegisterKeyRequestInitiator>();
            c.Key = ES.GetRSAPublicKey();
            c.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1026); // TODO - Make dynamic
            c.Launch();
        }

        public void SendChatMessage(string message)
        {
            if (SLRM.ActiveServer == null)
            {
                return;
            }

            Console.WriteLine("Attempting to send message: $", message);
            SendMessageRequestInitiator c = factory.CreateFromConversationType<SendMessageRequestInitiator>();
            c.ES = ES;
            c.RemoteEndPoint = SLRM.ActiveServer.ChatServerEndpoint;
            c.Message = message;
            c.GroupID = "Something?";
            
            c.Launch();
        }

        public void GetNewMessages()
        {
            if (SLRM.ActiveServer == null)
            {
                return;
            }
            NewMessagesRequestInitiator c = factory.CreateFromConversationType<NewMessagesRequestInitiator>();
            c.RemoteEndPoint = SLRM.ActiveServer.ChatServerEndpoint;
            c.ES = ES;
            c.Launch();
        }

        public void GetPublicKeys()
        {
            List<string> users = new List<string>();
            users.Add("Joe");
            users.Add("Bob");

            PublicKeysRequestInitiator c = factory.CreateFromConversationType<PublicKeysRequestInitiator>();
            c.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1027); // TODO - Make dynamic
            c.users = users;
            c.Launch();
        }
    }
}