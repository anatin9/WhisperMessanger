using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ListServersRequest : Message
    {
        public ListServersRequest() {}

        public ListServersRequest(bool generateId) : base(true) {}
    }
}
