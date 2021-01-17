using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Conversations;

namespace Communication.MessageClasses.Components
{
    public class HeartbeatThread
    {
        public IPEndPoint EndPoint { get; set; } = null;
        private ConversationFactory Factory;

        public HeartbeatThread(ConversationFactory factory)
        {
            Factory = factory;
            Thread _HeartbeatThread = new Thread(Run);
            _HeartbeatThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                if (EndPoint != null)
                {
                    HeartbeatInitiator c = Factory.CreateFromConversationType<HeartbeatInitiator>();
                    c.RemoteEndPoint = EndPoint;
                    c.Launch();
                }
                Thread.Sleep(10000);
            }
        }
    }
}
