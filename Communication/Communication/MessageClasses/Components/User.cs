using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        [JsonProperty]
        public string UserId { get; set; }
        [JsonProperty]
        public GroupId GroupId { get; set; }

        // Local attributes that don't need to be passed
        public string UserName { get; set; } = null;
        public RSACryptoServiceProvider PublicKey = null;

        public User() { }
        public User(string _UserId, GroupId _GroupId) {
            UserId = _UserId;
            GroupId = _GroupId;
        }
    }
}
