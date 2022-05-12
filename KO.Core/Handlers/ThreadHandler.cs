using System;
using System.Threading;

namespace KO.Core.Handlers
{
    public class ThreadHandler
    {
        public static Thread Start(ThreadStart threadStart)
        {
            var thread = new Thread(threadStart) { IsBackground = true };
            thread.Start();

            return thread;
        }

        public static void QueueAndRun(Action action)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((object state) =>
            {
                action();
            }));
        }

        public static void QueueAndRun(Action action, WaitHandle resetEvent)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((object state) =>
            {
                var are = (AutoResetEvent)state;

                action();

                are.Set();
            }), resetEvent);
        }
    }
}
