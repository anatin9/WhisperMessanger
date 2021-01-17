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
    public class RegisterServerRequest : Message
    {
        [JsonProperty]
        public override Server Server { get; set; } = new Server();

        public RegisterServerRequest() { }
        public RegisterServerRequest(Server server) : base(true) {
            Server = server;
        }
    }
}
