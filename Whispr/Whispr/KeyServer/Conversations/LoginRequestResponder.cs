using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;

namespace Whispr.KeyServer.Conversations
{
    class LoginRequestResponder : KeyServerResponder
    {
        protected override void ExecuteDetails(object context)
        {
            LoginRequest incoming = (LoginRequest)context;

            var response = new LoginResponse(AuthenticationCodes.AUTHENTICATED, "YourToken");
            var envelope = new Envelope() { Message = response, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send response";
        }
    }
}
