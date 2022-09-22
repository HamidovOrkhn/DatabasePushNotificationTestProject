
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCoreWebSocketDatabasePush.Models;
using AspNetCoreWebSocketDatabasePush.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace AspNetCoreWebSocketDatabasePush.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotify _rp;
        private readonly GlobalTimer _timer;


        public HomeController(ILogger<HomeController> logger, INotify rp,GlobalTimer globalTimer)
        {
            _logger = logger;
            _rp = rp;
            _timer = globalTimer;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult GetData()
        {
            return Ok(_rp.GetAll());
        }
        [HttpPost("api/time")]
        public async Task<string> IndexData([FromQuery] int ticks = 0)
        {
            var context = ControllerContext.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                await ProcessRequest(context.WebSockets, ticks);
                return null; // by this time the socket is closed, it does not matter what we return
            }

            return GetTime();
        }

        private static string GetTime()
        {
            var timeStr = DateTime.UtcNow.ToString("MMM dd yyyy HH:mm:ss.fff UTC", CultureInfo.InvariantCulture);
            var timeJson = JsonConvert.SerializeObject(timeStr);
            return timeJson;
        }

        private async Task ProcessRequest(WebSocketManager wsManager, int maxTicks)
        {
            using (var ws = await wsManager.AcceptWebSocketAsync())
            {
                var sender = new WebSocketSender(ws);
                int ticks = 0;

                void TickHandler()
                {
                    sender.QueueSend(GetTime());
                    if (maxTicks != 0 && ++ticks >= maxTicks) sender.CloseAsync();
                }

                _timer.Tick += TickHandler;
                try
                {
                    await sender.HandleCommunicationAsync();
                }
                finally
                {
                    _timer.Tick -= TickHandler;
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
