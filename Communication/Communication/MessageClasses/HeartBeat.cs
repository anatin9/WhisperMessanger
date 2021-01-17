using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HeartBeat : Message
    {
        [JsonProperty]
        public override int KeepAlive { get; set; }

        public HeartBeat() : base(true) { }
        public HeartBeat(int keepAlive) : base(true)
        {
            KeepAlive = keepAlive;
        }
    }
}
