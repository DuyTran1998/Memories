using System;
namespace Memories.Model
{
    public class ResponseMessage
    {
        public bool status { set; get; }
        public String message { set; get; }

        public ResponseMessage(bool status, String message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
