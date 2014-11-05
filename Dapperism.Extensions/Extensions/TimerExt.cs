using System;
using System.Timers;

namespace Dapperism.Extensions.Extensions
{
    public static class TimerExt
    {
        private static Action _action;
        public static void Action(this Timer timer, TimeSpan time, Action action)
        {
            timer.Interval = time.TotalMilliseconds;
            timer.Start();
            _action = action;
            timer.Elapsed += timer_Elapsed;
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _action();
        }
    }
}
