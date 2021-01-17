using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whispr.ChatServer.Components;
using Whispr.ChatServer.Conversations;
using Whispr.Client.Conversations;
using Communication;
using Communication.MessageClasses.Components;

namespace Whispr.Communication.Client.Components
{
    public class ChatToRegistryServerHeartbeatThread
    {
        public IPEndPoint EndPoint { get; set; } = null;
        public Server Server { get; set; } = new Server();
        private ConversationFactory Factory;

        public ChatToRegistryServerHeartbeatThread(ConversationFactory factory)
        {
            Factory = factory;
            Thread _HeartbeatThread = new Thread(Run);
            _HeartbeatThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                if (EndPoint != null)
                {
                    Console.WriteLine("Connecting to Registry Server");
                    RegisterServerInitiator c = Factory.CreateFromConversationType<RegisterServerInitiator>();
                    c.RemoteEndPoint = EndPoint;
                    c.Server = Server;
                    c.Launch();
                }
                Thread.Sleep(10000);
            }
        }
    }
}