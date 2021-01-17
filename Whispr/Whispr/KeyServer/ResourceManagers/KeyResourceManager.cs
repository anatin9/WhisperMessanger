using Communication.MessageClasses.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispr.KeyServer.ResourceManagers
{
    public class KeyResourceManager
    {
        private static KeyResourceManager _instance;
        private KeyResourceManager() { }

        private ConcurrentDictionary<string, UserKey> UserKeys = new ConcurrentDictionary<string, UserKey>();
        private ConcurrentDictionary<string, string> Usernames = new ConcurrentDictionary<string, string>();

        public static KeyResourceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new KeyResourceManager();
            }
            return _instance;
        }

        public Tuple<bool, string, string, UserKey> AddUserKey(string id, UserKey userKey)
        {
            string CleanId = id.Trim();
            string CleanUsername = userKey.Username.Trim();

            if (Usernames.ContainsKey(CleanUsername))
            {
                return new Tuple<bool, string, string, UserKey>(false, "Username already exists. Pick a different one.", null, null);
            }
            if (UserKeys.ContainsKey(CleanId))
            {
                return new Tuple<bool, string, string, UserKey>(false, "User ID already exists. Pick a different one.", null, null);
            }
            if (CleanUsername.Length < 3)
            {
                return new Tuple<bool, string, string, UserKey>(false, "Username is too short. Pick a different one.", null, null);
            }

            if (CleanId.Length != 36)
            {
                return new Tuple<bool, string, string, UserKey>(false, "User ID is invalid. Pick a different one.", null, null);
            }

            UserKeys[CleanId] = userKey;
            Usernames[CleanUsername] = id;
            return new Tuple<bool, string, string, UserKey>(true, "Success", CleanId, new UserKey(CleanUsername, userKey.Publickey));
        }

        public List<Tuple<string, UserKey>> GetUserKeys(List<string> ids)
        {
            List<Tuple<string, UserKey>> keys = new List<Tuple<string, UserKey>>();
            foreach (string id in ids)
            {
                if (UserKeys.ContainsKey(id))
                {
                    keys.Add(new Tuple<string, UserKey>(id, UserKeys[id]));
                }
            }
            return keys;
        }
    }
}
