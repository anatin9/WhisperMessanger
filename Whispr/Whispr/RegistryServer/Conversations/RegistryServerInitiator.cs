using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;

namespace Whispr.RegistryServer.Conversations
{
    class RegistryServerInitiator : Initiator
    {
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
