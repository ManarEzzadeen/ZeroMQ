//----------------------------------------------------------------------------------
// SyncPub Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace SyncPub
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
        static uint nbSubscribersConnected = 0;
        static Options options;
        static long msgCptr = 0;
        static int msgIndex = 0;

        static void Main(string[] args)
        {
             try
            {   
                options = new Options();
                var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
                if (!parser.ParseArguments(args, options))
                    Environment.Exit(1);
                         
                using (var ctx = ZmqContext.Create())
                {
                    var pubSocket = ctx.CreateSocket(SocketType.PUB);
                    pubSocket.Bind(options.pubEndpoint);
                    pubSocket.SendReady +=new EventHandler<SocketEventArgs>(pubSocket_SendReady);
                    var repSocket = ctx.CreateSocket(SocketType.REP);
                    repSocket.Bind(options.repEndpoint);
                    repSocket.SendReady +=new EventHandler<SocketEventArgs>(repSocket_SendReady);
                    repSocket.ReceiveReady +=new EventHandler<SocketEventArgs>(repSocket_ReceiveReady);
                    Poller poller = new Poller(new ZmqSocket[] {pubSocket, repSocket});
                    while (true)
                    {
                        poller.Poll();
                        if (options.maxMessage >= 0)
                            if (msgCptr > options.maxMessage)
                                Environment.Exit(0);
                    }                                                                  
                }
            }          
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }                       
        }

        #region REP events
        static void repSocket_ReceiveReady(object sender, SocketEventArgs e)
        {
            var reqMsg = e.Socket.Receive(Encoding.UTF8);
            DisplayRepMsg("REP, received: " + reqMsg);
        }

        static void repSocket_SendReady(object sender, SocketEventArgs e)
        {
            DisplayRepMsg("REP, sending: Sync OK");
            e.Socket.Send(Encoding.UTF8.GetBytes("Sync OK"));
            nbSubscribersConnected++;
        } 
        #endregion

        #region PUB events       
        static void pubSocket_SendReady(object sender, SocketEventArgs e)
        {
            var zmqMessage = new ZmqMessage();
            if (nbSubscribersConnected < options.nbExpectedSubscribers)
            {
                zmqMessage.Append(Encoding.UTF8.GetBytes("Sync"));
                zmqMessage.Append(Encoding.UTF8.GetBytes(options.repEndpoint));
                Thread.Sleep(options.delay);
                Console.WriteLine("Publishing: Sync");
            }
            else
            {
                zmqMessage.Append(Encoding.UTF8.GetBytes("Data"));
                var data = BuildDataToPublish();
                if (!string.IsNullOrEmpty(data))
                {
                    zmqMessage.Append(Encoding.UTF8.GetBytes(data));
                    Thread.Sleep(options.delay);
                    Console.WriteLine("Publishing (Data): " + data);
                }
            }
            e.Socket.SendMessage(zmqMessage);
        } 
        #endregion

        #region Build data to publish
        static string BuildDataToPublish()
        {
            if (msgCptr == long.MaxValue)
                msgCptr = 0;
            msgCptr++;
            if (options.maxMessage >= 0)
                if (msgCptr > options.maxMessage)
                    return "";
            if (msgIndex == options.altMessages.Count())
                msgIndex = 0;
            return options.altMessages[msgIndex++].Replace("#nb#", msgCptr.ToString("d2"));
        } 
        #endregion

        static void DisplayRepMsg(string msg)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }
    }
}
