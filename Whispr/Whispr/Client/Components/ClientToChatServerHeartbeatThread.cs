using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whispr.Client.Conversations;
using Communication.MessageClasses.Components;
using Communication;
using Whispr.ChatServer.ResourceManagers;
using Whispr.Client.ResourceManagers;

namespace Whispr.Communication.Client.Components
{
    public class ClientToChatServerHeartbeatThread
    {
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();
        private UserResourceManager URM = UserResourceManager.GetInstance();
        private ConversationFactory Factory;
        private volatile bool isRunning;

        public ClientToChatServerHeartbeatThread(ConversationFactory factory)
        {
            Factory = factory;
            isRunning = true;
            Thread _HeartbeatThread = new Thread(Run);
            _HeartbeatThread.Start();
        }

        public void EndHeartbeat()
        {
            isRunning = false;
        }

        private void Run()
        {
            while (isRunning)
            {
                if (SLRM.ActiveServer != null && URM.GetUserId() != null)
                {
                    Console.WriteLine("Connecting to Chat Server");
                    ConnectToChatInitiator c = Factory.CreateFromConversationType<ConnectToChatInitiator>();
                    c.RemoteEndPoint = ServerListResourceManager.GetInstance().ActiveServer.ChatServerEndpoint;
                    c.User = new User(URM.GetUserId(), null);
                    c.Launch();
                    Thread.Sleep(10000);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
