using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace Whispr.ChatServer.Conversations
{
    public class RegisterServerInitiator : ChatServerInitiator
    {
        private UserListResourceManager ULRM = UserListResourceManager.GetInstance();
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(Acknowledge) };
        public IPEndPoint MyIPEndpoint;
        public Server Server { get; set; } = new Server();

        protected override Message CreateFirstMessage()
        {
            Message m = new RegisterServerRequest(Server);
            Server.ActiveUsers = ULRM.Users.Count;
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            MyState = State.Success;
            Console.WriteLine("Conversation was completed successfully.");
            return;
        }
    }
}
