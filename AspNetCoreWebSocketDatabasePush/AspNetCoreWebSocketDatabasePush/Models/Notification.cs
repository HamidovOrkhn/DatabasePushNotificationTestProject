using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebSocketDatabasePush.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
