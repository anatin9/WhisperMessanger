using System;
using System.Net;
using Communication;
using Communication.MessageClasses;
using Whispr.RegistryServer.Conversations;

namespace Whispr.RegistryServer
{
    public class RegistryServerConversationFactory : ConversationFactory
    {
        public IPEndPoint[] ChatServerEndpoints;
        public RegistryServerConversationFactory() {}

        public override void Initialize()
        {
            Add(typeof(RegisterServerRequest), typeof(RegisterServerResponder));
            Add(typeof(ListServersRequest), typeof(ListServersRequestResponder));
        }
    }
}
