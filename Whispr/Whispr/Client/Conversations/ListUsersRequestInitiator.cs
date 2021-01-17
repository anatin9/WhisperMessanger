using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.Client.Components;
using Whispr.Client.ResourceManagers;
using Whispr.RegistryServer;

namespace Whispr.Client.Conversations
{
    class ListUsersRequestInitiator : ClientInitiator
    {
        private UserResourceManager URM = UserResourceManager.GetInstance();
        private ChatServerResourceManager CSRM = ChatServerResourceManager.GetInstance();
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(ListUsersResponse) };

        public GroupId GroupId { get; set; } = null;

        protected override Message CreateFirstMessage()
        {
            Message m = new ListUsersRequest(GroupId);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            ListUsersResponse m = (ListUsersResponse)env.Message;
            ChatServerManager CSM = CSRM.GetChatServerManager(RemoteEndPoint);
            if (m.Users != null)
            {
                foreach (User u in m.Users)
                {
                    URM.AddUser(u);
                }
                CSM.UpdateActiveUsers(m.Users);
                Console.WriteLine("Received User List");
                Console.WriteLine("Active Users: " + CSM.GetUsers().Count);
            }
        }
    }
}
