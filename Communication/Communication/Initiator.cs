using System;
using Communication.MessageClasses;

namespace Communication
{
    public abstract class Initiator : Conversation
    {
        protected Envelope FirstEnvelope { get; set; }
        
        protected abstract Type[] ExpectedReplyTypes { get; }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            FirstEnvelope = null;

            var msg = CreateFirstMessage();
            ConversationId = msg.ConversationId;
            CommSubsystem.Conversations.Add(this);
            if (msg == null) return false;

            FirstEnvelope = new Envelope() { Message = msg, EndPoint = RemoteEndPoint };
            return true;
        }

        //This method needs to be implemented by concrete specializations
        protected abstract Message CreateFirstMessage();

        private bool IsValidResponseType(int t)
        {
            Type realType = MessageTypes.ToType(t);
            foreach (Type type in ExpectedReplyTypes)
            {
                if (type == realType)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void ExecuteDetails(object context)
        {
            var env = DoReliableRequestReply(FirstEnvelope, ExpectedReplyTypes);

            if (env == null)
                Error = "No response received";
            else
            {
                if (IsValidResponseType(env.Message.MessageType))
                {
                    ProcessValidResponse(env);
                }
                else
                {
                    Console.WriteLine("Invalid Message Response Type");
                }
            }
        }

        private void ProcessValidResponse(object env)
        {
            throw new NotImplementedException();
        }
        
        protected abstract void ProcessValidResponse(Envelope env);
    }
}
