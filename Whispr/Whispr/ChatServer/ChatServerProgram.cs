using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Whispr.ChatServer;
using Whispr.ChatServer.Components;
using Whispr.ChatServer.Conversations;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Whispr.Communication.Client.Components;
using Communication.MessageClasses;
using Whispr.RegistryServer;

namespace Whispr
{
    class ChatServerProgram : AppProcess
    {
        private static ConversationFactory factory = new ChatServerConversationFactory();
        private ChatToRegistryServerHeartbeatThread Heartbeat = new ChatToRegistryServerHeartbeatThread(factory);
        private static HeartbeatTimeoutThread timeout = new HeartbeatTimeoutThread();
        readonly IPEndPoint REGISTRY_SERVER_IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024);
        private static UserListResourceManager ULRM = UserListResourceManager.GetInstance();



        public ChatServerProgram()
        {

            StartCommSubsystem(factory, 1030, 49151);
            Heartbeat.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024);

            while(true)
            {
                Console.WriteLine("Connected Clients: " + ULRM.Users.Count);
                Thread.Sleep(5000);
            }
        }

        

        public static string GetPublicIP()
        {
            return new WebClient().DownloadString("https://ipinfo.io/ip").Replace("\n", "");
        }

        // Copied from Stack Overflow
        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length != 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
            int port;
            if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }
    }
}