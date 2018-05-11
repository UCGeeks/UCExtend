using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Lync.Model;
using System.Windows.Threading;

namespace UCExtend
{
    public class LyncClientController
    {
        private LyncClient _lyncClient;
        public bool LyncClientLoaded;
        public bool LyncClientSignedIn;
        private string ucClient = "Lync/Skype4B";

        //Logging
        private Logging logging = Logging.Instance;

        //Timers
        private System.Timers.Timer TimerLyncClientChecks;

        //Custom event handlers:
        //Lync contact information events
        public delegate void LccContactInfoEvents(string availability, string activity);
        public event LccContactInfoEvents LccContactInfoEvent;

        private object _activity;
        public object Activity
        {
            get { return _activity; }
            set { _activity = value; }
        }

        private ContactAvailability _availablity;
        public ContactAvailability Availability
        {
            get { return _availablity; }
            set { _availablity = value; }
        }

        private object _personalNote;
        public object PersonalNote
        {
            get { return _personalNote; }
            set { _personalNote = value; }
        }

        public LyncClientController()
        {
            //initialise and start timer
            TimerLyncClientChecks = new System.Timers.Timer();
            TimerLyncClientChecks.Interval = 1000;
            TimerLyncClientChecks.Elapsed += TimerLyncClientChecks_Elapsed;
            TimerLyncClientChecks.Start();
        }

