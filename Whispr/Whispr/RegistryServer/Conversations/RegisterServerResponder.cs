using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace Whispr.RegistryServer.Conversations
{
    class RegisterServerResponder : RegistryServerResponder
    {
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();

        protected override void ExecuteDetails(object context)
        {
            RegisterServerRequest incoming = (RegisterServerRequest)context;
            Server s = incoming.Server;
            s.ChatServerEndpoint = RemoteEndPoint;
            SLRM.Add(RemoteEndPoint, s);

            //RegisteredServerManager MyRSM = RegisteredServerManager.GetInstance();
            //MyRSM.Add(CreateIPEndPoint(incoming.ChatServerEndpoint));

            var acknowledgement = new Acknowledge(ResponseCodes.SUCCESS, "true");
            var envelope = new Envelope() { Message = acknowledgement, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send back acknowledgement";
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
