using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Whispr.Client.Components;
using Whispr.Client.ResourceManagers;

namespace Whispr.Client.Conversations
{
    class SendMessageRequestInitiator : ClientInitiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(Acknowledge) };
        private UserResourceManager URM = UserResourceManager.GetInstance();
        private ChatServerResourceManager CSRM = ChatServerResourceManager.GetInstance();
        public string Message { get; set; }
        public string GroupID { get; set; } = null;
        public EncryptionService ES { get; set; } = null;

        protected override Message CreateFirstMessage()
        {
            SendMessageRequest m = new SendMessageRequest(GroupID, Message, URM.GetUserId());
            m.SecureMessage.MessageId = m.MessageId.ToString();
            Encrypt(m.SecureMessage);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            Acknowledge m = (Acknowledge)env.Message;
            if (m.Code == ResponseCodes.SUCCESS)
            {
                Console.WriteLine("Chat Message recieved");
                MyState = State.Success;
                return;
            }
            else
            {
                Console.WriteLine("Error Sending Chat Message");
                MyState = State.Failed;
                return;
            }
        }

        private void Encrypt(EncryptedMessage m)
        {
            var CSM = CSRM.GetChatServerManager(RemoteEndPoint);
            Tuple<string, string, string> DataKeyIV = ES.AESEncrypt(m.PlainText);
            m.EncryptedText = DataKeyIV.Item1;
            m.IV = DataKeyIV.Item3;

            foreach(User u in CSM.GetUsers())
            {
                m.EncryptedSymmetricKeys[u.UserId] = ES.RSAEncrypt(DataKeyIV.Item2, u.PublicKey);
            }
        }
    }
}
