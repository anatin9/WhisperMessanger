using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.Conversations;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Communication.MessageClasses;

namespace Whispr.RegistryServer.Conversations
{
    class ConnectToServerResponder : ChatServerResponder
    {
        private UserListResourceManager ULRM = UserListResourceManager.GetInstance();

        protected override void ExecuteDetails(object context)
        {
            ConnectToChatRequest incoming = (ConnectToChatRequest)context;

            ULRM.Add(RemoteEndPoint, incoming.User);

            var acknowledgement = new Acknowledge(ResponseCodes.SUCCESS, "true");
            var envelope = new Envelope() { Message = acknowledgement, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send back acknowledgement";
        }
    }
}
