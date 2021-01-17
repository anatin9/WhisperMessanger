using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace Whispr.Client.Conversations
{
    public class ConnectToChatInitiator : ClientInitiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(Acknowledge) };
        public User User { get; set; } = new User();

        protected override Message CreateFirstMessage()
        {
            Message m = new ConnectToChatRequest(User);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            Acknowledge m = (Acknowledge)env.Message;
            Console.WriteLine("Received Acknowledge for Connecting to Chat Server");
        }
    }
}