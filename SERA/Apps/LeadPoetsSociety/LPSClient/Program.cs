using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPSClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string character = "";
            if (args.Length > 0)
            {
                character = args[0];
            }
            LPSClient server = new LPSClient(character);
            Console.WriteLine(instructions);
            Console.WriteLine("\nPress a key to close...\n\n");
            Console.ReadLine();
            server.Dispose();
        }

        static string instructions =
@"INSTRUCTIONS:
This example routes messages received via Thalamus to an XmlRpc client (running in Unity).
It also routes messages received on an XmlRpc server (from unity) back to Thalamus.
In order to test it, you can launch this program, your Unity program, and ThalamusStandalone.
Navigate to the ''Events'' tab in ThalamusStandalone
Make sure you open the Event Log, by clicking the ''EventLog'' button.
Send out the SentFromThalamusToUnity action.
The Unity client should respond, and you should see a SentFromUnityToThalamus message received back in the Event Log
";

    }
}
