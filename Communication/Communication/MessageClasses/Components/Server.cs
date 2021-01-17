using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Server
    {

        [JsonProperty]
        private string ChatServerEndPoint { get; set; }

        public IPEndPoint ChatServerEndpoint 
        {
            get { return ToIPEndPoint(ChatServerEndPoint); }
            set { ChatServerEndPoint = value.ToString(); }
        }

        [JsonProperty]
        public string Hostname { get; set; }
        [JsonProperty]
        public int ActiveUsers { get; set; }

        private static IPEndPoint ToIPEndPoint(string EndPoint)
        {
            string[] IP_Port = EndPoint.Split(':');
            IPAddress addr = IPAddress.Parse(IP_Port[0]);
            int port = Int32.Parse(IP_Port[1]);
            return new IPEndPoint(addr, port);
        }
    }
}
