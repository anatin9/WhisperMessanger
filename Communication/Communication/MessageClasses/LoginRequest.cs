using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginRequest : Message
    {
        [JsonProperty]
        private string Username;
        [JsonProperty]
        private string Password;

        public LoginRequest() { }
        public LoginRequest(string username, string password) : base(true)
        {
            Username = username;
            Password = password;
        }
    }
}
