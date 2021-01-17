using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PublicKeysRequest : Message
    {
        [JsonProperty]
        public override List<string> Users { get; set; }

        public PublicKeysRequest() {}
        public PublicKeysRequest(List<string> users) : base(true)
        {
            Users = users;
        }
    }
}
