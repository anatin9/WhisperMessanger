using Communication.MessageClasses.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispr.ChatServer.ResourceManagers
{
    public class EncryptedMessageResourceManager
    {
        private static EncryptedMessageResourceManager _instance;
        private SortedDictionary<long, EncryptedMessage> Messages = new SortedDictionary<long, EncryptedMessage>();
        private long MessageCounter = 0;
        private Object Lock = new object();

        private EncryptedMessageResourceManager() { }

        public static EncryptedMessageResourceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EncryptedMessageResourceManager();
            }
            return _instance;
        }

        public void AddIncomingMessage(EncryptedMessage m)
        {
            lock (Lock)
            {
                Messages[MessageCounter] = m;
                MessageCounter += 1;
            }
        }

        public string GetLastMessageId()
        {
            if (Messages.Count == 0)
            {
                return null;
            }
            return Messages.Values.Last().MessageId;
        }

        public List<EncryptedMessage> GetMessages(string UserId, string MessageId = null)
        {
            List<EncryptedMessage> messages = new List<EncryptedMessage>();
            lock (Lock)
            {
                bool StopSkipping = false;
                foreach (KeyValuePair<long, EncryptedMessage> kpv in Messages)
                {
                    Console.WriteLine();
                    if (!kpv.Value.EncryptedSymmetricKeys.ContainsKey(UserId))
                    {
                        continue;
                    }
                    if (!StopSkipping)
                    {
                        if (MessageId != null && kpv.Value.MessageId != MessageId)
                        {
                            continue;
                        }
                        else
                        {
                            StopSkipping = true;
                            if (kpv.Value.MessageId == MessageId)
                            {
                                continue;
                            }
                        }  
                    }
                    EncryptedMessage copy = kpv.Value.Copy();
                    copy.SymmetricKey = kpv.Value.EncryptedSymmetricKeys[UserId];
                    copy.EncryptedSymmetricKeys = null;
                    messages.Add(copy);
                }
            }
            return messages;
        }
    }
}
