using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Lync.Model;

namespace UCExtend
{
    class LyncPresenceSwitcherOLD
    {
        //Use to pass instance of ProcessIcon invoked from Program to LyncPresenceSwitcher
        public ProcessIcon processIcon;

        //Use to pass instance of xxx invoked from xxx to xxx
        public LyncClientController lync;

        private int balloonTipTime = 1500;
        
        //Logging
        private Logging logging = Logging.Instance;

        //Language Library
        private string processFriendlyName = "Lync Presence Switcher";

        //Get instance of LyncClientController
        //LyncClientController lync = new LyncClientController();
        
        //Get instance of system idle timer
        SystemIdleTimer SystemIdleTimer1 = new SystemIdleTimer();

        //Timers
        private System.Timers.Timer timerLyncClientChecks;
        private System.Timers.Timer timerPresenceSwitcher;
        
        //Holds the current date
        public static DateTime currentDate { get; set; }

        //Holds the SettingsBox saved time
        public static DateTime settingsSavedTime { get; set; }

        //Track active match
        private bool presenceTimer1Active, presenceTimer2Active, presenceTimer3Active, presenceTimer4Active, presenceTimer5Active, presenceTimerSetMeAs1Active, presenceTimerSetMeAs2Active = false;
        private bool presenceTimer1ActiveProcessed, presenceTimer2ActiveProcessed, presenceTimer3ActiveProcessed, presenceTimer4ActiveProcessed, presenceTimer5ActiveProcessed, presenceTimerSetMeAs1ActiveProcessed, presenceTimerSetMeAs2ActiveProcessed = false;

        public LyncPresenceSwitcherOLD()
        {
            //Date at run
            currentDate = DateTime.Now;

            //Get the current state of settings saved
            settingsSavedTime = SettingsBox.SettingsSavedTime;

            //Subscribe to SystemIdleTimer events
            SystemIdleTimer1.OnEnterIdleState += SystemIdleTimer1_OnEnterIdleState;
            SystemIdleTimer1.OnExitIdleState += SystemIdleTimer1_OnExitIdleState;

            //Initialise Timers
            timerLyncClientChecks = new System.Timers.Timer();
            timerLyncClientChecks.Interval = 5000;
            timerLyncClientChecks.Elapsed += TimerLyncClientChecks_Elapsed;
            timerLyncClientChecks.Start();

            timerPresenceSwitcher = new System.Timers.Timer();
            timerPresenceSwitcher.Interval = 5000;
            timerPresenceSwitcher.Elapsed += TimerPresenceSwitcher_Elapsed;
            

            //Start timer if Lync client is loaded
            //if (lync.LyncClientSignedIn)
            //{
            //    Timer60();
            //}
            //else if (!lync.LyncClientSignedIn)
            //{
                //TimerLyncClientChecks();
            //}
            //need to change so that the timer continues to monitor state of Lync client, so that if it is exited Timer60 is stopped!

            logging.WriteToLog(processFriendlyName + " started at " + currentDate);
        }

//TIMERS  ################################################################################################

        /// <summary>
        /// TimerLyncClient - Timer runs to check if Lync client is in a useable state
        /// </summary>      
        //private void TimerLyncClientChecks()
        //{
        //    //timerLyncClientChecks = new System.Timers.Timer();
        //    //timerLyncClientChecks.Interval = 10000;
        //    //timerLyncClientChecks.Elapsed += TimerLyncClientChecks_Elapsed;
        //    //timerLyncClientChecks.Start();

        //    //AppTimer xxx = new AppTimer();
        //    //xxx.TimerInterval = 5000;
        //    //xxx.Elapsed +=
        //}


        ////TimerLyncClient - timer elapsed event
        void TimerLyncClientChecks_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Stop timer and process
            //((System.Timers.Timer)sender).Stop();
            timerLyncClientChecks.Stop();

