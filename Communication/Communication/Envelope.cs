using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using Communication.MessageClasses;

namespace Communication
{
    public class Envelope
    {
        public Message Message { get; set; }
        public bool IsValidToSend { get; internal set; }
        public IPEndPoint EndPoint { get; set; }

        public Envelope()
        {
            IsValidToSend = true; // This should be set in some validation method later
        }
        
        public Tuple<byte[], int> Encode()
        {
            byte[] Data = Encoding.UTF8.GetBytes(Message.JSON());
            return new Tuple<byte[], int>(Data, Data.Length);
        }

        public void Decode(byte[] data)
        {
            Message = MessageFromJSONFactory.GetMessage(Encoding.UTF8.GetString(data));
        }
    }

}