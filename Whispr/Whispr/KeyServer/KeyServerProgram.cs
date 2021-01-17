using System;
using Communication;
using Whispr.KeyServer;

namespace Whispr
{
    class KeyServerProgram : AppProcess
    {
        public KeyServerProgram()
        {
            ConversationFactory conversationFactory = new KeyServerConversationFactory();
            StartCommSubsystem(conversationFactory, 1026, 1026, 1027);
            Run();
        }

        private void Run()
        {
            
        }
    }
}