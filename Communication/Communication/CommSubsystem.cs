using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Communication.MessageClasses;

namespace Communication
{
    public class CommSubsystem
    {
        public ConversationFactory _conversationFactory;
        private int _minPort;
        private int _maxPort;
        public int TCPPort;
        private volatile bool stillRunning;

        public int Port => UdpCommunicator?.Port ?? 0;
        public ConversationDictionary Conversations { get; }
        public UdpCommunicator UdpCommunicator { get; private set; }
        public TcpCommunicator TcpCommunicator { get; private set; }

        // Communication Subsystem constructor

        public CommSubsystem(ConversationFactory factory, int minPort, int maxPort)
        {
            _conversationFactory = factory;
            _conversationFactory.ManagingSubsystem = this;

            _minPort = minPort;
            _maxPort = maxPort;

            stillRunning = true;
            Conversations = new ConversationDictionary();
        }

        //Starts the necessary parts of the Communication Subsystem

        public void Initialize()
        {
            _conversationFactory.Initialize();

            UdpCommunicator = new UdpCommunicator()
            {
                MinPort = _minPort,
                MaxPort = _maxPort,
                Timeout = 3000
            };
            UdpCommunicator.Start();

            if(TCPPort == 0)
            {
                TcpCommunicator = new TcpCommunicator()
                {
                    MinPort = _minPort,
                    MaxPort = _maxPort,
                    TCPPort = TCPPort
                };
                TcpCommunicator.Start();
            }
            else if(TCPPort != -1)
            {
                TcpCommunicator = new TcpCommunicator()
                {
                    MinPort = TCPPort,
                    MaxPort = TCPPort,
                    TCPPort = TCPPort,
                };
                TcpCommunicator.Start();
            }
            else
            {
                TcpCommunicator = null;
            }

            
            Thread _factoryThread = new Thread(Run);
            _factoryThread.Start();
            
        }

        public void End()
        {
            stillRunning = false;
            UdpCommunicator.Stop();
            TcpCommunicator.Stop();
        }

        public static string GetPublicIP()
        {
            return new WebClient().DownloadString("https://ipinfo.io/ip").Replace("\n", "");
        }

        public void Run()
        {
            Envelope envelope;
            Conversation conversation;

            while (stillRunning)
            {
                envelope = UdpCommunicator.Receive(1000);
                if(envelope == null && TcpCommunicator != null)
                {
                    envelope = TcpCommunicator.Receive(1000);
                }

                if (envelope != null)
                {
                    ConversationId id = envelope.Message.ConversationId;
                    conversation = Conversations.GetConversation(id);
                    
                    if (conversation != null) // If the conversation already exists
                    {
                        conversation.IncomingEnvelopes.Enqueue(envelope);
                    }
                    else // If the conversation does not exist
                    {
                        Responder newConversation;
                        newConversation = _conversationFactory.CreateFromEnvelope(envelope);
                        Message m = envelope.Message;
                        newConversation.Launch(m);
                        Conversations.Add(newConversation);
                    }
                }
            }
        }
    }
}
