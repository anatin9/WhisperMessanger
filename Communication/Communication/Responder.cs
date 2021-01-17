using System;
using System.Net;
using System.Threading;

namespace Communication
{
    public class Responder : Conversation
    {
        private ConversationId ConvId;

        public Envelope IncomingEnvelope { get; set; }


        //public IPEndPoint RemoteEndPoint { get; private set; }

        protected override void ExecuteDetails(object context)
        {
            throw new NotImplementedException();
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            ConvId = IncomingEnvelope?.Message?.ConversationId;
            if (ConvId != null)
            {
                RemoteEndPoint = IncomingEnvelope?.EndPoint;
                //State = PossibleState.Working;
            }
            else
                Error = $"Cannot initialize {GetType().Name} conversation because ConvId in incoming message is null";

            return (Error == null);
        }

        protected bool Send(Envelope env)
        {
            env.Message.ConversationId = ConversationId;
            if (isTCP)
            {
                return CommSubsystem.TcpCommunicator.Reply(env);
            }
            else
            {
                return CommSubsystem.UdpCommunicator.Send(env);
            }
        }
    }
}
