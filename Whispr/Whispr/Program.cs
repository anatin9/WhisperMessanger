using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Whispr.ChatServer;

namespace Whispr
{
    enum MODE : int { CLIENT = 0, CHATSERVER = 1, KEYSERVER = 2, REGISTRYSERVER = 3 };

    class Program
    {
        static void Main(string[] args)
        {
            /*
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
            }*/ 

            IDictionary<string, int> flags = ParseArgs(args);
            switch (flags["mode"])
            {
                case (int)MODE.CLIENT:
                    new ClientProgram();
                    break;
                case (int)MODE.CHATSERVER:
                    new ChatServerProgram();
                    break;
                case (int)MODE.KEYSERVER:
                    new KeyServerProgram();
                    break;
                case (int)MODE.REGISTRYSERVER:
                    new RegistryServerProgram();
                    break;
            }
        }

        static IDictionary<string, int> ParseArgs(string[] args)
        {
            int mode = (int)MODE.CLIENT;
            IDictionary<string, int> flags = new Dictionary<string, int>();
            for (int i = 0; i < args.Length; i++)
            {
                switch(args[i])
                {
                    case "--chat-server":
                        mode = (int)MODE.CHATSERVER;
                        break;
                    case "--key-server":
                        mode = (int)MODE.KEYSERVER;
                        break;
                    case "--registry-server":
                        mode = (int)MODE.REGISTRYSERVER;
                        break;
                    default:
                        break;
                }
            }
            flags["mode"] = mode;
            return flags;
        }
    }
}
