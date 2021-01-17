using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Whispr.RegistryServer;
using Whispr.RegistryServer.Components;
using static Communication.Conversation;

namespace Whispr
{
    class RegistryServerProgram : AppProcess
    {
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();
        private static HeartbeatTimeoutThread timeout = new HeartbeatTimeoutThread();
        private ConversationFactory _factory;
        private bool _running;
        private int TimeToCheckChatServer { get; set; } = 10000;

        public RegistryServerProgram()
        {
            _running = true;
            _factory = new RegistryServerConversationFactory();
            StartCommSubsystem(_factory, 1024, 1024);

            ReportServerCount();
        }

        public void ReportServerCount()
        {
            while (_running)
            {
                Console.WriteLine("\n\nNumber of registered chat servers: " + SLRM.Servers.Count);

                Thread.Sleep(TimeToCheckChatServer);
            }
        }

        public void Dispatch(Envelope new_env)
        {
            //Console.WriteLine("Got an envelope?");
        }
    }
}