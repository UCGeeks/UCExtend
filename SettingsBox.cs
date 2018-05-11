using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Win32;

namespace UCExtend
{
    public partial class SettingsBox : Form
    {

        // Current dispatcher reference for changes in the user interface.
        //private Dispatcher dispatcher;
       
        //SystemIdleTimer SystemIdleTimer1 = new SystemIdleTimer();
        //LyncClientController lync = new LyncClientController();

        //Use to pass shared LyncClientController instance invoked from Program
        public LyncClientController lync;
        
        //Day if week array
        //string[] selectedDaysOfWeek;
        List<string> _selectedDaysOfWeek;

        //Settings saved time - used to track when settingsare changed
        public static DateTime SettingsSavedTime { get; set; }

        public SettingsBox()
        {
            InitializeComponent();

            //Create presence timeswitcher combobox dropdowns
            //Add the availability values to the ComboBox 1
            comboBoxPresenceSwitcher1.Items.Add("Available");
            //comboBoxPresenceSwitcher1.Items.Add("Appear Offline");
            comboBoxPresenceSwitcher1.Items.Add("Away");
            comboBoxPresenceSwitcher1.Items.Add("Inactive");
            comboBoxPresenceSwitcher1.Items.Add("Be Right Back");
            comboBoxPresenceSwitcher1.Items.Add("Off Work");
            comboBoxPresenceSwitcher1.Items.Add("Busy");
            //comboBoxPresenceSwitcher1.Items.Add("Busy - Inactive");
            comboBoxPresenceSwitcher1.Items.Add("Do Not Disturb");
            comboBoxPresenceSwitcher1.Items.Add("Reset Status");
            comboBoxPresenceSwitcher1.Items.Add("Sign In");
            comboBoxPresenceSwitcher1.Items.Add("Sign In - Available");
            comboBoxPresenceSwitcher1.Items.Add("Sign Out");

            //Add the availability values to the ComboBox 2
            comboBoxPresenceSwitcher2.Items.Add("Available");
            //comboBoxPresenceSwitcher2.Items.Add("Appear Offline");
            comboBoxPresenceSwitcher2.Items.Add("Away");
            comboBoxPresenceSwitcher2.Items.Add("Inactive");
            comboBoxPresenceSwitcher2.Items.Add("Be Right Back");
            comboBoxPresenceSwitcher2.Items.Add("Off Work");
            comboBoxPresenceSwitcher2.Items.Add("Busy");
            //comboBoxPresenceSwitcher2.Items.Add("Busy - Inactive");
            comboBoxPresenceSwitcher2.Items.Add("Do Not Disturb");
            comboBoxPresenceSwitcher2.Items.Add("Reset Status");
            comboBoxPresenceSwitcher2.Items.Add("Sign In");
            comboBoxPresenceSwitcher2.Items.Add("Sign In - Available");
            comboBoxPresenceSwitcher2.Items.Add("Sign Out");

            //Add the availability values to the ComboBox 3
            comboBoxPresenceSwitcher3.Items.Add("Available");
            //comboBoxPresenceSwitcher3.Items.Add("Appear Offline");
            comboBoxPresenceSwitcher3.Items.Add("Away");
            comboBoxPresenceSwitcher3.Items.Add("Inactive");
            comboBoxPresenceSwitcher3.Items.Add("Be Right Back");
            comboBoxPresenceSwitcher3.Items.Add("Off Work");
            comboBoxPresenceSwitcher3.Items.Add("Busy");
            //comboBoxPresenceSwitcher3.Items.Add("Busy - Inactive");
            comboBoxPresenceSwitcher3.Items.Add("Do Not Disturb");
            comboBoxPresenceSwitcher3.Items.Add("Reset Status");
            comboBoxPresenceSwitcher3.Items.Add("Sign In");
            comboBoxPresenceSwitcher3.Items.Add("Sign In - Available");
            comboBoxPresenceSwitcher3.Items.Add("Sign Out");

            //Add the availability values to the ComboBox 4
            comboBoxPresenceSwitcher4.Items.Add("Available");
            //comboBoxPresenceSwitcher4.Items.Add("Appear Offline");
            comboBoxPresenceSwitcher4.Items.Add("Away");
            comboBoxPresenceSwitcher4.Items.Add("Inactive");
            comboBoxPresenceSwitcher4.Items.Add("Be Right Back");
            comboBoxPresenceSwitcher4.Items.Add("Off Work");
            comboBoxPresenceSwitcher4.Items.Add("Busy");
            //comboBoxPresenceSwitcher4.Items.Add("Busy - Inactive");
            comboBoxPresenceSwitcher4.Items.Add("Do Not Disturb");
            comboBoxPresenceSwitcher4.Items.Add("Reset Status");
            comboBoxPresenceSwitcher4.Items.Add("Sign In");
            comboBoxPresenceSwitcher4.Items.Add("Sign In - Available");
            comboBoxPresenceSwitcher4.Items.Add("Sign Out");

            //Add the availability values to the ComboBox 5 (week)
            comboBoxPresenceSwitcherDay.Items.Add("Available");
            //comboBoxPresenceSwitcherDay.Items.Add("Appear Offline");
            comboBoxPresenceSwitcherDay.Items.Add("Away");
            comboBoxPresenceSwitcherDay.Items.Add("Inactive");
            comboBoxPresenceSwitcherDay.Items.Add("Be Right Back");
            comboBoxPresenceSwitcherDay.Items.Add("Off Work");
            comboBoxPresenceSwitcherDay.Items.Add("Busy");
            //comboBoxPresenceSwitcherDay.Items.Add("Busy - Inactive");
            comboBoxPresenceSwitcherDay.Items.Add("Do Not Disturb");
            comboBoxPresenceSwitcherDay.Items.Add("Reset Status");
            comboBoxPresenceSwitcherDay.Items.Add("Sign In");
            comboBoxPresenceSwitcherDay.Items.Add("Sign In - Available");
            comboBoxPresenceSwitcherDay.Items.Add("Sign Out");

            //Add the availability values to SetMeAs 1
            comboBoxSettingsLyncSetMeAs1.Items.Add("Available");
            //comboBoxPresenceSwitcherDay.Items.Add("Appear Offline");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Away");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Inactive");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Be Right Back");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Off Work");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Busy");
            //comboBoxSettingsLyncSetMeAs1.Items.Add("Busy - Inactive");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Do Not Disturb");
            comboBoxSettingsLyncSetMeAs1.Items.Add("Reset Status");
            //comboBoxSettingsLyncSetMeAs1.Items.Add("Sign In");
            //comboBoxSettingsLyncSetMeAs1.Items.Add("Sign In - Available");
            //comboBoxSettingsLyncSetMeAs1.Items.Add("Sign Out");

            //Add the availability values to SetMeAs 2
            comboBoxSettingsLyncSetMeAs2.Items.Add("Available");
            //comboBoxSettingsLyncSetMeAs2.Items.Add("Appear Offline");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Away");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Inactive");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Be Right Back");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Off Work");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Busy");
            //comboBoxSettingsLyncSetMeAs2.Items.Add("Busy - Inactive");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Do Not Disturb");
            comboBoxSettingsLyncSetMeAs2.Items.Add("Reset Status");
            //comboBoxSettingsLyncSetMeAs2.Items.Add("Sign In");
            //comboBoxSettingsLyncSetMeAs2.Items.Add("Sign In - Available");
            //comboBoxSettingsLyncSetMeAs2.Items.Add("Sign Out");

        }

      

