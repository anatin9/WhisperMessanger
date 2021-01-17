using System;
using System.Collections.Generic;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace Communication
{
    public abstract class ConversationFactory
    {
        private Dictionary<Type, Type> _typeMappings = new Dictionary<Type, Type>();

        public CommSubsystem ManagingSubsystem { get; set; }
        public int DefaultMaxRetries { get; set; } = 3;
        public int DefaultTimeout { get; set; } = 3000;

        public abstract void Initialize();

        // This method adds program specific messageType and conversationType
        protected void Add(Type messageType, Type conversationType)
        {
            if (messageType == null || !typeof(Message).IsAssignableFrom(messageType))
            {
                throw new ApplicationException("Invalid message type: " + messageType.ToString() + " must be a specialization of Message");
            }

            if (conversationType == null || !typeof(Conversation).IsAssignableFrom(conversationType))
            {
                throw new ApplicationException("Invalid conversation type: "+ conversationType.ToString() + " must be a specialization of ResponderConversation");
            }

            if (!_typeMappings.ContainsKey(messageType))
                _typeMappings.Add(messageType, conversationType);
        }

        //Creates a conversation from an Envelope
        public virtual Responder CreateFromEnvelope(Envelope envelope)
        {
            Responder conversation = null;
            var messageType = envelope?.Message?.GetType();

            if (messageType != null && _typeMappings.ContainsKey(messageType))
            {
                conversation = CreateResponderConversation(_typeMappings[messageType], envelope);
            }
            else
            {
                Console.WriteLine("WARNING! Null conversation detected. ABORT!");
            }
            
            return conversation;
        }

        // Create conversation from type
        public virtual T CreateFromConversationType<T>() where T : Initiator, new()
        {
            var conversation = new T()
            {
                CommSubsystem = ManagingSubsystem,
                MaxRetries = DefaultMaxRetries,
                Timeout = DefaultTimeout,
                ConversationId = null
            };
            return conversation;
        }

        //Creates a conversation from a Responder
        protected virtual Responder CreateResponderConversation(Type conversationType, Envelope envelope = null)
        {
            if (conversationType == null || String.IsNullOrEmpty(envelope?.Message?.ConversationId?.ToString()))
            {
                return null;
            }

            var conversation = Activator.CreateInstance(conversationType) as Responder;
            if (conversation == null)
            {
                return null;
            }

            conversation.CommSubsystem = ManagingSubsystem;
            conversation.ConversationId = envelope.Message.ConversationId;
            conversation.IncomingEnvelope = envelope;
            return conversation;
        }
    }

}
