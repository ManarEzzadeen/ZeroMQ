//----------------------------------------------------------------------------------
// Sub Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace Sub
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
            try
            {
                var options = new Options();
                var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
                if (!parser.ParseArguments(args, options))
                    Environment.Exit(1);

                using(var ctx = ZmqContext.Create())
                {
                    using (var socket = ctx.CreateSocket(SocketType.SUB))
                    {
                        if (options.subscriptionPrefixes.Count() == 0)
                            socket.SubscribeAll();
                        else
                            foreach (var subscriptionPrefix in options.subscriptionPrefixes)
                                socket.Subscribe(Encoding.UTF8.GetBytes(subscriptionPrefix));

                        foreach (var endPoint in options.connectEndPoints)
                            socket.Connect(endPoint);

                        while (true)
                        {
                            Thread.Sleep(options.delay);
                            var msg = socket.Receive(Encoding.UTF8);
                            Console.WriteLine("Received: " + msg);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
