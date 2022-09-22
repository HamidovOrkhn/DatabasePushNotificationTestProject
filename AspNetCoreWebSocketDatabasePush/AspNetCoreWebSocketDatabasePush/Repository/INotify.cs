using AspNetCoreWebSocketDatabasePush.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebSocketDatabasePush.Repository
{
    public interface INotify
    {
        List<Notification> GetAll();
    }
}
