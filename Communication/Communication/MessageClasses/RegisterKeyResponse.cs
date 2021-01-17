using Communication.MessageClasses.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RegisterKeyResponse : Message
    {
        [JsonProperty]
        public ResponseCodes Code { get; set; }
        [JsonProperty]
        public string Message { get; set; }
        [JsonProperty]
        public string UserId { get; set; }
        [JsonProperty]
        public UserKey UserKey { get; set; }

        public RegisterKeyResponse(string userId, UserKey userKey) : base(true)
        {
            UserId = userId;
            UserKey = userKey;
        }
    }
}
