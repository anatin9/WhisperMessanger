using Communication.MessageClasses.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NewMessagesResponse : Message
    { 
        [JsonProperty]
        public override List<EncryptedMessage> Messages { get; set; }

        public NewMessagesResponse() { }
        public NewMessagesResponse(List<EncryptedMessage> messages) : base(true)
        {
            Messages = messages;
        }
    }
}
