//----------------------------------------------------------------------------------
// Push Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace Push
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
                    using (var socket = ctx.CreateSocket(SocketType.PUSH))
                    {
                        foreach (var endPoint in options.bindEndPoints)
                            socket.Bind(endPoint);

                        long msgCptr = 0;
                        int msgIndex = 0;
                        while (true)
                        {
                            if (msgCptr == long.MaxValue)
                                msgCptr = 0;
                            msgCptr++;
                            if (options.maxMessage >= 0)
                                if (msgCptr > options.maxMessage)
                                    break;
                            if (msgIndex == options.altMessages.Count())
                                msgIndex = 0;
                            var msg = options.altMessages[msgIndex++].Replace("#nb#", msgCptr.ToString("d2"));
                            Thread.Sleep(options.delay);
                            Console.WriteLine("Pushing: " + msg);
                            socket.Send(msg, Encoding.UTF8);
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
