using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UCExtend
{
    internal class AppTimer
    {
        private Timer timer;

        //Events
        public delegate void TimerElapsed();
        public event TimerElapsed Elapsed;

        //[Description("Event that if fired when idle state is entered.")]
        //public event OnEnterIdleStateEventHandler OnEnterIdleState;

        //public delegate void OnEnterIdleStateEventHandler(object sender, IdleEventArgs e);

        //[Description("Event that is fired when leaving idle state.")]
        //public event OnExitIdleStateEventHandler OnExitIdleState;

        //public delegate void OnExitIdleStateEventHandler(object sender, IdleEventArgs e);

        //private uint m_MaxIdleTime;
        //private object m_LockObject;

        /// <summary>
        /// Create timer and subscribe to events
        /// </summary>
        public AppTimer()
        {
            //m_LockObject = new object();
            timer = new Timer(TimerInterval);
            timer.Elapsed += AppTimer_Elapsed;

            //Default Values
            TimerInterval = 60000;
        }

        /// <summary>
        /// Timer run interval
        /// </summary>
        public double TimerInterval { get; set; }   


        /// <summary>
        /// Starts the timer running
        /// </summary>
        public void Start()
        {
            timer.Start();
        }


        /// <summary>
        /// Stops the timer from running
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            //lock (m_LockObject)
            //{
            //    m_IsIdle = false;
            //}
        }

        /// <summary>
        /// Returns true of false to indicate whether or not the timer is running
        /// </summary>
        public bool IsRunning
        {
            get { return timer.Enabled; }
        }

        private void AppTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed();
        }


        //    uint idleTime = Win32Wrapper.GetIdle();
            //    if (idleTime > (MaxIdleTime*1000))
            //    {
            //        if (m_IsIdle == false)
            //        {
            //            lock (m_LockObject)
            //            {
            //                m_IsIdle = true;
            //            }
            //            IdleEventArgs args = new IdleEventArgs(e.SignalTime);
            //            if (OnEnterIdleState != null)
            //            {
            //                OnEnterIdleState(this, args);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (m_IsIdle)
            //        {
            //            lock (m_LockObject)
            //            {
            //                m_IsIdle = false;
            //            }
            //            IdleEventArgs args = new IdleEventArgs(e.SignalTime);
            //            if (OnExitIdleState != null)
            //            {
            //                OnExitIdleState(this, args);
            //            }
            //        }
            //    }
            //}
        //}

        //public class IdleEventArgs : EventArgs
        //{

        //    private DateTime m_EventTime;

        //    public DateTime EventTime
        //    {
        //        get { return m_EventTime; }
        //    }

        //    public IdleEventArgs(DateTime timeOfEvent)
        //    {
        //        m_EventTime = timeOfEvent;
        //    }
        //}
    }
}
