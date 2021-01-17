using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Whispr.Client.Conversations;

namespace Whispr.Client.Components
{
    public class GetNewMessagesThread
    {
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();
        private ConversationFactory factory;
        private EncryptionService ES;
        private bool isRunning = true;

        public GetNewMessagesThread(ConversationFactory f, ref EncryptionService es)
        {
            factory = f;
            ES = es;
            Thread _MessagesUpdate = new Thread(Run);
            _MessagesUpdate.Start();
        }

        public void StopThread()
        {
            isRunning = false;
        }

        private void Run()
        {
            while (isRunning)
            {
                Console.WriteLine("Attempting to get new messages");
                if (SLRM.ActiveServer != null)
                {
                    NewMessagesRequestInitiator c = factory.CreateFromConversationType<NewMessagesRequestInitiator>();
                    c.RemoteEndPoint = SLRM.ActiveServer.ChatServerEndpoint;
                    c.ES = ES;
                    c.Launch();
                }
                Thread.Sleep(5000);
            }
        }
    }
}
