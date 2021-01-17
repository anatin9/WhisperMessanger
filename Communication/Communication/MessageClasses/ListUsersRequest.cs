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
    public class ListUsersRequest : Message
    {
        [JsonProperty]
        private GroupId GroupId;

        public ListUsersRequest() { }

        public ListUsersRequest(GroupId groupId) : base(true)
        {
            GroupId = groupId;
        }
    }
}
