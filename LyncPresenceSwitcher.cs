using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Lync.Model;

namespace UCExtend
{
    class LyncPresenceSwitcher
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

        public LyncPresenceSwitcher()
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

            logging.WriteToLog(processFriendlyName + " started at " + currentDate);
        }

//TIMERS  ################################################################################################
        ////TimerLyncClient - timer elapsed event
        void TimerLyncClientChecks_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Stop timer and process
            timerLyncClientChecks.Stop();

            //If the LyncClient is loaded, start the app processing timer
            if (lync.LyncClientLoaded && !timerPresenceSwitcher.Enabled)
            {
                //Start app processing timer
                timerPresenceSwitcher.Start();
            }
            else if (!lync.LyncClientLoaded && timerPresenceSwitcher.Enabled)
            {
                timerPresenceSwitcher.Stop();
            }

            //Start timer again
            timerLyncClientChecks.Start();
        }

        //timer elapsed event
        void TimerPresenceSwitcher_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Stop timer and process
            timerPresenceSwitcher.Stop();

            lock (timerPresenceSwitcher)
            {
                ProcessPresenceSwitcher();
            }

            //Start timer again
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
                    if (!presenceTimer5ActiveProcessed)
                    {
                        presenceTimer5ActiveProcessed = UpdatePresenceInfo(Settings.PsAvailability5, Settings.PsPersonalNote5);
                    }
                }
            }
            else if (presenceTimer4Active)
            {
                if (!presenceTimer4ActiveProcessed)
                {
                    if (!presenceTimer4ActiveProcessed)
                    {
                        presenceTimer4ActiveProcessed = UpdatePresenceInfo(Settings.PsAvailability4, Settings.PsPersonalNote4);
                    }
                }
            }
            else if (presenceTimer3Active)
            {
                if (!presenceTimer3ActiveProcessed)
                {
                    if (!presenceTimer3ActiveProcessed)
                    {
                        presenceTimer3ActiveProcessed = UpdatePresenceInfo(Settings.PsAvailability3, Settings.PsPersonalNote3);
                    }
                }
            }
            else if (presenceTimer2Active)
            {
                if (!presenceTimer2ActiveProcessed)
                {
                    if (!presenceTimer2ActiveProcessed)
                    {
                        presenceTimer2ActiveProcessed = UpdatePresenceInfo(Settings.PsAvailability2, Settings.PsPersonalNote2);
                    }
                }

            }
            else if (presenceTimer1Active)
            {
                if (!presenceTimer1ActiveProcessed)
                {
                    presenceTimer1ActiveProcessed = UpdatePresenceInfo(Settings.PsAvailability1, Settings.PsPersonalNote1);
                }
            }
        }
        

        private bool UpdatePresenceInfo(string settingAvailablity, string settingPersonalNote)
        {
            //Get Current Presenece Info
            //TODO

            //Get calculated personal note
            var personalNote = PersonalNoteHelper(settingPersonalNote);


            //Update Presence Info
            if (settingAvailablity == "Sign Out" && lync.LyncClientSignedIn)
            {
                processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing Out", balloonTipTime);
                lync.ClientSignOut();

                return true;
            }
            else if (settingAvailablity == "Sign In" && lync.LyncClientLoaded)
            {
                processIcon.BalloonTip(processFriendlyName + " Status Change", "Signing In", balloonTipTime);
                lync.ClientSignIn();

                return true;
            }
            else if (settingAvailablity == "Sign In - Available" && lync.LyncClientLoaded)
            {
                lync.ClientSignIn();

                if (lync.LyncClientSignedIn)
                {

                    //lync.SetAvailabilityInfo("Available", settingPersonalNote);
                    SetAvailabilityInfoHelper("Available", personalNote);

                    processIcon.BalloonTip(
                    processFriendlyName + " Status Change", "Signing In..." + Environment.NewLine + "Availability: " + settingAvailablity + Environment.NewLine +
                    "Personal Note: " + personalNote, balloonTipTime);

                    return true;
                }
            }
            else if (lync.LyncClientSignedIn)
            {
                if (settingAvailablity == "Off Work")
                {
                    //lync.SetAvailabilityInfo(settingAvailablity, "off-work", settingPersonalNote);
                    SetAvailabilityInfoHelper(settingAvailablity, "off-work", personalNote);
                }
                else
                {
                    SetAvailabilityInfoHelper(settingAvailablity, personalNote);
                }

                //processIcon.BalloonTip(processFriendlyName + " Status Change", 
                //    "Availability: " + settingAvailablity + Environment.NewLine +
                //    "Personal Note: " + settingPersonalNote, balloonTipTime);

                BalloonTipStatusChangeHelper(settingAvailablity, personalNote);

                return true;
            }

            return false;
        }

        private string BalloonTipStatusChangeHelper(string availability, string personalNote)
        {            
            if (!string.IsNullOrWhiteSpace(availability) && personalNote != null)
            {
                processIcon.BalloonTip(processFriendlyName + " Status Change",
                   "Availability: " + availability + Environment.NewLine +
                   "Personal Note: " + personalNote, balloonTipTime);
            }
            else if (!string.IsNullOrWhiteSpace(availability) && personalNote == null)
            {
                processIcon.BalloonTip(processFriendlyName + " Status Change",
                   "Availability: " + availability + Environment.NewLine, 
                   balloonTipTime);
            }

            return null;
        }

        private void SetAvailabilityInfoHelper(string availability, string personalNote)
        {
            if (!string.IsNullOrWhiteSpace(availability) && personalNote != null)
            {
                lync.SetAvailabilityInfo(availability, personalNote);
            }
            else if (!string.IsNullOrWhiteSpace(availability) && personalNote == null)
            {
                lync.SetAvailabilityInfo(availability);
            }            
        }

        private void SetAvailabilityInfoHelper(string availability, string activity, string personalNote)
        {
            if (!string.IsNullOrWhiteSpace(availability) && personalNote != null)
            {
                lync.SetAvailabilityInfo(availability, activity, personalNote);
            }
            else if (!string.IsNullOrWhiteSpace(availability) && personalNote == null)
            {
                lync.SetAvailabilityInfo(availability, activity, "");
            }
        }

        //if personal note not equal to any listed save the note and apply it when outside of any other timer period. 
        //maybe add a default reset presence time period to enforce this when no other periods are active. 
        //this way will revert to correct state. this can have the option to set a note or used last used not listed
        //on start get note regardless if listed
        //before each presence change get and update the note if not in list
        //each note can have option to enter blank or text note, or tick box to maintain current
        //each time period also can have tick box to revert prior custom note
        //private string PersonalNoteHelper(string availablity, string personalNote)
        private string PersonalNoteHelper(string personalNote)
        {
            //bool RetainCurrentAvailablePersonalNote = true;
            //RetainedPersonalNote = "RetainedPersonalNote";
            //if (availablity == "Available" && RetainCurrentAvailablePersonalNote)
            //{
            //    return RetainedPersonalNote;
            //}
            //else
            //    return personalNote;

            //labelDontChangePersonalNote1
            //checkBoxDontChangePersonalNote1
            //labelRestorePersonalNote1
            //checkBoxRestorePersonalNote1

            //var test = lync.PersonalNote;
            var currentPersonalNote = lync.PersonalNote.ToString();
            if (currentPersonalNote != Settings.PsPersonalNote1 && currentPersonalNote != Settings.PsPersonalNote2 && currentPersonalNote != Settings.PsPersonalNote3
                && currentPersonalNote != Settings.PsPersonalNote4 && currentPersonalNote != Settings.PsPersonalNote5)
            {
                //Current personal note is not one set by UCExtend, assuming custom set by user and saving for reverting purposes
                RetainedPersonalNote = currentPersonalNote;
            }
            else
            {
                RetainedPersonalNote = null;
            }

            var checkBoxDontChangePersonalNote1 = false;
            var checkBoxRestorePersonalNote1 = true;

            if (checkBoxDontChangePersonalNote1) 
            {
                //Dont change personal note
                return null;
            }
            else if (checkBoxRestorePersonalNote1 && RetainedPersonalNote != null)
            {
                return RetainedPersonalNote;
            }
            else if (string.IsNullOrWhiteSpace(personalNote))
            {
                //Set blank personal note
                return "";
            }
            else
            {
                //Set PS set personal note
                return personalNote;
            }

        }



        private string _retainedPersonalNote;
        public string RetainedPersonalNote
        {
            get { return _retainedPersonalNote; }
            set { _retainedPersonalNote = value; }
        }


    }
}
