using System;
using Communication;
using Communication.MessageClasses;

namespace Whispr.Client.Conversations
{
    public class ClientInitiator : Initiator
    {
        public ClientInitiator()
        {
        }

        protected override Type[] ExpectedReplyTypes => throw new NotImplementedException();

        protected override Message CreateFirstMessage()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            throw new NotImplementedException();
        }
    }
}
