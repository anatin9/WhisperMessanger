using System;
using Communication;
using Communication.MessageClasses;
using Whispr.KeyServer.Conversations;

namespace Whispr.KeyServer
{
    public class KeyServerConversationFactory : ConversationFactory
    {
        public KeyServerConversationFactory() {}

        public override void Initialize()
        {
            Add(typeof(PublicKeysRequest), typeof(PublicKeysRequestResponder));
            Add(typeof(RegisterKeyRequest), typeof(RegisterKeyRequestResponder));
            Add(typeof(LoginRequest), typeof(LoginRequestResponder));
        }
    }
}
