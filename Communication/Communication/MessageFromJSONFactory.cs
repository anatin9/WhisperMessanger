using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageClasses;

namespace Communication
{
    public class MessageFromJSONFactory
    {
        public static Message GetMessage(string jsonData)
        {
            JObject m = JsonConvert.DeserializeObject<JObject>(jsonData);
            int type = (int)m["MessageType"];
            return (Message)JsonConvert.DeserializeObject(jsonData, MessageTypes.ToType(type));
        }
    }
}
