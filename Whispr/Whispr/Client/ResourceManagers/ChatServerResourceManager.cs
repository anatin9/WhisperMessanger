using Communication.MessageClasses.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whispr.Client.ResourceManagers;

namespace Whispr.Client.Components
{
    public class ChatServerResourceManager
    {
        private static ChatServerResourceManager _instance;

        private ChatServerResourceManager() { }
        private ConcurrentDictionary<string, ChatServerManager> Servers = new ConcurrentDictionary<string, ChatServerManager>();

        public static ChatServerResourceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ChatServerResourceManager();
            }
            return _instance;
        }

        public ChatServerManager GetChatServerManager(IPEndPoint EndPoint)
        {
            if (EndPoint == null)
            {
                return null;
            }

            if (!Servers.ContainsKey(EndPoint.ToString()))
            {
                Servers[EndPoint.ToString()]  = new ChatServerManager();
            }

            return Servers[EndPoint.ToString()];
        }
    }

    public class ChatServerManager
    {
        private ConcurrentDictionary<string, bool> ActiveUsers = new ConcurrentDictionary<string, bool>();
        private UserResourceManager URM = UserResourceManager.GetInstance();
        private SortedDictionary<long, EncryptedMessage> Messages = new SortedDictionary<long, EncryptedMessage>();
        private long MessageCounter = 0;
        private Object Lock = new object();
        private Object UserListLock = new object();

        public void AddIncomingMessage(EncryptedMessage m)
        {
            lock (Lock)
            {
                foreach (KeyValuePair<long, EncryptedMessage> kpv in Messages)
                {
                    if (kpv.Value.MessageId == m.MessageId)
                    {
                        return;
                    }
                }
                Messages[MessageCounter] = m;
                MessageCounter += 1;
            }
        }

        public List<EncryptedMessage> GetMessages(bool UndisplayedMessagesOnly = false)
        {
            List<EncryptedMessage> messages = new List<EncryptedMessage>();
            lock (Lock)
            {
                foreach (KeyValuePair<long, EncryptedMessage> kpv in Messages)
                {
                    if (UndisplayedMessagesOnly == true)
                    {
                        if (kpv.Value.Displayed == true)
                        {
                            continue;
                        }
                    }
                    messages.Add(kpv.Value);
                }
            }
            return messages;
        }

        public string GetLastMessageId()
        {
            if (Messages.Count == 0)
            {
                return null;
            }
            return Messages.Values.Last().MessageId;
        }

        public void UpdateActiveUsers(List<User> users)
        {
            lock (UserListLock)
            {
                ActiveUsers.Clear();
                foreach (User u in users)
                {
                    ActiveUsers[u.UserId] = true;
                }
            }
        }

        public List<User> GetUsers()
        {
            lock (UserListLock)
            {
                List<User> users = new List<User>();
                string name;
                User tmp;
                foreach (KeyValuePair<string, bool> kpv in ActiveUsers)
                {
                    name = URM.GetUsername(kpv.Key);
                    if (name == null)
                    {
                        continue;
                    }
                    tmp = new User(kpv.Key, null);
                    tmp.UserName = name;
                    tmp.PublicKey = URM.GetKey(kpv.Key);
                    if (tmp.PublicKey == null)
                    {
                        continue;
                    }
                    users.Add(tmp);
                }
                return users;
            } 
        }
    }
}
