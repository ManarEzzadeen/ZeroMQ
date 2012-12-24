//----------------------------------------------------------------------------------
// Pull Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace Pull
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
                    using (var socket = ctx.CreateSocket(SocketType.PULL))
                    {
                        foreach (var endPoint in options.bindEndPoints)
                            socket.Bind(endPoint);

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
