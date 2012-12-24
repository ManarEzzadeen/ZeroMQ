//----------------------------------------------------------------------------------
// PullPush worker Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace PullPushWorker
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
                    using (ZmqSocket receiver = ctx.CreateSocket(SocketType.PULL),
                                        sender = ctx.CreateSocket(SocketType.PUSH))
                    {
                        receiver.Connect(options.pullEndPoint);
                        sender.Connect(options.pushEndPoint);

                        while (true)
                        {
                            var rcvdMsg = receiver.Receive(Encoding.UTF8);
                            Console.WriteLine("Pulled : " + rcvdMsg);
                            var sndMsg = options.rcvdMessageTag.Replace("#msg#", rcvdMsg);
                            Thread.Sleep(options.delay);
                            Console.WriteLine("Pushing: " + sndMsg);
                            sender.Send(sndMsg, Encoding.UTF8);
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
