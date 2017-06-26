using System;
using System.Threading;
using System.Timers;

namespace Spam
{
    public static class main
    {
        public static bool killed;
        public static int X;
        public static int Y;
        public static ushort clickHeader;
        static Spam spam;
        static System.Timers.Timer timer = new System.Timers.Timer(100);
        static bool enabled = false;
        static int interval = 100;
        public static void init(Spam spm)
        {
            spam = spm;
            clickHeader = spam.Game.GetMessageHeader(spam.Game.GetMessages("5dec6a7881d4a598d5b15d0e743bcdcb")[0]);
            timer.Elapsed += (s, e) => spam.Connection.SendToServerAsync(clickHeader, X, Y);
            //timer.AutoReset = false;
        }
        public static void Start()
        {
            timer.Enabled = true;
            //if (enabled) return;
            //enabled = true;
            //new Thread(new ThreadStart(() =>
            //{
            //    while (enabled)
            //    {
            //        ClicksOn(null, null);
            //        Thread.Sleep(interval);
            //    }
            //})).Start();
        }
        public static void Stop()
        {
            timer.Enabled = false;
            //enabled = false;
        }
        public static void SetInterval(int intv)
        {
            timer.Interval = intv;
            //interval = intv;
        }

        public static void Dispose()
        {
            timer.Stop();
            timer.Close();
        }
    }
}
