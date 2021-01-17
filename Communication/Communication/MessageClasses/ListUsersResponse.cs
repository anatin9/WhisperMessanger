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
    public class ListUsersResponse : Message
    {
        [JsonProperty]
        public List<User> Users;

        public ListUsersResponse() { }

        public ListUsersResponse(List<User> users) : base(true)
        {
            Users = users;
        }
    }
}
