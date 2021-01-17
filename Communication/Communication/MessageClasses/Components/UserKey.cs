using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.MessageClasses.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserKey
    {
        [JsonProperty]
        public string Username { get; }

        [JsonProperty]
        public string Publickey { get; }

        public UserKey(string username, string publickey)
        {
            Username = username;
            Publickey = publickey;
        }
    }
}
