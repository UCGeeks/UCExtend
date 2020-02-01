using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Win32;
using SingleInstanceClassLibrary;
using UCExtend.VideoTraining;

namespace UCExtend
{
    public static class Program
	{
        static SettingsBox settingsBox;
        private static UserAccessControl userAccessControl  = new UserAccessControl();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            //Load settings
            LoadSettings();
            
            //Check if settings have components that require admin rights
            RunAsAdminCheck();
            
            //SingleInstance ensures only 1 instance of the app runs at one time. If another instance is started
            //it will be closed. If the 2nd instance included arguments these will be passed to 
            //the singleInstance_ArgumentsReceived event for originally running process
            Guid guid = new Guid("{6EAE2E61-E7EE-42bf-8EBE-BAB890C5410F}");
            using (SingleInstance singleInstance = new SingleInstance(guid))
            {
                if (singleInstance.IsFirstInstance)
                {
                    singleInstance.ArgumentsReceived += singleInstance_ArgumentsReceived;
                    singleInstance.ListenForArgumentsFromSuccessiveInstances();

                    //Load app components
                    LoadApp();
                }
                else
                    singleInstance.PassArgumentsToFirstInstance(Environment.GetCommandLineArgs());
            }
        }


        /// <summary>
        /// Process arguments past with app execution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void singleInstance_ArgumentsReceived(object sender, ArgumentsReceivedEventArgs e)
        {
            foreach (String arg in e.Args)
            {
                //if arguments include OpenSettings open SettingsBox
                if (arg == "OpenSettings")
                {
                    settingsBox.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Check if settings have components that require admin rights, give option to restart as admin
        /// </summary>
        private static void RunAsAdminCheck()
        {
            //If not already running as admin
            if (!userAccessControl.IsRunAsAdmin())
            {
                #region Lync Custom Menus
                //If Lync custom menus are defined check if they need updating
                bool regRequiresUpdate = false;

                if (Settings.appSettings.Descendants("CustomMenuItems").Elements("MenuItem").Any())
                {

                    ModifyRegistry readKey = new ModifyRegistry();
                    readKey.RegistryView = RegistryView.Registry32;
                    readKey.RegistryHive = RegistryHive.LocalMachine;
                    readKey.ShowError = true;

                    //Read MenuItems from XML
                    foreach (var CustomMenuItem in Settings.appSettings.Descendants("CustomMenuItems").Descendants("MenuItem"))
                    {
                        readKey.SubKey = "SOFTWARE\\Microsoft\\Office\\15.0\\Lync\\SessionManager\\Apps\\" +
                                           CustomMenuItem.Element("GUID").Value;

                        foreach (var CustomMenuItemValue in CustomMenuItem.Elements())
                        {
                            if (CustomMenuItemValue.Name.ToString() != "GUID")
                            {
                                string regKeyName = CustomMenuItemValue.Name.ToString();
                                string regSettingsValue = CustomMenuItemValue.Value;
                                string regValue = readKey.Read(regKeyName);

                                //MessageBox.Show("Sval: " + regSettingsValue + Environment.NewLine + "regVal: " + regValue);
                                
                                if (regSettingsValue != regValue)
                                {
                                    regRequiresUpdate = true;
                                }
                            }
                        }
                    }
                }

                //If Lync custom menus need updating prompt to restart as admin
                if (regRequiresUpdate)
                {
                    if (MessageBox.Show(
                        "Application settings have enabled Lync custom menus, however you do not have the required permissions. " +
                        "To enable these settings run the application as admin, or remove this configuration from the applications settings." +
                        Environment.NewLine + Environment.NewLine + "Would you like to restart as admin?",
                        "Error creating Lync custom menus", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        userAccessControl.Elevate();
                    }
                }
                #endregion Lync Custom Menus
            }
        }

        /// <summary>
        /// Load Settings
        /// </summary>
        private static void LoadSettings()
        {
            //Create settings file if it doesnt exist
            Settings.CreateSettings();

            //Load settings
            Settings.LoadSettings();
        }

        /// <summary>
        /// Load application
        /// </summary>
        private static void LoadApp()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Run Lync registry modifcations
            Lync.CreateLyncMenu();

            //Run registry Watcher
            Lync.RegistryWatcher();

            //Get shared instance of LyncClientController - pass to other instances that require it
            LyncClientController lyncClientController = new LyncClientController();

            //Run Lync Presence Switcher
            LyncPresenceSwitcher lyncPresenceSwitcher = new LyncPresenceSwitcher();
            lyncPresenceSwitcher.lync = lyncClientController;

            settingsBox = new SettingsBox();
            settingsBox.lync = lyncClientController;
            //settingsBox.ShowDialog();

 #if DEBUG
//new VideoPlayer().ShowDialog();
#endif

            //Video offer service
            var videoOfferService = new VideoOfferService();

            // Show the system tray icon.					
            //using (ProcessIcon pi = new ProcessIcon())
            using (ProcessIcon pi = new ProcessIcon())
            {
                //Use to pass instance of ProcessIcon to LyncPresenceSwitcher
                lyncPresenceSwitcher.processIcon = pi;

                //Pass Lync instance
                pi.lync = lyncClientController;

                pi.Display();

                // Make sure the application runs!
                Application.Run();                
            }

            
            
        }
	}    
}



////string regKeyName = Settings.appSettings.Descendants("CustomMenuItems").Descendants("MenuItem").

////Read registry
//string GUID = readKey.Read("GUID");
//string ExtensibleMenu = readKey.Read("ExtensibleMenu");
//string Name = readKey.Read("Name");
//string Path = readKey.Read("Path");
//string ApplicationType = readKey.Read("ApplicationType");
//string ApplicationInstallPath = readKey.Read("ApplicationInstallPath");
//string SessionType = readKey.Read("SessionType");

//MessageBox.Show(GUID + ExtensibleMenu + Name + Path + ApplicationType + ApplicationInstallPath + SessionType);

//Lync Presense Timer
//LyncClientController y = new LyncClientController();
//y.SetUpTimer(new TimeSpan(14, 51, 00), new TimeSpan(15, 40, 0), new TimeSpan(20, 00, 0));

//Run User Access Control checks
//UAC uac = new UAC();
//Load load = new Load();
//load.getUACStatus();




//Application.EnableVisualStyles();
//Application.SetCompatibleTextRenderingDefault(false);

////Create settings file if it doesnt exist
//Settings.CreateSettings();

////Load settings
//Settings.LoadSettings();

////Run Lync registry modifcations
//Lync.CreateLyncMenu();

////Run registry Watcher
//Lync.RegistryWatcher();

////Get shared instance of LyncClientController - pass to other instances that require it
//LyncClientController lyncClientController = new LyncClientController();

////Run Lync Presence Switcher
//LyncPresenceSwitcher lyncPresenceSwitcher = new LyncPresenceSwitcher();
//lyncPresenceSwitcher.lync = lyncClientController;

////ProcessIcon p;
////try
////{
////    p = new ProcessIcon();

////}
////catch (InvalidOperationException)
////{
////    // ignore the exception
////}
////catch (InvalidTimeZoneException ex)
////{
////    MessageBox.Show(ex.ToString());
////}
////catch
////{
////    throw; //oh god oh god
////}
////finally
////{
////    if (p != null)
////    {
////        p.Dispose();
////    }
////    p = null;
////}


//// Show the system tray icon.					
//using (ProcessIcon pi = new ProcessIcon())
//{
//    //Use to pass instance of ProcessIcon to LyncPresenceSwitcher
//    lyncPresenceSwitcher.processIcon = pi;

//    //Pass Lync instance
//    pi.lync = lyncClientController;

//    pi.Display();

//    // Make sure the application runs!
//    Application.Run();
//}