using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageClasses;

namespace Communication
{
    public class HeartbeatResponder : Responder
    {
        protected override void ExecuteDetails(object context)
        {
            var acknowledgement = new Acknowledge(ResponseCodes.SUCCESS, "true");
            var envelope = new Envelope() { Message = acknowledgement, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send back acknowledgement";
        }
    }
}
