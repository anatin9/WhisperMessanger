using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;
using Whispr.Client.ResourceManagers;

namespace Whispr.Client.Conversations
{
    public class RegisterKeyRequestInitiator : ClientInitiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(RegisterKeyResponse) };
        private UserResourceManager URM = UserResourceManager.GetInstance();
        public string Key { get; set; }

        protected override Message CreateFirstMessage()
        {
            string Id = UserResourceManager.GetInstance().GetUserId(true);
            string User = UserResourceManager.GetInstance().Username;
            UserKey uKey = new UserKey(User, Key);
            Message m = new RegisterKeyRequest(Id, uKey);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            RegisterKeyResponse m = (RegisterKeyResponse)env.Message;
            if (m.Code == ResponseCodes.SUCCESS)
            {
                UserResourceManager.GetInstance().SetUserId(new Guid(m.UserId));
                URM.SetUserKey(m.UserId, m.UserKey);
                MyState = State.Success;
                Console.WriteLine("Conversation Complete");
            }
            else
            {
                MyState = State.Failed;
                Console.WriteLine("Conversation Failed: Server Responded with: " + m.Message);
            }
            return;
        }
    }
}
