using Newtonsoft.Json;
using Communication.MessageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Communication.MessageClasses.Components;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SendMessageRequest : Message
    {
        [JsonProperty]
        public EncryptedMessage SecureMessage;

        public SendMessageRequest() { }

        public SendMessageRequest(string gid, string text, string senderId) : base(true)
        {
            SecureMessage = new EncryptedMessage();
            SecureMessage.PlainText = text;
            SecureMessage.GroupId = gid;
            SecureMessage.SenderId = senderId;
        }
    }
}
