using System;

namespace Communication
{
    //Base class for every program class

    public abstract class AppProcess
    {
        public CommSubsystem MyCommSubsystem { get; protected set;}

        protected virtual void StartCommSubsystem(ConversationFactory conversationFactory, int minPort, int maxPort, int TCPPort = -1)
        {
            MyCommSubsystem = new CommSubsystem(conversationFactory, minPort, maxPort);
            MyCommSubsystem.TCPPort = TCPPort;
            MyCommSubsystem.Initialize();
        }

        protected virtual void EndCommSubsystem()
        {
            MyCommSubsystem.End();
        }

    }
}
