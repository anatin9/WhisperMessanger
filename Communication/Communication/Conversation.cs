using System;
using System.Linq;
using System.Threading;
using System.Net;
using System.Collections.Concurrent;

namespace Communication
{
    public abstract class Conversation
    {

        public  State MyState { get; protected set; } = State.NotInitialized;
        public CommSubsystem CommSubsystem { get; set; }
        public string Error { get; protected set; }
        public ConversationId ConversationId { get; internal set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public int MaxRetries { get; set; } = 3;
        public readonly ConcurrentQueue<Envelope> IncomingEnvelopes = new ConcurrentQueue<Envelope>();
        protected readonly AutoResetEvent IncomingEvent = new AutoResetEvent(false);
        public int Timeout { get; set; } = 3000;
        public virtual bool isTCP { get; set; } = false;

        public enum State
        {
            NotInitialized,
            Working,
            Failed,
            Success
        };

        public void Process(Envelope env)
        {
            if (env?.Message == null || env.EndPoint == null) return;

            IncomingEnvelopes.Enqueue(env);
            IncomingEvent.Set();
        }

        public virtual void Launch(object context = null)
        {
            var result = ThreadPool.QueueUserWorkItem(Execute, context);
        }

        public void Execute(object context = null)
        {
            if (Initialize())
                ExecuteDetails(context);

            if (string.IsNullOrEmpty(Error))
                MyState = State.Success;
            else
            {
                MyState = State.Failed;
            }
        }

        protected virtual bool Initialize()
        {
            MyState = State.Working;
            return true;
        }

        protected abstract void ExecuteDetails(object context);

        protected bool IsEnvelopeValid(Envelope env, params Type[] allowedTypes)
        {
            return true; //TODO This probably is bad
        }

        protected Envelope DoReliableRequestReply(Envelope outgoingEnv, params Type[] allowedTypes)
        {
            if (isTCP)
            {
                return DoTCPRequestReply(outgoingEnv, allowedTypes);
            }

            Envelope incomingEnvelope = null;

            var remainingSends = MaxRetries;
            while (remainingSends > 0 && incomingEnvelope == null)
            {
                remainingSends--;

                if (!CommSubsystem.UdpCommunicator.Send(outgoingEnv))
                {
                    Error = "Cannot send message";
                    break;
                }

                if (IncomingEnvelopes.IsEmpty)
                    IncomingEvent.WaitOne(Timeout);

                var gotOne = IncomingEnvelopes.TryDequeue(out incomingEnvelope);

                if (gotOne && !IsEnvelopeValid(incomingEnvelope, allowedTypes))
                    incomingEnvelope = null;
            }

            if (Error != null)
                Console.WriteLine(Error);

            return incomingEnvelope;
        }

        protected Envelope DoTCPRequestReply(Envelope outgoingEnv, params Type[] allowedTypes)
        {
            Envelope incomingEnvelope = null;

            var remainingSends = MaxRetries;
            while (remainingSends > 0 && incomingEnvelope == null)
            {
                remainingSends--;

                if (!CommSubsystem.TcpCommunicator.Send(outgoingEnv))
                {
                    Error = "Cannot send message";
                    continue;
                }

                if (IncomingEnvelopes.IsEmpty)
                    IncomingEvent.WaitOne(Timeout);

                var gotOne = IncomingEnvelopes.TryDequeue(out incomingEnvelope);

                if (gotOne && !IsEnvelopeValid(incomingEnvelope, allowedTypes))
                    incomingEnvelope = null;
            }

            if (Error != null)
                Console.WriteLine(Error);
            
            return incomingEnvelope;
        }

    }
}