            //If the LyncClient is loaded, start the app processing timer
            if (lync.LyncClientLoaded && !timerPresenceSwitcher.Enabled)
            {
                //Start app processing timer
                //TimerSetPresence();
                timerPresenceSwitcher.Start();
            }
            else if (!lync.LyncClientLoaded && timerPresenceSwitcher.Enabled)
            {
                timerPresenceSwitcher.Stop();
            }

            //Start timer again
            //((System.Timers.Timer)sender).Start();
            timerLyncClientChecks.Start();

                        //((System.Timers.Timer)sender).Stop();

                        ////If the Lync client is still not loaded, start the timer again
                        //if (!lync.LyncClientSignedIn)
                        //{
                        //    ((System.Timers.Timer)sender).Start();
                        //}
                        //else if (lync.LyncClientSignedIn)
                        //{
                        //    Timer60();
                        //}
        }

        /// <summary>
        /// Triggers PresenceTimer() method on timer elapsed
        /// </summary>  
        //public void TimerSetPresence()
        //{
        //    //timerSetPresence = new System.Timers.Timer();
        //    //timerSetPresence.Interval = 5000;
        //    //timerSetPresence.Elapsed += TimerSetPresence_Elapsed;
        //    //timerSetPresence.Start();
        //}


        //timer elapsed event
        void TimerPresenceSwitcher_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Stop timer and process
            //((System.Timers.Timer)sender).Stop();
            timerPresenceSwitcher.Stop();

            lock (timerPresenceSwitcher)
            {
                ProcessPresenceSwitcher();
            }

