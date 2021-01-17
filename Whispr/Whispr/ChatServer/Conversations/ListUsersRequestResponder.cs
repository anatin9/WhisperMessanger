using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whispr.ChatServer.ResourceManagers;
using Communication;
using Communication.MessageClasses;
using Communication.MessageClasses.Components;

namespace Whispr.ChatServer.Conversations
{
    class ListUsersRequestResponder : ChatServerResponder
    {
        UserListResourceManager ULRM = UserListResourceManager.GetInstance();

        protected override void ExecuteDetails(object context)
        {
            Message incoming = (ListUsersRequest)context;

            List<User> users = new List<User>();
            foreach (KeyValuePair<string, Tuple<User, long>> kvp in ULRM.Users)
            {
                users.Add(kvp.Value.Item1);
            }

            var response = new ListUsersResponse(users);
            var envelope = new Envelope() { Message = response, EndPoint = RemoteEndPoint };
            if (!Send(envelope))
                Error = "Cannot send response";
        }
    }
}
