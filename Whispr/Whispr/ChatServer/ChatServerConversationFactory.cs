using System;
using Whispr.ChatServer.Conversations;
using Whispr.Client.Conversations;
using Communication;
using Communication.MessageClasses;
using Whispr.RegistryServer.Conversations;

namespace Whispr.ChatServer
{
    public class ChatServerConversationFactory : ConversationFactory
    {
        public ChatServerConversationFactory() {}

        public override void Initialize()
        {
            // Map Inconiming RegisterSelfResponse messages to the RegisterSelfResponder Conversation
            Add(typeof(HeartBeat), typeof(HeartbeatResponder));
            Add(typeof(ConnectToChatRequest), typeof(ConnectToServerResponder));
            Add(typeof(SendMessageRequest), typeof(SendMessageResponder));
            Add(typeof(NewMessagesRequest), typeof(NewMessagesRequestResponder));
            Add(typeof(ListUsersRequest), typeof(ListUsersRequestResponder));
        }
    }
}
