//----------------------------------------------------------------------------------
// Rep Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace Rep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using CommandLine;
    using ZeroMQ;

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            using (var context = ZmqContext.Create())
            {
                using (var socket = context.CreateSocket(SocketType.REP))
                {
                    foreach (var bindEndPoint in options.bindEndPoints)
                        socket.Bind(bindEndPoint);
                    while (true)
                    {
                        Thread.Sleep(options.delay);
                        var rcvdMsg = socket.Receive(Encoding.UTF8);
                        Console.WriteLine("Received: " + rcvdMsg);
                        var replyMsg = options.replyMessage.Replace("#msg#", rcvdMsg);
                        Console.WriteLine("Sending : " + replyMsg + Environment.NewLine);                        
                        socket.Send(replyMsg, Encoding.UTF8);
                    }
                }
            }
        }     
    }
}
