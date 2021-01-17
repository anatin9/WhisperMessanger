using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.MessageClasses.Components
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EncryptedMessage
    {
        // No [JsonProperty] decorator - This should not be Json encoded as it is private
        public string PlainText { get; set; } = null;

        public EncryptedMessage() { }

        public EncryptedMessage Copy()
        {
            EncryptedMessage c = new EncryptedMessage();
            c.EncryptedText = EncryptedText;
            c.IV = IV;
            c.SenderId = SenderId;
            c.SymmetricKey = SymmetricKey;
            c.GroupId = GroupId;
            c.MessageId = MessageId;
            c.Timestamp = Timestamp;
            return c;
        }

        [JsonProperty]
        public string EncryptedText { get; set; } = null;
        [JsonProperty]
        public string IV { get; set; } = null;
        [JsonProperty]
        public string SenderId { get; set; } = null;
        [JsonProperty]
        public Dictionary<string, string> EncryptedSymmetricKeys = new Dictionary<string, string>();
        [JsonProperty]
        public string SymmetricKey = null;
        [JsonProperty]
        public string GroupId { get; set; }
        [JsonProperty]
        public string MessageId { get; set; } = null;
        [JsonProperty]
        public long Timestamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public bool Displayed { get; set; } = false;
    }
}
