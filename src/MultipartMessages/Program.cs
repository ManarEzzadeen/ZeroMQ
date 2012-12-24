//----------------------------------------------------------------------------------
// Main
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace MultipartMessages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            //MultipartMessaging multipartMessaging = new MultipartMessaging();
            //multipartMessaging.Start();

            JsonMultipartMessaging jsonMultipartMessaging = new JsonMultipartMessaging();
            jsonMultipartMessaging.Start();
        }
    }
}
