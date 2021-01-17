using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageClasses
{
    public class MessageTypes
    {
        private static Dictionary<int, Type> _typeMappings = new Dictionary<int, Type>() {
            { 0, typeof(Unknown) },
            { 1, typeof(RegisterServerRequest) },
            { 2, typeof(SendMessageRequest) },
            { 3, typeof(PublicKeysRequest) },
            { 4, typeof(PublicKeysResponse) },
            { 5, typeof(RegisterKeyRequest) },
            { 6, typeof(HeartBeat) },
            { 7, typeof(ListServersRequest) },
            { 8, typeof(ListServersResponse) },
            { 9, typeof(NewMessagesRequest) },
            { 10, typeof(NewMessagesResponse) },
            { 11, typeof(Acknowledge) },
            { 12, typeof(LoginRequest) },
            { 13, typeof(LoginResponse) },
            { 14, typeof(ConnectToChatRequest) },
            { 15, typeof(ListUsersRequest) },
            { 16, typeof(ListUsersResponse) },
            { 17, typeof(RegisterKeyResponse) }
        };

        public static int ToInt(Type t)
        {
            foreach(KeyValuePair<int, Type> kpv in _typeMappings)
            {
                if (t == kpv.Value)
                {
                    return kpv.Key;
                }
            }
            return 0;
        }

        public static Type ToType(int index)
        {
            if (_typeMappings.ContainsKey(index))
            {
                return _typeMappings[index];
            }
            return typeof(Unknown);
        }
    }
    
}
