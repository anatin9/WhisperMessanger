using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginResponse : Message
    {
        [JsonProperty]
        public AuthenticationCodes Status { get; }
        [JsonProperty]
        public string Token { get; }

        public LoginResponse() { }
        public LoginResponse(AuthenticationCodes status, string token) : base(true)
        {
            Status = status;
            Token = token;
        }
    }

    public enum AuthenticationCodes
    {
        AUTHENTICATED,
        INVALID_LOGIN
    }
}