        /// <summary>
        /// Checks if a named process is running
        /// </summary>
        /// <param name="nameSubstring"></param>
        /// <returns></returns>
        static bool CheckIfProcessIsRunning(string nameSubstring)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(nameSubstring))
                {
                    return true;
                }
            }
            return false;
        }

        //TIMERS
        private void TimerLyncClientChecks_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimerLyncClientChecks.Stop();

            bool lyncProcessRunning = CheckIfProcessIsRunning("lync");

            if (lyncProcessRunning && _lyncClient == null)
            {
                //Clear GetClient cache hack - https://social.msdn.microsoft.com/Forums/lync/en-US/09639fc5-f933-4744-a8b6-86ef9567362a/selfcontact-is-null-after-restarting-lync-client
                try
                {
                    var lyncStateHack = _lyncClient.State;
                }
                catch (Exception) { } //Do nothing
                
                LoadClient();
                
                if (_lyncClient != null)
                {
                    //Get contact info
                    GetContactInfo();
                    //Subscribe to contact information changes
                    SubscribeContactInfoChanges();

                    if (_lyncClient.State == ClientState.SignedIn)
                    {
                        LyncClientSignedIn = true;
                    }
                }
            }
            else if (lyncProcessRunning && _lyncClient != null)
            {
                //Clear GetClient cache hack - https://social.msdn.microsoft.com/Forums/lync/en-US/09639fc5-f933-4744-a8b6-86ef9567362a/selfcontact-is-null-after-restarting-lync-client
                try
                {
                    var lyncStateHack = _lyncClient.State;
                }
                catch (Exception) { } //Do nothing

                LyncClientLoaded = true;

                if (_lyncClient.State == ClientState.SignedIn)
                {
                    LyncClientSignedIn = true;
                }
            }
            else if (!lyncProcessRunning && _lyncClient != null)
            {
                _lyncClient = null;
                LyncClientLoaded = false;
            }
            else if (!lyncProcessRunning && _lyncClient == null)
            {
                LyncClientLoaded = false;
            }

            TimerLyncClientChecks.Start();
            //MessageBox.Show(v.ToString());
        }

        public void LoadClient()
        {
            //Listen for events of changes in the state of the client
            try
            {
                _lyncClient = LyncClient.GetClient();
                
                //Subscribe to events
                if (_lyncClient != null)
                {
                    _lyncClient.StateChanged += _lyncClient_StateChanged;
                    _lyncClient.ClientDisconnected += _lyncClient_ClientDisconnected;
                }
            }
            catch (ClientNotFoundException clientNotFoundException)
            {
                //Console.WriteLine(clientNotFoundException);
                //Shared.MB(clientNotFoundException.ToString(), ucClient + " Client Not Found");
                logging.WriteToLog(ucClient + " Client Not Found: " + clientNotFoundException);
            }
            catch (NotStartedByUserException notStartedByUserException)
            {
                //Console.Out.WriteLine(notStartedByUserException);
                //Shared.MB(notStartedByUserException.ToString(), ucClient + " Client Not Started");
                logging.WriteToLog(ucClient + " Client Not Started: " + notStartedByUserException);
            }
            catch (LyncClientException lyncClientException)
            {
                //Console.Out.WriteLine(lyncClientException);
                //Shared.MB(lyncClientException.ToString(), ucClient + " Client Error");
                logging.WriteToLog(ucClient + " Client Exception: " + lyncClientException);
            }
            catch (SystemException systemException)
            {
                if (IsLyncException(systemException))
                {
                    // Log the exception thrown by the Lync Model API.
                    //Console.WriteLine("Error: " + systemException);
                    //Shared.MB(systemException.ToString(), ucClient + "System Error");
                    logging.WriteToLog(ucClient + " Exception: " + systemException);
                }
                else
                {
                    // Rethrow the SystemException which did not come from the Lync Model API.
                    throw;
                }
            }

        }

        /// <summary>
        /// Lync client state changes event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _lyncClient_StateChanged(object sender, ClientStateChangedEventArgs e)
        {
            //MessageBox.Show("OLD: " + e.OldState + Environment.NewLine + "NEW: " + e.NewState);
            if (e.NewState == ClientState.SignedIn)
            {
                LyncClientSignedIn = true;
            }
            else
            {
                LyncClientSignedIn = false;
            }
            //throw new NotImplementedException();
        }
        
        /// <summary>
        /// Lync client disconnected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _lyncClient_ClientDisconnected(object sender, EventArgs e)
        {
            LyncClientSignedIn = false;
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Get the current state of the client
        /// </summary>
        /// <returns></returns>
        public string GetClientState()
        {
            if (_lyncClient.State == ClientState.Uninitialized)
            {
                return "Uninitialized";
            }
            else if (_lyncClient.State == ClientState.SigningOut)
            {
                return "SigningOut";
            }
            else if (_lyncClient.State == ClientState.SigningIn)
            {
                return "SigningIn";
            }
            else if (_lyncClient.State == ClientState.SignedOut)
            {
                return "SignedOut";
            }
            else if (_lyncClient.State == ClientState.SignedIn)
            {
                return "SignedIn";
            }
            else if (_lyncClient.State == ClientState.ShuttingDown)
            {
                return "ShuttingDown";
            }
            else if (_lyncClient.State == ClientState.Invalid)
            {
                return "Invalid";
            }
            else if (_lyncClient.State == ClientState.Initializing)
            {
                return "Initializing";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Get Lync contact information such as current activity and availability status
        /// </summary>
        public void GetContactInfo()
        {
            if (_lyncClient.State == ClientState.SignedIn)
            {
                Activity = _lyncClient.Self.Contact.GetContactInformation(ContactInformationType.ActivityId);
                Availability = (ContactAvailability)_lyncClient.Self.Contact.GetContactInformation(ContactInformationType.Availability);
                PersonalNote = _lyncClient.Self.Contact.GetContactInformation(ContactInformationType.PersonalNote);
            }
        }

        /// <summary>
        /// Converts availability names to the coresponding Id
        /// </summary>
        /// <param name="availability"></param>
        /// <returns></returns>
        /// https://msdn.microsoft.com/en-us/library/microsoft.lync.controls.contactavailability_di_2_lyncctrlslmref.aspx
        private static int GetAvailabilityId(string availability)
        {
            switch (availability)
            {
                case "Available": return 3500;
                case "Inactive": return 5000;
                case "Away": return 15500;
                case "Be Right Back": return 12500;
                case "Off Work": return 15500;
                case "Busy": return 6500;
                case "Busy - Inactive": return 7500;
                case "Do Not Disturb": return 9500;
                case "Appear Offline": return 18500;
                case "Reset Status": return 0;
                default: return 0;
                //off-work on-the-phone
            }
        }
        

        /// <summary>
        /// Subscribe to contact information changes
        /// </summary>
        public void SubscribeContactInfoChanges()
        {
            //Subscribe to contact information changes
            if (_lyncClient.State == ClientState.SignedIn)
            {
                if (_lyncClient != null)
                    _lyncClient.Self.Contact.ContactInformationChanged += SelfContact_ContactInformationChanged;
                    //_lyncClient.Self.Contact.SettingChanged += SelfContact_ContactInformationChanged;
            }
        }


 

        /// <summary>
        /// Handler for the ContactInformationChanged event of the contact
        /// </summary>
        private void SelfContact_ContactInformationChanged(object sender, ContactInformationChangedEventArgs e)
        {
            //Only update the contact information in the user interface if the client is signed in.
            //Ignore other states including transitions (e.g. signing in or out).
            if (LyncClientSignedIn && _lyncClient.State == ClientState.SignedIn)
            {
                //If activity or availablity info is changed
                if (e.ChangedContactInformation.Contains(ContactInformationType.Activity) || e.ChangedContactInformation.Contains(ContactInformationType.Availability))
                {
                    Activity = _lyncClient.Self.Contact.GetContactInformation(ContactInformationType.ActivityId);
                    Availability = (ContactAvailability)_lyncClient.Self.Contact.GetContactInformation(ContactInformationType.Availability);
                    PersonalNote = _lyncClient.Self.Contact.GetContactInformation(ContactInformationType.PersonalNote);

                    //Raise LccContactInfoEvent update event for availability and activity states
                    if (LccContactInfoEvent != null)
                    {
                        LccContactInfoEvent(Availability.ToString(), Activity.ToString());
                    }
                    
                    ////If busy and the busy state is "on-the-phone"
                    //if (Availability == ContactAvailability.Busy && Activity.ToString().ToLower() == "on-the-phone")
                    //{

                    //}
                    
                    ////If busy
                    //if (Availability == ContactAvailability.Busy)
                    //{

                    //}
                }
  
                ////Get from Lync only the contact information that changed.
                //if (e.ChangedContactInformation.Contains(ContactInformationType.DisplayName))
                //{
                //    //Use the current dispatcher to update the contact's name in the user interface.
                //        //dispatcher.BeginInvoke(new Action(SetName));
                //}
                //if (e.ChangedContactInformation.Contains(ContactInformationType.Availability))
                //{
                //    //Use the current dispatcher to update the contact's availability in the user interface.
                //        //dispatcher.BeginInvoke(new Action(SetAvailability));
                //}
                //if (e.ChangedContactInformation.Contains(ContactInformationType.PersonalNote))
                //{
                //    //Use the current dispatcher to update the contact's personal note in the user interface.
                //        //dispatcher.BeginInvoke(new Action(SetPersonalNote));
                //}
                //if (e.ChangedContactInformation.Contains(ContactInformationType.Photo))
                //{
                //    //Use the current dispatcher to update the contact's photo in the user interface.
                //        //dispatcher.BeginInvoke(new Action(SetContactPhoto));
                //}
            }
        }


        /// <summary>
        /// Sets availability information in Lync (Overload 1)
        /// </summary>
        /// <param name="availability"></param>
        public void SetAvailabilityInfo(string availability)
        {
            if (LyncClientSignedIn && _lyncClient.State == ClientState.SignedIn)
            {
                int availabilityId = GetAvailabilityId(availability);

                Dictionary<PublishableContactInformationType, object> stuffToPublish =
                    new Dictionary<PublishableContactInformationType, object>();
                stuffToPublish.Add(PublishableContactInformationType.Availability, availabilityId);

                try
                {
                    _lyncClient.Self.BeginPublishContactInformation(
                        stuffToPublish,
                        (ar) =>
                        {
                            _lyncClient.Self.EndPublishContactInformation(ar);
                        },
                        null);
                }
                catch (ItemNotFoundException)
                {
                    MessageBox.Show("Error updating presence information", "Error!");
                }
            }
        }

        /// <summary>
        /// Sets availability info in Lync (overload 2)
        /// </summary>
        /// <param name="availability"></param>
        /// <param name="personalNote"></param>
        public void SetAvailabilityInfo(string availability, string personalNote)
        {
            if (LyncClientSignedIn && _lyncClient.State == ClientState.SignedIn)
            {
                int availabilityId = GetAvailabilityId(availability);

                Dictionary<PublishableContactInformationType, object> stuffToPublish =
                    new Dictionary<PublishableContactInformationType, object>();
                stuffToPublish.Add(PublishableContactInformationType.Availability, availabilityId);
                //stuffToPublish.Add(PublishableContactInformationType.Availability, ContactAvailability.Away);
                //stuffToPublish.Add(PublishableContactInformationType.ActivityId, ContactInformationType.ActivityId);
                stuffToPublish.Add(PublishableContactInformationType.PersonalNote, personalNote);

                try
                {
                    _lyncClient.Self.BeginPublishContactInformation(
                        stuffToPublish,
                        (ar) =>
                        {
                            _lyncClient.Self.EndPublishContactInformation(ar);
                        },
                        null);
                }
                catch (ItemNotFoundException)
                {
                    MessageBox.Show("Error updating presence information", "Error!");

                }
            }
        }

        /// <summary>
        /// Sets availability info in Lync (overload 3)
        /// </summary>
        /// <param name="availability"></param>
        /// <param name="personalNote"></param>
        /// /// <param name="activity"></param>
        public void SetAvailabilityInfo(string availability, string activity, string personalNote)
        {
            if (LyncClientSignedIn && _lyncClient.State == ClientState.SignedIn)
            {
                int availabilityId = GetAvailabilityId(availability);

                Dictionary<PublishableContactInformationType, object> stuffToPublish =
                    new Dictionary<PublishableContactInformationType, object>();
                stuffToPublish.Add(PublishableContactInformationType.Availability, availabilityId);
                stuffToPublish.Add(PublishableContactInformationType.ActivityId, activity);
                stuffToPublish.Add(PublishableContactInformationType.PersonalNote, personalNote);

                try
                {
                    _lyncClient.Self.BeginPublishContactInformation(
                        stuffToPublish,
                        (ar) =>
                        {
                            _lyncClient.Self.EndPublishContactInformation(ar);
                        },
                        null);
                }
                catch (ItemNotFoundException)
                {
                    MessageBox.Show("Error updating presence information", "Error!");

                }
            }
        }

        /// <summary>
        /// Reset status to available and clear personal note
        /// </summary>
        public void ResetStatus()
        {
            if (LyncClientSignedIn && _lyncClient.State == ClientState.SignedIn)
            {
                SetAvailabilityInfo("Available", "");
            }
        }


        #region Callbacks From Presence Publicaion example
        /// <summary>
        /// Callback invoked when LyncClient.BeginSignIn is completed
        /// </summary>
        /// <param name="result">The status of the asynchronous operation</param>
        private void SignInCallback(IAsyncResult result)
        {
            try
            {
                _lyncClient.EndSignIn(result);
            }
            catch (LyncClientException e)
            {
                Console.WriteLine(e);
            }
            catch (SystemException systemException)
            {
                if (IsLyncException(systemException))
                {
                    // Log the exception thrown by the Lync Model API.
                    Console.WriteLine("Error: " + systemException);
                }
                else
                {
                    // Rethrow the SystemException which did not come from the Lync Model API.
                    throw;
                }
            }

        }

        /// <summary>
        /// Callback invoked when LyncClient.BeginSignOut is completed
        /// </summary>
        /// <param name="result">The status of the asynchronous operation</param>
        private void SignOutCallback(IAsyncResult result)
        {
            try
            {
                _lyncClient.EndSignOut(result);
            }
            catch (LyncClientException e)
            {
                Console.WriteLine(e);
            }
            catch (SystemException systemException)
            {
                if (IsLyncException(systemException))
                {
                    // Log the exception thrown by the Lync Model API.
                    Console.WriteLine("Error: " + systemException);
                }
                else
                {
                    // Rethrow the SystemException which did not come from the Lync Model API.
                    throw;
                }
            }

        }

        /// <summary>
        /// Callback invoked when Self.BeginPublishContactInformation is completed
        /// </summary>
        /// <param name="result">The status of the asynchronous operation</param>
        private void PublishContactInformationCallback(IAsyncResult result)
        {
            _lyncClient.Self.EndPublishContactInformation(result);
        }
        #endregion


        /// <summary>
        /// Handler for the Click event of the SignInOut Button. Used to sign in or out Lync depending on the current client state.
        /// </summary>
        public void ClientSignInOut()
        {
            //MessageBox.Show("HERE");
            try
            {
                if (_lyncClient.State == ClientState.SignedIn)
                {
                    //Sign out If the current client state is Signed In
                    _lyncClient.BeginSignOut(SignOutCallback, null);
                }
                else if (_lyncClient.State == ClientState.SignedOut)
                {
                    //Sign in If the current client state is Signed Out
                    _lyncClient.BeginSignIn(null, null, null, SignInCallback, null);
                }
            }
            catch (LyncClientException lyncClientException)
            {
                Console.WriteLine(lyncClientException);
            }
            catch (SystemException systemException)
            {
                if (IsLyncException(systemException))
                {
                    // Log the exception thrown by the Lync Model API.
                    Console.WriteLine("Error: " + systemException);
                }
                else
                {
                    // Rethrow the SystemException which did not come from the Lync Model API.
                    throw;
                }
            }

        }

        public void ClientSignIn()
        {
            //MessageBox.Show("HERE");
            try
            {
                if (_lyncClient.State == ClientState.SignedOut)
                {
                    //Sign in If the current client state is Signed Out
                    _lyncClient.BeginSignIn(null, null, null, SignInCallback, null);
                }
            }
            catch (LyncClientException lyncClientException)
            {
                Console.WriteLine(lyncClientException);
            }
            catch (SystemException systemException)
            {
                if (IsLyncException(systemException))
                {
                    // Log the exception thrown by the Lync Model API.
                    Console.WriteLine("Error: " + systemException);
                }
                else
                {
                    // Rethrow the SystemException which did not come from the Lync Model API.
                    throw;
                }
            }

        }

        public void ClientSignOut()
        {
            //MessageBox.Show("HERE");
            try
            {
                if (_lyncClient.State == ClientState.SignedIn)
                {
                    //Sign out If the current client state is Signed In
                    _lyncClient.BeginSignOut(SignOutCallback, null);
                }
            }
            catch (LyncClientException lyncClientException)
            {
                Console.WriteLine(lyncClientException);
            }
            catch (SystemException systemException)
            {
                if (IsLyncException(systemException))
                {
                    // Log the exception thrown by the Lync Model API.
                    Console.WriteLine("Error: " + systemException);
                }
                else
                {
                    // Rethrow the SystemException which did not come from the Lync Model API.
                    throw;
                }
            }

        }

        /// <summary>
        /// Identify if a particular SystemException is one of the exceptions which may be thrown
        /// by the Lync Model API.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool IsLyncException(SystemException ex)
        {
            return
                ex is NotImplementedException ||
                ex is ArgumentException ||
                ex is NullReferenceException ||
                ex is NotSupportedException ||
                ex is ArgumentOutOfRangeException ||
                ex is IndexOutOfRangeException ||
                ex is InvalidOperationException ||
                ex is TypeLoadException ||
                ex is TypeInitializationException ||
                ex is InvalidComObjectException ||
                ex is InvalidCastException;
        }
    }
}















//Contact information changed event
//private void Contact_ContactInformationChanged(object sender, ContactInformationChangedEventArgs e)
//{
//    if (e.ChangedContactInformation.Contains(ContactInformationType.Activity) || e.ChangedContactInformation.Contains(ContactInformationType.Availability))
//    {
//        var activity = _lyncClient.Self.Contact.GetContactInformation(ContactInformationType.ActivityId);
//        ContactAvailability availability = (ContactAvailability)_lyncClient.Self.Contact.GetContactInformation(ContactInformationType.Availability);

//        //MessageBox.Show(availability.ToString());

//        //If busy and the busy state is "on-the-phone"
//        if (availability == ContactAvailability.Busy && activity.ToString().ToLower() == "on-the-phone")
//        {
//            //MessageBox.Show("You're on the phone!");
//        }
//        //If busy
//        if (availability == ContactAvailability.Busy)
//        {
//            //MessageBox.Show("You're busy!");
//        }
//    }
//}




//Change states
//public void State()
//{
//    int currentLCID = System.Globalization.CultureInfo.CurrentUICulture.LCID;
//    IList<CustomAvailabilityState> customStates = lyncClient.Self.GetPublishableCustomAvailabilityStates(currentLCID);

//    foreach (CustomAvailabilityState customState in customStates)
//    {
//        //MessageBox.Show(customState.ToString());
//        //CustomAvailability_List.Items.Add(customState.Id.ToString() + " " + customState.Activity + " " + customState.Availability.ToString());
//    }


//    Dictionary<PublishableContactInformationType, object> stuffToPublish = new Dictionary<PublishableContactInformationType, object>();
//    stuffToPublish.Add(PublishableContactInformationType.Availability, 12000);
//    stuffToPublish.Add(PublishableContactInformationType.PersonalNote, "Lync SDK");
//    //stuffToPublish.Add(PublishableContactInformationType.CustomActivityId, "6000");
//    //stuffToPublish.Add(PublishableContactInformationType.ActivityId, "6000");

//    try
//    {
//        lyncClient.Self.BeginPublishContactInformation(
//            stuffToPublish,
//            (ar) =>
//            {
//                lyncClient.Self.EndPublishContactInformation(ar);
//            },
//            null);
//    }
//    catch (ItemNotFoundException)
//    {
//        //MessageBox.Show(_SelectedCustomAvailabilityId.ToString() + " Item not found");

//    }
//}


//EnvironmentVariableTarget ContactInfo = Dictionary<PublishableContactInformationType, object> ; 


//    $ContactInfo = New-Object 'System.Collections.Generic.Dictionary[Microsoft.Lync.Model.PublishableContactInformationType, object]'

//    $ContactInfo.Add([Microsoft.Lync.Model.PublishableContactInformationType]::Availability, $AvailabilityId)
//    $ContactInfo.Add([Microsoft.Lync.Model.PublishableContactInformationType]::CustomActivityId, $CustomActivityId)
//    $ContactInfo.Add([Microsoft.Lync.Model.PublishableContactInformationType]::PersonalNote, $PersonalNote)

//    $Publish = $Self.BeginPublishContactInformation($ContactInfo, $null, $null)
//    self.EndPublishContactInformation($Publish)




//public void ShowMessageToUser()
//{
//    MessageBox.Show("Your message");
//    //if (Control.InvokeRequired)
//    //{
//    //    Control.Invoke(new MethodInvoker(this.ShowMessageToUser));
//    //}
//    //else
//    //{
//    //    MessageBox.Show("Your message");
//    //}
//}


