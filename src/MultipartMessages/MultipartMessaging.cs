//----------------------------------------------------------------------------------
// MultipartMessaging
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace MultipartMessages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using ZeroMQ;

    class MultipartMessaging
    {
        private BackgroundWorker subThread;

        public void Start()
        {
            subThread = new BackgroundWorker();
            subThread.DoWork += new DoWorkEventHandler(subThread_DoWork);
            subThread.RunWorkerAsync();
            
            using (var ctx = ZmqContext.Create())
            {
                using(var socket = ctx.CreateSocket(SocketType.PUB))
                {
                    socket.Bind("tcp://127.0.0.1:5000");
                    while (true)
                    {
                        Thread.Sleep(1000);
                        // Create a ZmqMessage containing 3 frames
                        ZmqMessage zmqMessage = new ZmqMessage();
                        zmqMessage.Append(new Frame(Encoding.UTF8.GetBytes("My Frame 01")));
                        zmqMessage.Append(new Frame(Encoding.UTF8.GetBytes("My Frame 02")));
                        zmqMessage.Append(new Frame(Encoding.UTF8.GetBytes("My Frame 03")));
                        Console.WriteLine("PUB; publishing: ");
                        foreach (var msg in zmqMessage)                        
                            Console.WriteLine("\t" + Encoding.UTF8.GetString(msg));                        
                        socket.SendMessage(zmqMessage);
                    }
                }
            }
        }

        void subThread_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var ctx = ZmqContext.Create())
            {
                using(var socket = ctx.CreateSocket(SocketType.SUB))
                {
                    socket.Connect("tcp://127.0.0.1:5000");
                    socket.SubscribeAll();
                    while (true)
                    {
                        var zmqMessage = socket.ReceiveMessage();
                        var frameContents = zmqMessage
                                            .Select(f => Encoding.UTF8
                                                                 .GetString(f.Buffer))
                                                                 .ToList();
                        Console.WriteLine("SUB; Received: ");
                        foreach (var frameContent in frameContents)
                        {
                            Console.WriteLine("\t" + frameContent);
                        }
                    }
                }
            }
        }       
    }
}
