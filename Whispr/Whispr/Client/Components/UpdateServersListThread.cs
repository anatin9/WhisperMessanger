using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whispr.Client.Conversations;

namespace Whispr.Client.Components
{
    public class UpdateServersListThread
    {
        private bool isRunning = true;
        private ConversationFactory factory;

        public UpdateServersListThread(ConversationFactory f)
        {
            factory = f;
            Thread _ServerListUpdate = new Thread(Run);
            _ServerListUpdate.Start();
        }

        public void StopThread()
        {
            isRunning = false;
        }

        private void Run()
        {
            while (isRunning)
            {
                Console.WriteLine("Getting Servers from Registry");
                ListServersRequestInitiator c = factory.CreateFromConversationType<ListServersRequestInitiator>();
                c.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024); // TODO - Make dynamic
                c.Launch();
                Thread.Sleep(10000);
            }
        }
    }
}
