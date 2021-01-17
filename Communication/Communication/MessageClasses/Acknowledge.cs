using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Acknowledge : Message
    {
        [JsonProperty]
        public override ResponseCodes Code { get; set; }
        [JsonProperty]
        public override string ResponseMessage { get; set; }

        public Acknowledge() { }
        public Acknowledge(ResponseCodes code, string message) : base(true) {
            Code = code;
            ResponseMessage = message;
        }
    }
}
