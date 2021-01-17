using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.MessageClasses;

namespace Whispr.Client.Conversations
{
    public class LoginRequestInitiator : ClientInitiator
    {
        protected override Type[] ExpectedReplyTypes { get; } = { typeof(LoginResponse) };
        public string Username { get; set; }
        public string Password { get; set; }

        protected override Message CreateFirstMessage()
        {
            Message m = new LoginRequest(Username, Password);
            return m;
        }

        protected override void ProcessValidResponse(Envelope env)
        {
            LoginResponse m = (LoginResponse)env.Message;
            if (m.Status == AuthenticationCodes.AUTHENTICATED)
            {
                Console.WriteLine("Login Successful");
                MyState = State.Success;
                return;
            }
            else
            {
                Console.WriteLine("Invalid Login Credentials");
                MyState = State.Failed;
                return;
            }
        }
    }
}