        //When LccContactInfoEvent event is raised update the form
        void lync_LccContactInfoEvent(string availability, string activity)
        {
            //MessageBox.Show(someinfo);
            //if(SettingsBox.ActiveForm.)
            labelCurrentStatus.Invoke((MethodInvoker)delegate()
            {
                labelCurrentStatus.Text = "Lync Status: " + lync.Availability + " - " + lync.Activity.ToString();
            })
            ;
            //throw new NotImplementedException();
        }

        private void buttonSignOut_Click(object sender, EventArgs e)
        {
           // LyncClientController signOut = new LyncClientController();
            lync.ClientSignInOut();
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SettingsSavedTime = DateTime.Now;

            //Presense switcher check boxes
            if (checkBoxPresenceSwitcher1.Checked == true) {Settings.PsEnabled1 = "true";}
            else{Settings.PsEnabled1 = "false";}
            if (checkBoxPresenceSwitcher2.Checked == true) { Settings.PsEnabled2 = "true"; }
            else { Settings.PsEnabled2 = "false"; }
            if (checkBoxPresenceSwitcher3.Checked == true) { Settings.PsEnabled3 = "true"; }
            else { Settings.PsEnabled3 = "false"; }
            if (checkBoxPresenceSwitcher4.Checked == true) { Settings.PsEnabled4 = "true"; }
            else { Settings.PsEnabled4 = "false"; }
            if (checkBoxPresenceSwitcherDay.Checked == true) { Settings.PsEnabled5 = "true"; }
            else { Settings.PsEnabled5 = "false"; }

            //From
            Settings.PsFromTime1 = dateTimePickerFromPresenceSwitcher1.Value.TimeOfDay.ToString();
            Settings.PsFromTime2 = dateTimePickerFromPresenceSwitcher2.Value.TimeOfDay.ToString();
            Settings.PsFromTime3 = dateTimePickerFromPresenceSwitcher3.Value.TimeOfDay.ToString();
            Settings.PsFromTime4 = dateTimePickerFromPresenceSwitcher4.Value.TimeOfDay.ToString();

            //To
            Settings.PsToTime1 = dateTimePickerToPresenceSwitcher1.Value.TimeOfDay.ToString();
            Settings.PsToTime2 = dateTimePickerToPresenceSwitcher2.Value.TimeOfDay.ToString();
            Settings.PsToTime3 = dateTimePickerToPresenceSwitcher3.Value.TimeOfDay.ToString();
            Settings.PsToTime4 = dateTimePickerToPresenceSwitcher4.Value.TimeOfDay.ToString();

            //Availability
            Settings.PsAvailability1 = comboBoxPresenceSwitcher1.Text;
            Settings.PsAvailability2 = comboBoxPresenceSwitcher2.Text;
            Settings.PsAvailability3 = comboBoxPresenceSwitcher3.Text;
            Settings.PsAvailability4 = comboBoxPresenceSwitcher4.Text;
            Settings.PsAvailability5 = comboBoxPresenceSwitcherDay.Text;

            //PersonalNote
            Settings.PsPersonalNote1 = textBoxPresenceSwitcher1.Text;
            Settings.PsPersonalNote2 = textBoxPresenceSwitcher2.Text;
            Settings.PsPersonalNote3 = textBoxPresenceSwitcher3.Text;
            Settings.PsPersonalNote4 = textBoxPresenceSwitcher4.Text;
            Settings.PsPersonalNote5 = textBoxPresenceSwitcherDay.Text;

            //Days of Week
            _selectedDaysOfWeek = new List<string>();

            if (checkBoxSettingsMonday.Checked)
            {
                _selectedDaysOfWeek.Add("Monday");
            }
            if (checkBoxSettingsTuesday.Checked)
            {
                _selectedDaysOfWeek.Add("Tuesday");
            }
            if (checkBoxSettingsWednesday.Checked)
            {
                _selectedDaysOfWeek.Add("Wednesday");
            }
            if (checkBoxSettingsThursday.Checked)
            {
                _selectedDaysOfWeek.Add("Thursday");
            }
            if (checkBoxSettingsFriday.Checked)
            {
                _selectedDaysOfWeek.Add("Friday");
            }
            if (checkBoxSettingsSaturday.Checked)
            {
                _selectedDaysOfWeek.Add("Saturday");
            }
            if (checkBoxSettingsSunday.Checked)
            {
                _selectedDaysOfWeek.Add("Sunday");
            }
            Settings.PsSelectedDaysOfWeek5 = (string.Join(",",_selectedDaysOfWeek));


            //Set Me As
            if (checkBoxSettingsLyncSetMeAs1.Checked == true) { Settings.PsSetMeAsEnabled1 = "true"; }
            else { Settings.PsSetMeAsEnabled1 = "false"; }
            if (checkBoxSettingsLyncSetMeAs2.Checked == true) { Settings.PsSetMeAsEnabled2 = "true"; }
            else { Settings.PsSetMeAsEnabled2 = "false"; }

            Settings.PsSetMeAsAvailability1 = comboBoxSettingsLyncSetMeAs1.Text;
            Settings.PsSetMeAsTime1 = numericUpDownSettingsLyncSetMeAs1.Value.ToString();

            Settings.PsSetMeAsAvailability2 = comboBoxSettingsLyncSetMeAs2.Text;
            Settings.PsSetMeAsTime2 = numericUpDownSettingsLyncSetMeAs2.Value.ToString();


            //EnableSkypeUI - One off change when user saves settings (not saved in settings or enforced after save)
            ModifyRegistry enableSkypeUI = new ModifyRegistry();
            enableSkypeUI.RegistryView = RegistryView.Registry32; //specifies to use the x64 registry node for x32 application. If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
            enableSkypeUI.RegistryHive = RegistryHive.CurrentUser;
            enableSkypeUI.ShowError = true;
            string regKeyName = "EnableSkypeUI";
            enableSkypeUI.SubKey = "SOFTWARE\\Microsoft\\Office\\Lync";

            if (radioButtonSkypeUIFalse.Checked)
            {
                enableSkypeUI.Write(regKeyName, new byte[] {00, 00, 00, 00}, RegistryValueKind.Binary);
            }
            else if (radioButtonSkypeUITrue.Checked)
            {
                enableSkypeUI.Write(regKeyName, new byte[] {01, 00, 00, 00}, RegistryValueKind.Binary);
            }

            labelSettingsSavedTime.Text = "Saved Time: " + SettingsSavedTime.ToString("dd-MM-yyyy HH:mm:ss");
        }

