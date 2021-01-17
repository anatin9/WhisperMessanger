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
    public class ServerListResourceManager
    {
        public static ServerListResourceManager _instance = null;
        public ConcurrentDictionary<string, Tuple<Server, long>> Servers { get; internal set; } = new ConcurrentDictionary<string, Tuple<Server, long>>();
        public Server ActiveServer { get; set; } = null;

        private ServerListResourceManager() { }

        public void Add(IPEndPoint newEndPoint, Server newServer)
        {
            Servers[newEndPoint.ToString()] = new Tuple<Server, long>(newServer, DateTimeOffset.Now.ToUnixTimeSeconds());
        }

        public void Delete(string deleted)
        {
            Tuple<Server, long> output;
            if (!Servers.TryRemove(deleted, out output))
            {
                Console.WriteLine("Server removal failed.");
            }
        }

        public static ServerListResourceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServerListResourceManager();
            }
            return _instance;
        }

        public void SetActiveServer(IPEndPoint server)
        {
            if (Servers.ContainsKey(server.ToString()))
            {
                ActiveServer = Servers[server.ToString()].Item1;
            }
        }

        public void UnsetActiveServer()
        {
            ActiveServer = null;
        }
    }
}
