using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageClasses.Components;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)] 
    public class ConnectToChatRequest : Message
    {
        [JsonProperty]
        public User User { get; set; }

        public ConnectToChatRequest() { }

        public ConnectToChatRequest(User user) : base(true)
        {
            User = user;
        }
    }
}
