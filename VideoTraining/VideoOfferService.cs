using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCExtend.VideoTraining
{
    class VideoOfferService
    {
        //Use to pass instance of ProcessIcon invoked from Program to VideoOfferService
        public ProcessIcon processIcon;
        private int balloonTipTime = 5000;

        //Logging
        private Logging logging = Logging.Instance;
        //Timers
        private System.Timers.Timer TimerVideoOffer;
        //Get instance of system idle timer
        SystemIdleTimer SystemIdleTimer1 = new SystemIdleTimer();
        int idleTime = 10;
        Video nextVideoUp;
        public VideoOfferService()
        {
            //initialise and start timer
            TimerVideoOffer = new System.Timers.Timer();
            TimerVideoOffer.Interval = 10000;//60000;
            TimerVideoOffer.Elapsed += TimerVideoOffer_Elapsed;
            TimerVideoOffer.Start();
            
            //Subscribe to SystemIdleTimer events
            SystemIdleTimer1.OnEnterIdleState += SystemIdleTimer1_OnEnterIdleState;
            SystemIdleTimer1.OnExitIdleState += SystemIdleTimer1_OnExitIdleState;

            SystemIdleTimer1.MaxIdleTime = Convert.ToUInt32(idleTime);
            SystemIdleTimer1.Start();

            nextVideoUp = NextVideoUp();

        }

        //TIMERS
        private void TimerVideoOffer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimerVideoOffer.Stop();
            logging.WriteToLog("TimerVideoOffer_Elapsed Timer elapsed!");
            Offer();
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

            if (activeHour)
            {
                logging.WriteToLog("Active hour and system has come out of idle period of " + idleTime + " seconds. Triggering toast to user!");
                BalloonTip("TEST IDLE!!", "Come out of idle state of " + idleTime, balloonTipTime, true);
                //Need to mark as triggered for that hour and day so hours might need to be their own class
            }
        }

        string preferedHours = "9,11,15,20,21,22,23"; //Need to mark as triggered for that hour and day so hours might need to be their own class
        bool activeHour;
        List<PreferredOfferHour> hours = new List<PreferredOfferHour>();
        private void Offer()
        {            
            //List<int> hours = preferedHours.Split(',').Select(int.Parse).ToList();           
            foreach (var hour in preferedHours.Split(',').Select(int.Parse).ToList())
            {
                hours.Add(new PreferredOfferHour
                {
                    Hour = hour                    
                });
            }

            var now = DateTime.Now;
            var nowHour = now.Hour;
            var currentActiveHour = hours.FirstOrDefault(h => h.Hour.Equals(nowHour));
            //hours.Any(h => h.Hour.Equals(now))
            if (currentActiveHour != null)
            {
                logging.WriteToLog("Active hour");
                activeHour = true;

                if (now.Minute >= 45 && !currentActiveHour.IsOfferedForHour)
                {
                    //It's 45m past an active hour and no offer has been made, so now one is :)
                    BalloonTip("TEST!!", "Its 45 past the hour!", balloonTipTime, false);                    
                }
            }
            else
            {
                logging.WriteToLog("Not active hour");
                activeHour = false;
            }

            
        }

        public Video NextVideoUp()
        {
            var videos = DataFactory.GetVideos();
            Video nextUp = videos.Where(w => w.IsWatched == false).OrderBy(o => o.OfferCount).FirstOrDefault();
            return nextUp;
        }

        NotifyIcon ni = new NotifyIcon();
        static string appIcon = Application.StartupPath + @"\Images\app_icon.ico";
        public void BalloonTip(string title, string message, int timeToShow, bool clickable)
        {
            //ni.BalloonTipTitle = title;
            //ni.BalloonTipText = message;
            //ni.Visible = true;
            //ni.ShowBalloonTip(timeToShow);
            //ni.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);
            //ni.BalloonTipIcon = ToolTipIcon.Error;

            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(appIcon), 
                //optional - BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                BalloonTipTitle = title,
                BalloonTipText = message
            };

            //Display for 5 seconds.
            notification.ShowBalloonTip(timeToShow);
            //Click event handler
            notification.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);

            // This will let the balloon close after it's 5 second timeout
            // for demonstration purposes. Comment this out to see what happens
            // when dispose is called while a balloon is still visible.
            //Thread.Sleep(10000);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            new VideoPlayer(VideoId: nextVideoUp.VideoId).ShowDialog();
        }

    }


}
