//----------------------------------------------------------------------------------
// JsonFrame
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
    using ServiceStack.Text;
    using ZeroMQ;

    public static class JsonFrame
    {
        public static Frame Serialize<T>(T messageObject)
        {
            var message = JsonSerializer.SerializeToString<T>(
                                            messageObject);
            return new Frame(Encoding.UTF8.GetBytes(message));
        }

        public static T DeSerialize<T>(Frame frame)
        {
            var messageObject = JsonSerializer.DeserializeFromString<T>(
                                                Encoding
                                                .UTF8
                                                .GetString(frame.Buffer));
            return messageObject;
        }
    }
}