        private void SettingsBox_Load(object sender, EventArgs e)
        {
            //subscribe to LccContactInfoEvent event handler for form updates
            lync.LccContactInfoEvent += lync_LccContactInfoEvent;

            //Update form with current Lync status
            //lync.GetContactInfo();
            labelCurrentStatus.Text = "Lync Status: " + lync.Availability + " - " + lync.Activity;

            //Presense switcher check boxes
            if (Settings.PsEnabled1 == "true") { checkBoxPresenceSwitcher1.Checked = true; }
            else { checkBoxPresenceSwitcher1.Checked = false; }
            if (Settings.PsEnabled2 == "true") { checkBoxPresenceSwitcher2.Checked = true; }
            else { checkBoxPresenceSwitcher2.Checked = false; }
            if (Settings.PsEnabled3 == "true") { checkBoxPresenceSwitcher3.Checked = true; }
            else { checkBoxPresenceSwitcher3.Checked = false; }
            if (Settings.PsEnabled4 == "true") { checkBoxPresenceSwitcher4.Checked = true; }
            else { checkBoxPresenceSwitcher4.Checked = false; }
            if (Settings.PsEnabled5 == "true") { checkBoxPresenceSwitcherDay.Checked = true; }
            else { checkBoxPresenceSwitcherDay.Checked = false; }

            //From
            dateTimePickerFromPresenceSwitcher1.Value = Convert.ToDateTime(Settings.PsFromTime1);
            dateTimePickerFromPresenceSwitcher2.Value = Convert.ToDateTime(Settings.PsFromTime2);
            dateTimePickerFromPresenceSwitcher3.Value = Convert.ToDateTime(Settings.PsFromTime3);
            dateTimePickerFromPresenceSwitcher4.Value = Convert.ToDateTime(Settings.PsFromTime4);

            //To
            dateTimePickerToPresenceSwitcher1.Value = Convert.ToDateTime(Settings.PsToTime1);
            dateTimePickerToPresenceSwitcher2.Value = Convert.ToDateTime(Settings.PsToTime2);
            dateTimePickerToPresenceSwitcher3.Value = Convert.ToDateTime(Settings.PsToTime3);
            dateTimePickerToPresenceSwitcher4.Value = Convert.ToDateTime(Settings.PsToTime4);

            //Availability
            comboBoxPresenceSwitcher1.Text = Settings.PsAvailability1;
            comboBoxPresenceSwitcher2.Text = Settings.PsAvailability2;
            comboBoxPresenceSwitcher3.Text = Settings.PsAvailability3;
            comboBoxPresenceSwitcher4.Text = Settings.PsAvailability4;
            comboBoxPresenceSwitcherDay.Text = Settings.PsAvailability5;

            //PersonalNote
            textBoxPresenceSwitcher1.Text = Settings.PsPersonalNote1;
            textBoxPresenceSwitcher2.Text = Settings.PsPersonalNote2;
            textBoxPresenceSwitcher3.Text = Settings.PsPersonalNote3;
            textBoxPresenceSwitcher4.Text = Settings.PsPersonalNote4;
            textBoxPresenceSwitcherDay.Text = Settings.PsPersonalNote5;

            //Days of Week
            _selectedDaysOfWeek = new List<string>();
            _selectedDaysOfWeek = Settings.PsSelectedDaysOfWeek5.Split(',').ToList();

            if (_selectedDaysOfWeek.Contains("Monday"))
            {
                checkBoxSettingsMonday.Checked = true;
            }
            if (_selectedDaysOfWeek.Contains("Tuesday"))
            {
                checkBoxSettingsTuesday.Checked = true;
            }
            if (_selectedDaysOfWeek.Contains("Wednesday"))
            {
                checkBoxSettingsWednesday.Checked = true;
            }
            if (_selectedDaysOfWeek.Contains("Thursday"))
            {
                checkBoxSettingsThursday.Checked = true;
            }
            if (_selectedDaysOfWeek.Contains("Friday"))
            {
                checkBoxSettingsFriday.Checked = true;
            }
            if (_selectedDaysOfWeek.Contains("Saturday"))
            {
                checkBoxSettingsSaturday.Checked = true;
            }
            if (_selectedDaysOfWeek.Contains("Sunday"))
            {
                checkBoxSettingsSunday.Checked = true;
            }

            //Set Me As
            if (Settings.PsSetMeAsEnabled1 == "true") { checkBoxSettingsLyncSetMeAs1.Checked = true; }
            else { checkBoxSettingsLyncSetMeAs1.Checked = false; }
            if (Settings.PsSetMeAsEnabled2 == "true") { checkBoxSettingsLyncSetMeAs2.Checked = true; }
            else { checkBoxSettingsLyncSetMeAs2.Checked = false; }

            comboBoxSettingsLyncSetMeAs1.Text = Settings.PsSetMeAsAvailability1;
            numericUpDownSettingsLyncSetMeAs1.Value = Convert.ToInt32(Settings.PsSetMeAsTime1);

            comboBoxSettingsLyncSetMeAs2.Text = Settings.PsSetMeAsAvailability2;
            numericUpDownSettingsLyncSetMeAs2.Value = Convert.ToInt32(Settings.PsSetMeAsTime2);

        }

