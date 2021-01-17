using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.RegistryServer;

namespace Whispr.Client.Conversations
{
    public class ListServersRequestInitiator : ClientInitiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(ListServersResponse) };
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();

        protected override Message CreateFirstMessage()
        {
            Message m = new ListServersRequest(true);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            ListServersResponse m = (ListServersResponse)env.Message;
            ConcurrentBag<IPEndPoint> endpoints = new ConcurrentBag<IPEndPoint>();
            SLRM.Servers.Clear();
            foreach(Server s in m.Servers)
            {
                SLRM.Add(s.ChatServerEndpoint, s);
            }
            RegisteredServerManager MyRST = RegisteredServerManager.GetInstance();
            MyRST.Update(endpoints);
            Console.WriteLine("Received Chat Server Endpoints");
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