            //Start timer again
            //((System.Timers.Timer)sender).Start();
            timerPresenceSwitcher.Start();
        }


        /// <summary>
        /// Resets the presence timer states
        /// </summary>
        public void ResetPresenceTimer()
        {
            presenceTimer1ActiveProcessed = false;
            presenceTimer2ActiveProcessed = false;
            presenceTimer3ActiveProcessed = false;
            presenceTimer4ActiveProcessed = false; 
            presenceTimer5ActiveProcessed = false;
            presenceTimerSetMeAs1ActiveProcessed = false;
            presenceTimerSetMeAs2ActiveProcessed = false;
        }

        
        //SetMeAs x for x seconds timed presence change
        public void SetMeAsForPresenceTimer()
        {
            if (lync.LyncClientSignedIn)
            {
                lync.SetAvailabilityInfo(Settings.PsSetMeAsAvailability2);

                processIcon.BalloonTip(
                    processFriendlyName + " Status Change",
                    "Timer started for " + Settings.PsSetMeAsTime2 + " seconds" + Environment.NewLine + "Availability: " +
                    Settings.PsSetMeAsAvailability2, balloonTipTime);

                System.Timers.Timer SetMeAsForPresenceTimer = new System.Timers.Timer();
                SetMeAsForPresenceTimer.Interval = (Convert.ToInt32(Settings.PsSetMeAsTime2))*1000;
                SetMeAsForPresenceTimer.Elapsed += SetMeAsForPresenceTimer_Elapsed;
                SetMeAsForPresenceTimer.Start();
            }
        }
        //EVENT SetMeAs x for x seconds timed presence change - timer expired
        void SetMeAsForPresenceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ((System.Timers.Timer)sender).Stop();

            if (lync.LyncClientSignedIn)
            {
                lync.SetAvailabilityInfo("Available");

                processIcon.BalloonTip(
                    processFriendlyName + " Status Change",
                    "Timer expired after " + Settings.PsSetMeAsTime2 + " seconds" + Environment.NewLine +
                    "Availability: Available", balloonTipTime);
            }
            Settings.PsSetMeAsEnabled2 = "false";
            ResetPresenceTimer();

            //((System.Timers.Timer)sender).Start();
        }


        //EVENT System idle timer - On idle timeout met
        public void SystemIdleTimer1_OnEnterIdleState(Object sender, IdleEventArgs e)
        {
            if (lync.LyncClientSignedIn)
            {
                lync.SetAvailabilityInfo(Settings.PsSetMeAsAvailability1);

                processIcon.BalloonTip(
                    processFriendlyName + " Status Change",
                    "Your computer has been idle for " + Settings.PsSetMeAsTime1 +
                    " seconds." + Environment.NewLine + "Availability: " + Settings.PsSetMeAsAvailability1,
                    balloonTipTime);
            }
            //presenceTimerSetMeAs1ActiveProcessed = true;
        }

        //EVENT System idle timer - On idle end
        public void SystemIdleTimer1_OnExitIdleState(Object sender, IdleEventArgs e)
        {
            //MessageBox.Show("Welcome back!");
            if (lync.LyncClientSignedIn)
            {
                lync.SetAvailabilityInfo("Available");

                processIcon.BalloonTip(
                    processFriendlyName + " Status Change",
                    "Your computer has come out of an idle state." + Environment.NewLine +
                    "Availability: Available", balloonTipTime);
            }

            ResetPresenceTimer();
        }

        /// <summary>
        /// Check if the current time is between 2 times, checking whether the start-end times cross midnight
        /// </summary>
        /// 
        bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }
        //if ((dateNow.TimeOfDay > Convert.ToDateTime(Settings.PsFromTime1).TimeOfDay) &&
        //    (dateNow.TimeOfDay < Convert.ToDateTime(Settings.PsToTime1).TimeOfDay) && (Settings.PsEnabled1 == "true"))

        private void ActiveTimers()
        {
            DateTime dateNow = DateTime.Now;

            //Check to see if its the next day
            if (currentDate.Date > dateNow.Date)
            {
                //Reset the timer daily to 
                ResetPresenceTimer();
                //Update the the current date
                currentDate = dateNow;

                logging.WriteToLog(processFriendlyName + "timer reset");
            }

            //if (!lync.LyncClientLoaded || !lync.LyncClientSignedIn)
            //{
            //    ResetPresenceTimer();
            //}

            //Check if settings have been saved since run time
            if (settingsSavedTime != SettingsBox.SettingsSavedTime)
            {
                //If they have reset the presence timer
                ResetPresenceTimer();

                //Get the current state of settings saved
                settingsSavedTime = SettingsBox.SettingsSavedTime;
            }

            //Check if Lync is signed out or not running
            if (!lync.LyncClientLoaded || !lync.LyncClientSignedIn)
            {
                ResetPresenceTimer();
            }

            
            //Determine active timers
            //BUG to be fixed:
            //var xxx = Convert.ToDateTime(Settings.PsFromTime1).TimeOfDay;
            //var yyy = Convert.ToDateTime("22:00:00").TimeOfDay;
            //var eee = dateNow.ToShortTimeString();
            //if (yyy < xxx)
            //{
            //    //ddd
            //}

            //if ((dateNow.TimeOfDay > Convert.ToDateTime(Settings.PsFromTime1).TimeOfDay) &&
            //    (dateNow.TimeOfDay < Convert.ToDateTime(Settings.PsToTime1).TimeOfDay) && (Settings.PsEnabled1 == "true"))
            if (TimeBetween(dateNow, Convert.ToDateTime(Settings.PsFromTime1).TimeOfDay, 
                Convert.ToDateTime(Settings.PsToTime1).TimeOfDay) && (Settings.PsEnabled1 == "true"))
            {
                //Lync Presense Timer 1
                presenceTimer1Active = true;
            }
            else
            {
                presenceTimer1Active = false;
                presenceTimer1ActiveProcessed = false;
            }

            if (TimeBetween(dateNow, Convert.ToDateTime(Settings.PsFromTime2).TimeOfDay,
                Convert.ToDateTime(Settings.PsToTime2).TimeOfDay) && Settings.PsEnabled2 == "true")
            {
                //Lync Presense Timer 2
                presenceTimer2Active = true;
            }
            else
            {
                presenceTimer2Active = false;
                presenceTimer2ActiveProcessed = false;
            }

            if (TimeBetween(dateNow, Convert.ToDateTime(Settings.PsFromTime3).TimeOfDay,
                Convert.ToDateTime(Settings.PsToTime3).TimeOfDay) && (Settings.PsEnabled3 == "true"))
            {
                //Lync Presense Timer 3
                presenceTimer3Active = true;
            }
            else
            {
                presenceTimer3Active = false;
                presenceTimer3ActiveProcessed = false;
            }

            if (TimeBetween(dateNow, Convert.ToDateTime(Settings.PsFromTime4).TimeOfDay,
                Convert.ToDateTime(Settings.PsToTime4).TimeOfDay) && (Settings.PsEnabled4 == "true"))
            {
                //Lync Presense Timer 4
                presenceTimer4Active = true;
            }
            else
            {
                presenceTimer4Active = false;
                presenceTimer4ActiveProcessed = false;
            }

            var selectedDaysOfWeek = Settings.PsSelectedDaysOfWeek5.Split(',').ToList();
            if (selectedDaysOfWeek.Contains(dateNow.DayOfWeek.ToString()) && (Settings.PsEnabled5 == "true"))
            {
                //Lync Presense Timer 5 (Days)
                presenceTimer5Active = true;
            }
            else
            {
                presenceTimer5Active = false;
                presenceTimer5ActiveProcessed = false;
            }

            //Determine active Set Me As timers - idle timer
            if (Settings.PsSetMeAsEnabled1 == "true")
            {
                //Lync Set Me As 1
                presenceTimerSetMeAs1Active = true;
            }
            else
            {
                presenceTimerSetMeAs1Active = false;
                presenceTimerSetMeAs1ActiveProcessed = false;
            }

            //Determine active Set Me As timers - fixed period
            if (Settings.PsSetMeAsEnabled2 == "true")
            {
                //Lync Set Me As 2
                presenceTimerSetMeAs2Active = true;
            }
            else
            {
                presenceTimerSetMeAs2Active = false;
                presenceTimerSetMeAs2ActiveProcessed = false;
            }
        }
        
        public void ProcessPresenceSwitcher()
        {
            //Determine active timers
            ActiveTimers();
            
            //IDLE TIMER
            //SetMeAs idle timer - Set me as xx after xx idle time...
            //If idle active and timed not active
            if (presenceTimerSetMeAs1Active && !presenceTimerSetMeAs2Active)
            {
                if (!presenceTimerSetMeAs1ActiveProcessed)
                {
                    if (SystemIdleTimer1.IsRunning == false)
                    {
                        //SystemIdleTimer1.MaxIdleTime = CUInt(numOfSeconds.Value * 1000) ' seconds to miliseconds 
                        SystemIdleTimer1.MaxIdleTime = Convert.ToUInt32(Settings.PsSetMeAsTime1);
                        SystemIdleTimer1.Start();

                        presenceTimerSetMeAs1ActiveProcessed = true;
                    }
                    else
                    {
                        SystemIdleTimer1.Stop();
                        presenceTimerSetMeAs1ActiveProcessed = false;
                    }
                }
            }
            //Stop timer if disabled: SetMeAs idle timer - Set me as xx after xx idle time...
            else //if (!presenceTimerSetMeAs1Active)
            {
                //If the timer is still running after it has been disabled, stop it
                if (SystemIdleTimer1.IsRunning)
                {
                    SystemIdleTimer1.Stop();
                }
            }

            //SET PRESENCE
            //Processed from bottom up, overlapping time ranges will be overided
            //Set presence for SetMeAs timer - Set me as xx for xx 
            if (presenceTimerSetMeAs2Active)
            {
                if (!presenceTimerSetMeAs2ActiveProcessed)
                {
                    //start timer
                    SetMeAsForPresenceTimer();
                    presenceTimerSetMeAs2ActiveProcessed = true;
                }
            }
            //Set presence for timer ranges
            else if (presenceTimer5Active)
            {
                if (!presenceTimer5ActiveProcessed)
                {
                    if (Settings.PsAvailability5 == "Sign Out" && lync.LyncClientSignedIn)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing Out", balloonTipTime);
                        lync.ClientSignOut();

                        presenceTimer5ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability5 == "Sign In" && lync.LyncClientLoaded && !lync.LyncClientSignedIn)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing In", balloonTipTime);
                        lync.ClientSignIn();

                        presenceTimer5ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability5 == "Sign In - Available" && lync.LyncClientLoaded && !lync.LyncClientSignedIn)
                    {
                        lync.ClientSignIn();

                        if (lync.LyncClientSignedIn) //lync.GetClientState() == "SignedIn")
                        {
                            lync.SetAvailabilityInfo("Available", Settings.PsPersonalNote5);

                            processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Signing In..." + Environment.NewLine + "Availability: " + Settings.PsAvailability5 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote5, balloonTipTime);

                            presenceTimer5ActiveProcessed = true;
                        }
                    }
                    else if (lync.LyncClientSignedIn)
                    {
                        if (Settings.PsAvailability5 == "Off Work")
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability5, "off-work", Settings.PsPersonalNote5);
                        }
                        else
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability5, Settings.PsPersonalNote5);
                        }
                        
                        processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Availability: " + Settings.PsAvailability5 + Environment.NewLine + 
                            "Personal Note: " + Settings.PsPersonalNote5, balloonTipTime);
                        
                        presenceTimer5ActiveProcessed = true;
                    }
                }
            }
            else if (presenceTimer4Active)
            {
                if (!presenceTimer4ActiveProcessed)
                {
                    if (Settings.PsAvailability4 == "Sign Out" && lync.LyncClientSignedIn)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing Out", balloonTipTime);
                        lync.ClientSignOut();

                        presenceTimer4ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability4 == "Sign In" && lync.LyncClientLoaded)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing In", balloonTipTime);
                        lync.ClientSignIn();

                        presenceTimer4ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability4 == "Sign In - Available" && lync.LyncClientLoaded)
                    {
                        lync.ClientSignIn();

                        if (lync.LyncClientSignedIn)
                        {

                            lync.SetAvailabilityInfo("Available", Settings.PsPersonalNote4);

                            processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Signing In..." + Environment.NewLine + "Availability: " + Settings.PsAvailability4 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote4, balloonTipTime);
                            
                            presenceTimer4ActiveProcessed = true;
                        }
                    }
                    else if (lync.LyncClientSignedIn)
                    {
                        if (Settings.PsAvailability4 == "Off Work")
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability4, "off-work", Settings.PsPersonalNote4);
                        }
                        else
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability4, Settings.PsPersonalNote4);
                        }

                        processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Availability: " + Settings.PsAvailability4 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote4, balloonTipTime);

                        presenceTimer4ActiveProcessed = true;
                    }
                }
            }
            else if (presenceTimer3Active)
            {
                if (!presenceTimer3ActiveProcessed)
                {
                    if (Settings.PsAvailability3 == "Sign Out" && lync.LyncClientSignedIn)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing Out", balloonTipTime);
                        lync.ClientSignOut();

                        presenceTimer3ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability3 == "Sign In" && lync.LyncClientLoaded)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing In", balloonTipTime);
                        lync.ClientSignIn();

                        presenceTimer3ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability3 == "Sign In - Available" && lync.LyncClientLoaded)
                    {
                        lync.ClientSignIn();

                        if (lync.LyncClientSignedIn)
                        {

                            lync.SetAvailabilityInfo("Available", Settings.PsPersonalNote3);

                            processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Signing In..." + Environment.NewLine + "Availability: " + Settings.PsAvailability3 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote3, balloonTipTime);

                            presenceTimer3ActiveProcessed = true;
                        }
                    }
                    else if (lync.LyncClientSignedIn)
                    {
                        if (Settings.PsAvailability3 == "Off Work")
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability3, "off-work", Settings.PsPersonalNote3);
                        }
                        else
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability3, Settings.PsPersonalNote3);
                        }

                        processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Availability: " + Settings.PsAvailability3 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote3, balloonTipTime);

                        presenceTimer3ActiveProcessed = true;
                    }
                }
            }
            else if (presenceTimer2Active)
            {
                if (!presenceTimer2ActiveProcessed)
                {
                    if (Settings.PsAvailability2 == "Sign Out" && lync.LyncClientSignedIn)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing Out", balloonTipTime);
                        lync.ClientSignOut();

                        presenceTimer2ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability2 == "Sign In" && lync.LyncClientLoaded)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing In", balloonTipTime);
                        lync.ClientSignIn();

                        presenceTimer2ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability2 == "Sign In - Available" && lync.LyncClientLoaded)
                    {
                        lync.ClientSignIn();

                        if (lync.LyncClientSignedIn)
                        {

                            lync.SetAvailabilityInfo("Available", Settings.PsPersonalNote2);

                            processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Signing In..." + Environment.NewLine + "Availability: " + Settings.PsAvailability2 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote2, balloonTipTime);

                            presenceTimer2ActiveProcessed = true;
                        }
                    }
                    else if (lync.LyncClientSignedIn)
                    {
                         if (Settings.PsAvailability2 == "Off Work")
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability2, "off-work", Settings.PsPersonalNote2);
                        }
                        else
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability2, Settings.PsPersonalNote2);
                        }

                        processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Availability: " + Settings.PsAvailability2 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote2, balloonTipTime);

                        presenceTimer2ActiveProcessed = true;
                    }
                }
            }
            else if (presenceTimer1Active)
            {
                if (!presenceTimer1ActiveProcessed)
                {
                    if (Settings.PsAvailability1 == "Sign Out" && lync.LyncClientSignedIn)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing Out", balloonTipTime);
                        lync.ClientSignOut();

                        presenceTimer1ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability1 == "Sign In" && lync.LyncClientLoaded)
                    {
                        processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing In", balloonTipTime);
                        lync.ClientSignIn();

                        presenceTimer1ActiveProcessed = true;
                    }
                    else if (Settings.PsAvailability1 == "Sign In - Available" && lync.LyncClientLoaded)
                    {
                        lync.ClientSignIn();

                        if (lync.LyncClientSignedIn)
                        {

                            lync.SetAvailabilityInfo("Available", Settings.PsPersonalNote1);

                            processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Signing In..." + Environment.NewLine + "Availability: " + Settings.PsAvailability1 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote1, balloonTipTime);

                            presenceTimer1ActiveProcessed = true;
                        }
                    }
                    else if (lync.LyncClientSignedIn)
                    {
                        if (Settings.PsAvailability1 == "Off Work")
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability1, "off-work", Settings.PsPersonalNote1);
                        }
                        else
                        {
                            lync.SetAvailabilityInfo(Settings.PsAvailability1, Settings.PsPersonalNote1);
                        }

                        processIcon.BalloonTip(
                            processFriendlyName + " Status Change", "Availability: " + Settings.PsAvailability1 + Environment.NewLine +
                            "Personal Note: " + Settings.PsPersonalNote1, balloonTipTime);

                        presenceTimer1ActiveProcessed = true;
                    }
                }
            }
        }

        private string RetainedPersonalNoteHandler(string availablity, string personalNote)
        {
            bool RetainCurrentAvailablePersonalNote = true;
            string RetainedPersonalNote = "RetainedPersonalNote";
            if (availablity == "Available" && RetainCurrentAvailablePersonalNote)
            {
                return RetainedPersonalNote;
            }
            else
                return personalNote;
        }
    }
}