        private void SettingsBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //subscribe = null;
            lync.LccContactInfoEvent -= lync_LccContactInfoEvent;
        }

        ///// <summary>
        ///// Day of week check box change events
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void checkBoxSettingsMonday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsMonday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Monday");
        //    }
        //    else if (!checkBoxSettingsMonday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Monday");
        //    }
        //}

        //private void checkBoxSettingsTuesday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsTuesday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Tuesday");
        //    }
        //    else if (!checkBoxSettingsTuesday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Tuesday");
        //    }
        //}

        //private void checkBoxSettingsWednesday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsWednesday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Wednesday");
        //    }
        //    else if (!checkBoxSettingsWednesday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Wednesday");
        //    }
        //}

        //private void checkBoxSettingsThursday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsThursday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Thursday");
        //    }
        //    else if (!checkBoxSettingsThursday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Thursday");
        //    }
        //}

        //private void checkBoxSettingsFriday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsFriday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Friday");
        //    }
        //    else if (!checkBoxSettingsFriday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Friday");
        //    }
        //}

        //private void checkBoxSettingsSaturday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsSaturday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Saturday");
        //    }
        //    else if (!checkBoxSettingsSaturday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Saturday");
        //    }
        //}
        //private void checkBoxSettingsSunday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxSettingsSunday.Checked)
        //    {
        //        _selectedDaysOfWeek.Add("Sunday");
        //    }
        //    else if (!checkBoxSettingsSunday.Checked)
        //    {
        //        _selectedDaysOfWeek.Remove("Sunday");
        //    }
        //}




