using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.KeyServer.Conversations;
using Whispr.KeyServer.ResourceManagers;

namespace Whispr.KeyServer
{
    class PublicKeysRequestResponder : KeyServerResponder
    {
        KeyResourceManager KRM = KeyResourceManager.GetInstance();
        public override bool isTCP { get; set; } = true;

        protected override void ExecuteDetails(object context)
        {
            PublicKeysRequest incoming = (PublicKeysRequest)context;
            List<Tuple<string, UserKey>> keys = KRM.GetUserKeys(incoming.Users);
            var response = new PublicKeysResponse(keys);
            var envelope = new Envelope() { Message = response, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send response";
        }
    }
}
