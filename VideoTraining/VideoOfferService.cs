using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCExtend.VideoTraining
{
    public class VideoOfferService
    {
        //Logging
        private Logging logging = Logging.Instance;
        //Timers
        private System.Timers.Timer TimerVideoOffer;
        //Get instance of system idle timer
        SystemIdleTimer SystemIdleTimer1 = new SystemIdleTimer();

        public VideoOfferService()
        {
            //initialise and start timer
            TimerVideoOffer = new System.Timers.Timer();
            TimerVideoOffer.Interval = 60000;
            TimerVideoOffer.Elapsed += TimerVideoOffer_Elapsed;
            TimerVideoOffer.Start();
            
            //Subscribe to SystemIdleTimer events
            SystemIdleTimer1.OnEnterIdleState += SystemIdleTimer1_OnEnterIdleState;
            SystemIdleTimer1.OnExitIdleState += SystemIdleTimer1_OnExitIdleState;

            SystemIdleTimer1.MaxIdleTime = Convert.ToUInt32(5);
            SystemIdleTimer1.Start();

        }

        //TIMERS
        private void TimerVideoOffer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimerVideoOffer.Stop();
            logging.WriteToLog("TimerVideoOffer_Elapsed Timer elapsed!");
            TimerVideoOffer.Start();
        }

        //EVENT System idle timer - On idle timeout met
        public void SystemIdleTimer1_OnEnterIdleState(Object sender, IdleEventArgs e)
        {
            logging.WriteToLog("SystemIdleTimer1_OnEnterIdleState triggered!");
        }

        //EVENT System idle timer - On idle end
        public void SystemIdleTimer1_OnExitIdleState(Object sender, IdleEventArgs e)
        {
            logging.WriteToLog("SystemIdleTimer1_OnExitIdleState triggered!");
        }

        private void Offer()
        {
            
            
        }


    }

    
}
