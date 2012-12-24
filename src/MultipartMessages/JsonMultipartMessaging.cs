//----------------------------------------------------------------------------------
// JsonMultipartMessaging
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

    class JsonMultipartMessaging
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
                        var shoppingBasket = new ShoppingBasket()
                        {
                            StoreName = "Fruits City",
                            ShoppingItems = new List<ShoppingItem>() {
                                new ShoppingItem() { Description="Orange", Weight=0.5f},
                                new ShoppingItem() { Description="Apple", Weight=1.4f},
                                new ShoppingItem() { Description="Banana", Weight=0.75f}}
                        };
                        ZmqMessage zmqMessage = new ZmqMessage();
                        zmqMessage.Append(
                                   new Frame(Encoding.UTF8.GetBytes("Shopping Basket")));
                        zmqMessage.Append(
                                   JsonFrame.Serialize<ShoppingBasket>(shoppingBasket));                      
                        Console.WriteLine("PUB; publishing: ");
                        Console.WriteLine("\t" + Encoding.UTF8.GetString(zmqMessage[0]));
                        Console.WriteLine("\t" + Encoding.UTF8.GetString(zmqMessage[1]));                        
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
                        var msgTitle = Encoding.UTF8.GetString(zmqMessage[0]);
                        ShoppingBasket shoppingBasket = JsonFrame
                                                        .DeSerialize<ShoppingBasket>
                                                         (zmqMessage[1]);
                        Console.WriteLine("SUB; Received: ");
                        Console.WriteLine("\t" + msgTitle);
                        Console.WriteLine("\t" + Encoding.UTF8.GetString(zmqMessage[1]));
                    }
                }
            }
        }       
    }
}
