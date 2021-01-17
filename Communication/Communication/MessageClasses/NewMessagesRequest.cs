using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NewMessagesRequest : Message
    {
        [JsonProperty]
        public string UserId { get; set; }

        [JsonProperty]
        public override string LastMessage { get; set; } = null;

        public NewMessagesRequest() { }
        public NewMessagesRequest(string userId, string lastMessage = null) : base(true) 
        {
            UserId = userId;
            LastMessage = lastMessage;
        }
    }
}
