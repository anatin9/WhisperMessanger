using System;
using System.Net;
using Communication;
using Communication.MessageClasses;

namespace Conversations
{
    public class HeartbeatInitiator : Initiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(Acknowledge) };

        protected override Message CreateFirstMessage()
        {
            Message m = new HeartBeat(30000);
            return m;
        }
        
        protected override void ProcessValidResponse(Envelope env)
        {
            MyState = State.Success;
            Console.WriteLine("Heartbeat Conversation was completed successfully.");
            return;
        }
    }
}
