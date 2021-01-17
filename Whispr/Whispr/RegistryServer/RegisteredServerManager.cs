using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Whispr.RegistryServer
{
    class RegisteredServerManager
    {
        private static RegisteredServerManager _instance = null;
        public ConcurrentBag<IPEndPoint> endpoints { get; set; }
        public ConcurrentBag<IPEndPoint> newEndpoints { get; set; }

        public RegisteredServerManager()
        {
            endpoints = new ConcurrentBag<IPEndPoint>();
            newEndpoints = new ConcurrentBag<IPEndPoint>();
        }


        public void Add(IPEndPoint newEndpoint)
        {
            newEndpoints.Add(newEndpoint);
        }

        public void Update()
        {
            foreach (IPEndPoint newEndpoint in newEndpoints)
            {
                endpoints.Add(newEndpoint);
            }
            newEndpoints = new ConcurrentBag<IPEndPoint>();
        }

        public void Update(ConcurrentBag<IPEndPoint> endpointsToKeep)
        {
            endpoints = endpointsToKeep;

            foreach (IPEndPoint newEndpoint in newEndpoints)
            {
                endpoints.Add(newEndpoint);
            }

            newEndpoints = new ConcurrentBag<IPEndPoint>();
        }

    public static RegisteredServerManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RegisteredServerManager();
            }
            return _instance;
        }
    }
}
