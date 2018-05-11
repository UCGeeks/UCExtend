using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;

namespace UCExtend
{
    public class SystemIdleTimer : Component
    {
        private const double INTERNAL_TIMER_INTERVAL = 550;

        [Description("Event that if fired when idle state is entered.")]
        public event OnEnterIdleStateEventHandler OnEnterIdleState;

        public delegate void OnEnterIdleStateEventHandler(object sender, IdleEventArgs e);

        [Description("Event that is fired when leaving idle state.")]
        public event OnExitIdleStateEventHandler OnExitIdleState;

        public delegate void OnExitIdleStateEventHandler(object sender, IdleEventArgs e);

        private Timer ticker;
        private uint m_MaxIdleTime;
        private object m_LockObject;

        private bool m_IsIdle = false;

        [Description("Maximum idle time in seconds.")]
        public uint MaxIdleTime
        {
            get { return m_MaxIdleTime; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("MaxIdleTime must be larger then 0.");
                }
                else
                {
                    m_MaxIdleTime = value;
                }
            }
        }

        public SystemIdleTimer()
        {
            m_LockObject = new object();
            ticker = new Timer(INTERNAL_TIMER_INTERVAL);
            ticker.Elapsed += InternalTickerElapsed;
        }

        public void Start()
        {
            ticker.Start();
        }

        public void Stop()
        {
            ticker.Stop();
            lock (m_LockObject)
            {
                m_IsIdle = false;
            }
        }

        public bool IsRunning
        {
            get { return ticker.Enabled; }
        }

        private void InternalTickerElapsed(object sender, ElapsedEventArgs e)
        {
            uint idleTime = Win32Wrapper.GetIdle();
            if (idleTime > (MaxIdleTime*1000))
            {
                if (m_IsIdle == false)
                {
                    lock (m_LockObject)
                    {
                        m_IsIdle = true;
                    }
                    IdleEventArgs args = new IdleEventArgs(e.SignalTime);
                    if (OnEnterIdleState != null)
                    {
                        OnEnterIdleState(this, args);
                    }
                }
            }
            else
            {
                if (m_IsIdle)
                {
                    lock (m_LockObject)
                    {
                        m_IsIdle = false;
                    }
                    IdleEventArgs args = new IdleEventArgs(e.SignalTime);
                    if (OnExitIdleState != null)
                    {
                        OnExitIdleState(this, args);
                    }
                }
            }
        }
    }

    public class IdleEventArgs : EventArgs
    {

        private DateTime m_EventTime;

        public DateTime EventTime
        {
            get { return m_EventTime; }
        }

        public IdleEventArgs(DateTime timeOfEvent)
        {
            m_EventTime = timeOfEvent;
        }
    }

    public class Win32Wrapper
    {
        public struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO lii);

        public static uint GetIdle()
        {
            LASTINPUTINFO lii = new LASTINPUTINFO();
            lii.cbSize = Convert.ToUInt32((Marshal.SizeOf(lii)));
            GetLastInputInfo(ref lii);
            return Convert.ToUInt32(Environment.TickCount) - lii.dwTime;
        }
    }
}
