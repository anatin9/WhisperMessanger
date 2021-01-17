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
    public class RegisterKeyRequest : Message
    {
        [JsonProperty]
        public string Id;
        [JsonProperty]
        public override UserKey UserKey { get; set; }

        public RegisterKeyRequest() { }

        public RegisterKeyRequest(string id, UserKey uKey) : base(true)
        {
            Id = id;
            UserKey = uKey;
        }
    }
}