        ////SYSTEM IDLE TIMER
        //private void SystemIdleTimer1_OnEnterIdleState(Object sender, IdleEventArgs e)
        //{
        //    MessageBox.Show("Entered idle state");
        //}

        //private void SystemIdleTimer1_OnExitIdleState(Object sender, IdleEventArgs e)
        //{
        //    MessageBox.Show("Welcome back!");
        //}
        
        //private void buttonStart_Click(object sender, EventArgs e)
        //{
        //    if (SystemIdleTimer1.IsRunning == false)
        //    {
        //        //SystemIdleTimer1.MaxIdleTime = CUInt(numOfSeconds.Value * 1000) ' seconds to miliseconds 
        //        SystemIdleTimer1.MaxIdleTime = Convert.ToUInt32(10);
        //        SystemIdleTimer1.Start();
        //    }
        //    else
        //    {
        //        SystemIdleTimer1.Stop();
        //    }
        //}
    }
}





//BindingSource bs1 = new BindingSource();
//BindingSource bs2 = new BindingSource();
//BindingSource bs3 = new BindingSource();
//BindingSource bs4 = new BindingSource();
//BindingSource bsReset = new BindingSource();

//Hashtable ht = new Hashtable();
//ht.Add("Available", "3000");
//ht.Add("Appear Offline", "18000");
//ht.Add("Away", "15000");
//ht.Add("Busy", "6000");
//ht.Add("Do Not Disturb", "9000");
//ht.Add("Be Right Back", "12000");
//ht.Add("Off Work", "15500");

//bs1.DataSource = ht;
//bs2.DataSource = ht;
//bs3.DataSource = ht;
//bs4.DataSource = ht;
//bsReset.DataSource = ht;

//this.comboBoxPresenceSwitcher1.DataSource = bs1;
//this.comboBoxPresenceSwitcher2.DataSource = bs2;
//this.comboBoxPresenceSwitcher3.DataSource = bs3;
//this.comboBoxPresenceSwitcher4.DataSource = bs4;

//this.comboBoxPresenceSwitcher1.DisplayMember = "Key";
//this.comboBoxPresenceSwitcher2.DisplayMember = "Key";
//this.comboBoxPresenceSwitcher3.DisplayMember = "Key";
//this.comboBoxPresenceSwitcher4.DisplayMember = "Key";

//this.comboBoxPresenceSwitcher1.ValueMember = "Value";
//this.comboBoxPresenceSwitcher2.ValueMember = "Value";
//this.comboBoxPresenceSwitcher3.ValueMember = "Value";
//this.comboBoxPresenceSwitcher4.ValueMember = "Value";