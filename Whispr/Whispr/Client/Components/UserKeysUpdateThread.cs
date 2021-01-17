using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Whispr.Client.Conversations;
using Whispr.Client.ResourceManagers;

namespace Whispr.Client.Components
{
    public class UserKeysUpdateThread
    {
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();
        private UserResourceManager URM = UserResourceManager.GetInstance();
        private ConversationFactory factory;
        private IPEndPoint KeyServerIPEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1027);
        private bool isRunning = true;
        public UserKeysUpdateThread(ConversationFactory f)
        {
            factory = f;
            Thread _UserUpdate = new Thread(RunUsersUpdate);
            Thread _KeysUpdate = new Thread(RunKeysUpdate);
            _UserUpdate.Start();
            _KeysUpdate.Start();

        }

        public void StopThread()
        {
            isRunning = false;
        }

        public void RunUsersUpdate()
        {
            while (isRunning)
            {
                if (SLRM.ActiveServer != null)
                {
                    ListUsersRequestInitiator c = factory.CreateFromConversationType<ListUsersRequestInitiator>();
                    c.RemoteEndPoint = SLRM.ActiveServer.ChatServerEndpoint;
                    c.Launch();
                    Thread.Sleep(5000);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void RunKeysUpdate()
        {
            while (isRunning)
            {
                List<string> UserIds = URM.GetUsersWithoutKeys();
                if (UserIds.Count > 0)
                {
                    PublicKeysRequestInitiator c = factory.CreateFromConversationType<PublicKeysRequestInitiator>();
                    c.RemoteEndPoint = KeyServerIPEndpoint;
                    c.users = UserIds;
                    c.Launch();
                    Thread.Sleep(5000);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
