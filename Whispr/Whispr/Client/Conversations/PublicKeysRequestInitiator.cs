using System;
using System.Collections.Generic;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.Client.ResourceManagers;

namespace Whispr.Client.Conversations
{
    public class PublicKeysRequestInitiator : ClientInitiator
    {
        public List<string> users { get; set; } = new List<string>();
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(PublicKeysResponse) };
        public override bool isTCP { get; set; } = true;

        protected override Message CreateFirstMessage()
        {
            Message m = new PublicKeysRequest(users);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            PublicKeysResponse m = (PublicKeysResponse)env.Message;
            UserResourceManager URM = UserResourceManager.GetInstance();
            if (m.Keys != null)
            {
                foreach (Tuple<string, UserKey> item in m.Keys)
                {
                    URM.SetUserKey(item.Item1, item.Item2);
                }
            }
            MyState = State.Success;
            Console.WriteLine("PublicKeysRequest Conversation Complete");
            return;
        }
    }
}
