using Communication.MessageClasses.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Whispr.Client.ResourceManagers
{
    public class UserResourceManager
    {
        // Singleton stuff
        private static UserResourceManager _instance;
        private UserResourceManager() { }

        // ID of local user
        private string userId { get; set; } = null;

        // Username from login prompt
        public string Username { get; set; }

        // Stores User ids
        private static ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        // ID to username mappings
        private ConcurrentDictionary<string, string> Usernames = new ConcurrentDictionary<string, string>();

        // ID to public key mappings
        private ConcurrentDictionary<string, RSACryptoServiceProvider> PublicKeys = new ConcurrentDictionary<string, RSACryptoServiceProvider>();

        public static UserResourceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserResourceManager();
            }
            return _instance;
        }

        public void AddUser(User u)
        {
            if (u.UserId != null)
            {
                Users[u.UserId] = u;
            }
        }

        public void SetUserKey(string id, UserKey uKey)
        {
            SetUsername(id, uKey.Username);
            SetKey(id, uKey.Publickey);
        }

        private void SetUsername(string id, string name)
        {
            Usernames[id] = name;
        }

        public string GetUsername(string id)
        {
            if (Usernames.ContainsKey(id))
            {
                return Usernames[id];
            }
            return null;
        }

        private void SetKey(string id, string publickey)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(publickey);
            PublicKeys[id] = RSA;
        }

        public RSACryptoServiceProvider GetKey(string id)
        {
            if (!PublicKeys.ContainsKey(id))
            {
                return null;
            }
            return PublicKeys[id];
        }

        public string GetUserId(bool generate = false)
        {
            if (generate == true)
            {
                Guid gid = Guid.NewGuid();
                userId = gid.ToString();    
            }
            return userId;
        }

        public void SetUserId(Guid gid)
        {
            userId = gid.ToString();
        }

        public List<string> GetUsersWithoutKeys()
        {
            List<string> usersIds = new List<string>();
            foreach (KeyValuePair<string, User> kpv in Users)
            {
                if (PublicKeys.ContainsKey(kpv.Key) && Usernames.ContainsKey(kpv.Key))
                {
                    continue;
                }
                usersIds.Add(kpv.Key);
            }
            return usersIds;
        }

        // TODO - Update this to pull users for the active server only
        public Dictionary<string, RSACryptoServiceProvider> GetUsersWithKeys()
        {
            Dictionary<string, RSACryptoServiceProvider> users = new Dictionary<string, RSACryptoServiceProvider>();
            foreach(KeyValuePair<string, RSACryptoServiceProvider> kpv in PublicKeys)
            {
                users[kpv.Key] = kpv.Value;
            }
            return users;
        }
    }
}
