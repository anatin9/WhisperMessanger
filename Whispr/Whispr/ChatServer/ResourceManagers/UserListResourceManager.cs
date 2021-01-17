using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageClasses.Components;

namespace Whispr.ChatServer.ResourceManagers
{
    public class UserListResourceManager
    {
        public static UserListResourceManager _instance = null;
        public ConcurrentDictionary<string, Tuple<User, long>> Users { get; internal set; } = new ConcurrentDictionary<string, Tuple<User, long>>();

        private UserListResourceManager() { }

        public void Add(IPEndPoint newEndPoint, User newUser)
        {
            Users[newEndPoint.ToString()] = new Tuple<User, long>(newUser, DateTimeOffset.Now.ToUnixTimeSeconds());
        }

        public void Delete(string deleted)
        {
            Tuple <User, long> output;
            if (!Users.TryRemove(deleted, out output))
            {
                Console.WriteLine("User removal failed.");
            }
        }

        public static UserListResourceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserListResourceManager();
            }
            return _instance;
        }
    }
}
