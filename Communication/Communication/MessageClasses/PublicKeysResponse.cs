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
    public class PublicKeysResponse : Message
    {
        [JsonProperty]
        public override List<Tuple<string, UserKey>> Keys { get; set; }

        public PublicKeysResponse() { }

        public PublicKeysResponse(List<Tuple<string, UserKey>> keys) : base(true)
        {
            Keys = keys;
        }
    }
}
