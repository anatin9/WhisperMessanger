using System;
using System.Collections.Concurrent;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace Communication
{
    public class ConversationDictionary
    {
        private readonly ConcurrentDictionary<string, Conversation> _activeConversation =
            new ConcurrentDictionary<string, Conversation>();

        public void Add(Conversation conversation)
        {
            if (conversation == null) return;

            var existingConversation = GetConversation(conversation.ConversationId);
            if (existingConversation == null)
                _activeConversation.TryAdd(conversation.ConversationId.ToString(), conversation);
        }

        private object GetConversation(object convId)
        {
            throw new NotImplementedException();
        }

        public Conversation GetConversation(ConversationId conversationId)
        {
            Conversation conversation;

            _activeConversation.TryGetValue(conversationId.ToString(), out conversation);

            return conversation;
        }

        public void Remove(ConversationId conversationId)
        { 
        Conversation conversation;
            _activeConversation.TryRemove(conversationId.ToString(), out conversation);
        }

    }
}
