using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.RegistryServer.Conversations;

namespace Whispr.RegistryServer
{
    class ListServersRequestResponder : RegistryServerResponder
    {
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();

        protected override void ExecuteDetails(object context)
        {
            Message incomingMessage = (ListServersRequest)context;
            RegisteredServerManager MyRST = RegisteredServerManager.GetInstance();

            List<Server> servers = new List<Server>();
            foreach (KeyValuePair<string, Tuple<Server, long>> kpv in SLRM.Servers)
            {
                servers.Add(kpv.Value.Item1);
            }

            var response = new ListServersResponse(servers);
            var envelope = new Envelope() { Message = response, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send back response.";
        }
    }
}
