using System;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;

namespace UCExtend
{
    public static class Settings
    {
        public static XDocument appSettings;

        //Load application settings from settings.xml
        public static string settingsFolderBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Application.CompanyName + @"\" + Application.ProductName;
        //static string settingsFilePath = Application.StartupPath + @"\Settings.xml";
        static string settingsFilePath = settingsFolderBase + @"\Settings.xml";
        static string settingsTemplateFilePath = Application.StartupPath + @"\SettingsTemplate.xml";

        public static void LoadSettings()
        {
            try
            {
                appSettings = XDocument.Load(settingsFilePath);
                //MessageBox.Show(appSettings.ToString());
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB(ex.Message, "Error loading " + settingsFilePath);
                System.Environment.Exit(1);
            }
        }

        //Check if settings.xml exisits and create if not
        public static void CreateSettings()
        {
            try
            {
                //UserAccessControl uacCreateSettings = new UserAccessControl();
                
                //Create file path if it doesnt exisit
                Directory.CreateDirectory(settingsFolderBase);

                if (!File.Exists(settingsFilePath))
                {
                    MessageBox.Show("Default application settings will be created at " + settingsFilePath + "." + Environment.NewLine + Environment.NewLine + 
                        "To customise the application settings edit the above mentioned file, " + 
                        "or access the settings menu by right clicking the task tray icon (only available for some settings currently).", "Applications Settings Not Found");
                    File.Copy(settingsTemplateFilePath, settingsFilePath);
                    //settingsTemplateFilePath.CopyTo(settingsFilePath);
                }
                
                ////Request to elevate if required - no longer required as settings file is in User AppData
                //if (!File.Exists(settingsFilePath) && !uacCreateSettings.IsRunAsAdmin())
                //{
                //    if (MessageBox.Show("Creating the default settings.xml file requires admin rights. Would you like to restart the application as an administrator?", "Create Default Settings", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //    {
                //        uacCreateSettings.Elevate();
                //    }
                //}

                //if (!File.Exists(settingsFilePath) && uacCreateSettings.IsRunAsAdmin())
                //{
                //    //MessageBox.Show("Creating Settings.xml from template", "Settings.xml not found");
                //    System.IO.File.Copy(settingsTemplateFilePath, settingsFilePath);
                //}
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB(ex.Message, "Error creating file " + settingsFilePath);
                System.Environment.Exit(1);
            }
        }

        /// <summary>
        /// Settings file variables 
        /// </summary>
        public static string PsEnabled1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet1")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet1")
                    .Element("Enabled")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsEnabled2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet2")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet2")
                    .Element("Enabled")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsEnabled3
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet3")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet3")
                    .Element("Enabled")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsEnabled4
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet4")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet4")
                    .Element("Enabled")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }
        public static string PsFromTime1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet1")
                        .Element("FromTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet1")
                    .Element("FromTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsFromTime2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet2")
                        .Element("FromTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet2")
                    .Element("FromTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsFromTime3
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet3")
                        .Element("FromTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet3")
                    .Element("FromTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsFromTime4
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet4")
                        .Element("FromTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet4")
                    .Element("FromTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }
        public static string PsToTime1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet1")
                        .Element("ToTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet1")
                    .Element("ToTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsToTime2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet2")
                        .Element("ToTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet2")
                    .Element("ToTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsToTime3
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet3")
                        .Element("ToTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet3")
                    .Element("ToTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsToTime4
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet4")
                        .Element("ToTime")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet4")
                    .Element("ToTime")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsAvailability1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet1")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet1")
                    .Element("Availability")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsAvailability2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet2")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet2")
                    .Element("Availability")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsAvailability3
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet3")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet3")
                    .Element("Availability")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsAvailability4
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet4")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet4")
                    .Element("Availability")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }
        public static string PsPersonalNote1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet1")
                        .Element("PersonalNote")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet1")
                    .Element("PersonalNote")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsPersonalNote2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet2")
                        .Element("PersonalNote")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet2")
                    .Element("PersonalNote")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsPersonalNote3
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet3")
                        .Element("PersonalNote")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet3")
                    .Element("PersonalNote")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsPersonalNote4
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet4")
                        .Element("PersonalNote")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet4")
                    .Element("PersonalNote")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        //Week
        public static string PsEnabled5
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet5")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet5")
                    .Element("Enabled")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }
        public static string PsSelectedDaysOfWeek5
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet5")
                        .Element("SelectedDaysOfWeek")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet5")
                    .Element("SelectedDaysOfWeek")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsAvailability5
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet5")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet5")
                    .Element("Availability")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsPersonalNote5
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("TimeSet5")
                        .Element("PersonalNote")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("TimeSet5")
                    .Element("PersonalNote")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        //Set me as
        public static string PsSetMeAsEnabled1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs1")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs1")
                        .Element("Enabled")
                    .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsSetMeAsAvailability1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs1")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("SetMeAs1")
                    .Element("Availability")
                .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsSetMeAsTime1
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs1")
                        .Element("Time")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("SetMeAs1")
                    .Element("Time")
                .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsSetMeAsEnabled2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs2")
                        .Element("Enabled")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("SetMeAs2")
                    .Element("Enabled")
                .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsSetMeAsAvailability2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs2")
                        .Element("Availability")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("SetMeAs2")
                    .Element("Availability")
                .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }

        public static string PsSetMeAsTime2
        {
            get
            {
                return
                    appSettings.Element("Configuration")
                        .Element("SettingsWindow")
                        .Element("Lync")
                        .Element("PresenceSwitcher")
                        .Element("SetMeAs2")
                        .Element("Time")
                        .Value;
            }
            set
            {
                appSettings.Element("Configuration")
                    .Element("SettingsWindow")
                    .Element("Lync")
                    .Element("PresenceSwitcher")
                    .Element("SetMeAs2")
                    .Element("Time")
                .Value = value;

                appSettings.Save(settingsFilePath);
            }
        }


        //private static bool _expired;
        //public static bool Expired
        //{
        //    get
        //    {
        //        // Reads are usually simple
        //        return _expired;
        //    }
        //    set
        //    {
        //        // You can add logic here for race conditions,
        //        // or other measurements
        //        _expired = value;
        //    }
        //}
        //// Perhaps extend this to have Read-Modify-Write static methods
        //// for data integrity during concurrency? Situational.
    }
}
