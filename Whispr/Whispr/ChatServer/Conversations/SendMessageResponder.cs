using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;

namespace Whispr.ChatServer.Conversations
{
    class SendMessageResponder : ChatServerResponder
    {
        private UserListResourceManager ULRM = UserListResourceManager.GetInstance();
        private EncryptedMessageResourceManager EMRM = EncryptedMessageResourceManager.GetInstance();

        protected override void ExecuteDetails(object context)
        {
            SendMessageRequest incoming = (SendMessageRequest)context;

            if (incoming.SecureMessage != null)
            {
                Console.WriteLine("!! Encrypted Message: " + incoming.SecureMessage.EncryptedText);
                EMRM.AddIncomingMessage(incoming.SecureMessage);
            }

            var response = new Acknowledge(ResponseCodes.SUCCESS, "Recieved new chat message.");
            var envelope = new Envelope() { Message = response, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send response";
        }
    }
}
