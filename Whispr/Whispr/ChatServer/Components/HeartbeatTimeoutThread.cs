using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Communication.MessageClasses.Components;

namespace Whispr.ChatServer.Components
{
    public class HeartbeatTimeoutThread
    {
        private UserListResourceManager ULRM = UserListResourceManager.GetInstance();

        private static long TIMEOUT = 30;

        public HeartbeatTimeoutThread()
        {
            Thread HeartbeatTimeout = new Thread(Run);
            HeartbeatTimeout.Start();
        }

        private void Run()
        {
            while (true)
            {
                long now = DateTimeOffset.Now.ToUnixTimeSeconds();
                foreach (KeyValuePair<string, Tuple<User, long>> kpv in ULRM.Users)
                {
                    if (now - kpv.Value.Item2 > TIMEOUT)
                    {
                        Console.WriteLine("Deleting user...");
                        ULRM.Delete(kpv.Key);
                    }
                }
                Thread.Sleep(5000);
            }
        }
    }
}
