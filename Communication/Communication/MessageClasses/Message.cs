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
    public abstract class Message
    {
        [JsonProperty]
        public MessageId MessageId { get; set; }
        [JsonProperty]
        public ConversationId ConversationId { get; set; }
        [JsonProperty]
        public int MessageType { get; private set; }

        public virtual string ChatServerEndpoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual Server Server { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string KeyServerEndpoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual UserKey UserKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string LastMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string ResponseMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual List<string> Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual List<Tuple<string, UserKey>> Keys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual List<Server> Servers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual ResponseCodes Code { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual int KeepAlive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual List<EncryptedMessage> Messages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string EncryptedMessageText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual string GroupId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Message() {}

        public Message(bool generateId)
        {
            this.setType();
            MessageId = new MessageId();
            ConversationId = new ConversationId();
        }

        public string JSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        private void setType()
        {
            MessageType = MessageTypes.ToInt(this.GetType());
            if (MessageType == 0)
            {
                // If this triggers, remember to go add your Message Class Type to the MessageTypes.cs file.
                throw new UnknownMessageTypeException();
            }
        }
    }
}
