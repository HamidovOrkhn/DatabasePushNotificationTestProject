using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace AspNetCoreWebSocketDatabasePush
{
  
        public class GlobalTimer
        {
            // ReSharper disable once NotAccessedField.Local
            private readonly Timer _timer;

            public GlobalTimer()
            {
                var now = DateTime.UtcNow;
                var delay = GetDelayToNextEvenSecond(now);

                _timer = new Timer(state => OnTimerTick(), null, delay, TimeSpan.Zero);
            }

            public event Action Tick;

            private void OnTimerTick()
            {
                Tick?.Invoke();
                var now = DateTime.UtcNow;
                var delay = GetDelayToNextEvenSecond(now);
                _timer.Change(delay, TimeSpan.Zero);
            }

            private static TimeSpan GetDelayToNextEvenSecond(DateTime time)
            {
                var evenSecond = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, 0, DateTimeKind.Utc);
                bool itIsAlmostNextSecond = time.Millisecond > 950;
                var nextSecond = evenSecond.AddSeconds(itIsAlmostNextSecond ? 2 : 1);
                var delay = nextSecond - time;
                return delay;
            }
        }
    }

