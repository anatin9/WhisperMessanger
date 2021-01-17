using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageClasses.Components;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ListServersResponse : Message
    { 
        [JsonProperty]
        public override List<Server> Servers { get; set; }

        public ListServersResponse() { }
        public ListServersResponse(List<Server> servers) : base(true)
        {
            Servers = servers;
        }
    }
}
