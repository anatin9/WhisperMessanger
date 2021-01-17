using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Whispr.Client.Components;
using Whispr.Client.ResourceManagers;

namespace Whispr.Client.Conversations
{
    public class NewMessagesRequestInitiator : ClientInitiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(NewMessagesResponse) };
        private UserResourceManager URM = UserResourceManager.GetInstance();
        public EncryptionService ES { get; set; }
        private ChatServerResourceManager CSRM = ChatServerResourceManager.GetInstance();
        

        protected override Message CreateFirstMessage()
        {
            ChatServerManager CSM = CSRM.GetChatServerManager(RemoteEndPoint);
            string lastMessage = CSM.GetLastMessageId();
            Message m = new NewMessagesRequest(URM.GetUserId(), lastMessage);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            ChatServerManager CSM = CSRM.GetChatServerManager(RemoteEndPoint);
            NewMessagesResponse m = (NewMessagesResponse)env.Message;
            if (m.Messages != null)
            {
                foreach (EncryptedMessage message in m.Messages)
                {
                    Decrypt(message);
                    Console.WriteLine(message.PlainText);
                    CSM.AddIncomingMessage(message);
                }
                Console.WriteLine("Current Message Count: " + CSM.GetMessages().Count);
            }
        }

        void Decrypt(EncryptedMessage m)
        {
            m.PlainText = ES.AESDecrypt(m.EncryptedText, ES.RSADecrypt(m.SymmetricKey), m.IV);
        }
    }
}
