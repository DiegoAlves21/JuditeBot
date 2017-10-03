using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Client
    {
        public int Id { get; set; }
        public string toId { get; set; }
        public string toName { get; set; }
        public string fromId { get; set; }
        public string fromName { get; set; }
        public string serviceUrl { get; set; }
        public string channelId { get; set; }
        public string conversationId { get; set; }
    }
}
