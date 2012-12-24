//----------------------------------------------------------------------------------
// SyncSub Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace SyncSub
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
        const string SYNC = "Sync";

        static void Main(string[] args)
        {
            try
            {
                var options = new Options();
                var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
                if (!parser.ParseArguments(args, options))
                    Environment.Exit(1);

                using (var ctx = ZmqContext.Create())
                {
                    // Simulate late arrivals
                    Thread.Sleep(options.delay);

                    // Create and connect SUB socket
                    var subSocket = ctx.CreateSocket(SocketType.SUB);
                    subSocket.Connect(options.subEndpoint);
                    subSocket.SubscribeAll();
                    
                    // Receive Sync messqage
                    var pubMsg = subSocket.ReceiveMessage();
                    if (Encoding.UTF8.GetString(pubMsg[0]) == SYNC)
                    {
                        Console.WriteLine("SUB; received: " + Encoding.UTF8.GetString(pubMsg[0]));
                        using (var reqSocket = ctx.CreateSocket(SocketType.REQ))
                        {
                            reqSocket.Connect(Encoding.UTF8.GetString(pubMsg[1]));
                            DisplayReqMsg("REQ; sending : Sync me");
                            reqSocket.Send("Sync me", Encoding.UTF8);
                            var repMsg = reqSocket.Receive(Encoding.UTF8);
                            DisplayReqMsg("REQ; received: " + repMsg);
                        }
                    }
                                     
                    // Receive published messages
                    while (true)
                    {                        
                        pubMsg = subSocket.ReceiveMessage();
                        if (Encoding.UTF8.GetString(pubMsg[0]) != SYNC)
                        {
                            Console.WriteLine(
                                    "SUB; received: " +
                                    Encoding.UTF8.GetString(pubMsg[1]));
                        }
                    }                   
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }            
        }

        static void DisplayReqMsg(string msg)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }
    }
}
