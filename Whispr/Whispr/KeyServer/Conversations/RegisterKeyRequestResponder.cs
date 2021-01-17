using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.KeyServer.Conversations;
using Whispr.KeyServer.ResourceManagers;

namespace Whispr.KeyServer
{
    public class RegisterKeyRequestResponder : KeyServerResponder
    {
        protected override void ExecuteDetails(object context)
        {
            RegisterKeyRequest incoming = (RegisterKeyRequest)context;

            Tuple<bool, string, string, UserKey> result = KeyResourceManager.GetInstance().AddUserKey(incoming.Id, incoming.UserKey);
            if (result.Item1 == true)
            {
                var acknowledgement = new RegisterKeyResponse(result.Item3, result.Item4);
                acknowledgement.Code = ResponseCodes.SUCCESS;
                acknowledgement.Message = result.Item2;
                var envelope = new Envelope() { Message = acknowledgement, EndPoint = RemoteEndPoint };
                if (!Send(envelope))
                    Error = "Cannot send back acknowledgement";
            }
            else
            {
                var acknowledgement = new RegisterKeyResponse(result.Item3, result.Item4);
                acknowledgement.Code = ResponseCodes.FAIL;
                acknowledgement.Message = result.Item2;
                var envelope = new Envelope() { Message = acknowledgement, EndPoint = RemoteEndPoint };
                if (!Send(envelope))
                    Error = "Cannot send back acknowledgement";
            }
        }
    }
}
