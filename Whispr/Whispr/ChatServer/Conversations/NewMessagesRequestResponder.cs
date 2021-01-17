using Communication;
using Communication.MessageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;

namespace Whispr.ChatServer.Conversations
{
    public class NewMessagesRequestResponder : ChatServerResponder
    {
        private EncryptedMessageResourceManager EMRM = EncryptedMessageResourceManager.GetInstance();

        protected override void ExecuteDetails(object context)
        {
            NewMessagesRequest incoming = (NewMessagesRequest)context;

            var Messages = EMRM.GetMessages(incoming.UserId, incoming.LastMessage);
            NewMessagesResponse response = new NewMessagesResponse(Messages);
            var envelope = new Envelope() { Message = response, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send response";
        }
    }
}
