using System;
using System.Diagnostics;
using System.Windows.Forms;
using UCExtend.Properties;
using System.Drawing;
using System.Xml.Linq;

//http://www.codeproject.com/Articles/290013/Formless-System-Tray-Application

namespace UCExtend
{
	class ContextMenus
	{
        // Is the About box displayed?
        bool isAboutLoaded = false;
        bool isVideoLoaded = false;
        bool isSettingsLoaded = false;
        static string menuIcon = Application.StartupPath + @"\Images\app_menuicon.png";

        //Use to pass shared LyncClientController instance invoked from Program
        public LyncClientController lync;

		public ContextMenuStrip Create()
		{
            try
            {
                //MessageBox.Show(Shared.appSettings.ToString());
                // Add the default menu options.
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem item;
                ToolStripSeparator sep;

                // Title
                item = new ToolStripMenuItem();
                item.Text = Settings.appSettings.Element("Configuration").Element("General").Element("AppName").Value;
                //item.CanSelect = false;
                item.Image = System.Drawing.Image.FromFile(menuIcon);
                menu.Items.Add(item);

                // Separator
                sep = new ToolStripSeparator();
                menu.Items.Add(sep);

                //Read MenuItems from XML
                foreach (var menuItem in Settings.appSettings.Descendants("MenuItems").Descendants("MenuItem"))
                {
                    string name = menuItem.Element("Name").Value;
                    string image = menuItem.Element("Image").Value;
                    string action = menuItem.Element("Action").Value;

                    item = new ToolStripMenuItem();
                    item.Text = name;
                    item.Click += new EventHandler((s, e) => Action_Click(s, e, action));
                    if (image != "")
                    {
                        item.Image = Image.FromFile(Application.StartupPath + image);
                    }

                    menu.Items.Add(item);
                }

                #region hardcoded menu items
                //// Voicemail Setup
                //item = new ToolStripMenuItem();
                //item.Text = "Voicemail Setup";
                //item.Click += new EventHandler(VoicemailSetup_Click);
                //item.Image = Resources.About;
                //menu.Items.Add(item);

                //// My Account
                //item = new ToolStripMenuItem();
                //item.Text = "My Account";
                //item.Click += new EventHandler(MyAccount_Click);
                //item.Image = Resources.About;
                //menu.Items.Add(item);

                //// Online Help
                //item = new ToolStripMenuItem();
                //item.Text = "Online Help";
                //item.Click += new EventHandler(OnlineHelp_Click);
                //item.Image = Resources.About;
                //menu.Items.Add(item);

                //// Contact
                //item = new ToolStripMenuItem();
                //item.Text = "Online Help";
                //item.Click += new EventHandler(OnlineHelp_Click);
                //item.Image = Resources.About;
                //menu.Items.Add(item);

                //// About
                //item = new ToolStripMenuItem();
                //item.Text = "About";
                //item.Click += new EventHandler(About_Click);
                //item.Image = Resources.About;
                //menu.Items.Add(item);

                #endregion

                // Separator
                sep = new ToolStripSeparator();
                menu.Items.Add(sep);

                //// About
                item = new ToolStripMenuItem();
                item.Text = "Videos";
                item.Click += new EventHandler(Video_Click);
                item.Image = Resources.About;
                menu.Items.Add(item);

                //Read support details from settings
                //// About
                item = new ToolStripMenuItem();
                item.Text = Settings.appSettings.Element("Configuration").Element("Support").Element("NameInMenu").Value;
                item.Click += new EventHandler(About_Click);
                item.Image = Resources.About;
                menu.Items.Add(item);

                //// Settings
                item = new ToolStripMenuItem();
                item.Text =  "Settings";
                item.Click += new EventHandler(Settings_Click);
                item.Image = Resources.About;
                menu.Items.Add(item);

                // Exit
                item = new ToolStripMenuItem();
                item.Text = "Exit";
                item.Click += new System.EventHandler(Exit_Click);
                item.Image = Resources.Exit;
                menu.Items.Add(item);

                return menu;
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error creating menu options : " + ex.Message, "ERROR!");
                return null;
            }
		}

        //Handles the Click event of the Explorer control
        //Handles click event for XML imported configuration
        void Action_Click(object sender, EventArgs e, string action)
        {
            try
            {
                Process.Start(action, null);
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error on click event : " + ex.Message, "ERROR!");
            }
        }
        #region hardcoded handlers
        //void VoicemailSetup_Click(object sender, EventArgs e)
        //{
        //    Process.Start("https://mail.lynconline.co.nz/ecp/?rfr=olk&p=customize/voicemail.aspx", null);
        //}

        //void MyAccount_Click(object sender, EventArgs e)
        //{
        //    Process.Start("https://cp.lexel.co.nz//UserConfig/UserSettings.aspx", null);
        //}

        //void OnlineHelp_Click(object sender, EventArgs e)
        //{
        //    Process.Start("http://blog.lynconline.co.nz", null);
        //}
        #endregion hardcoded handlers


        // Handles the Click event of the About control
		void About_Click(object sender, EventArgs e)
		{
            try
            {
                if (!isAboutLoaded)
                {
                    isAboutLoaded = true;
                    new AboutBox().ShowDialog();
                    isAboutLoaded = false;
                }
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error on click event : " + ex.Message, "ERROR!");
            }
		}

        void Video_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isVideoLoaded)
                {
                    isVideoLoaded = true;
                    new VideoPlayer().ShowDialog();
                    isVideoLoaded = false;
                }
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error on click event : " + ex.Message, "ERROR!");
            }
        }

        // Handles the Click event of the settings control
        void Settings_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isSettingsLoaded)
                {
                    isSettingsLoaded = true;
                    SettingsBox box = new SettingsBox();
                    
                    //Pass Lync instance
                    box.lync = lync;

                    box.ShowDialog();
                    isSettingsLoaded = false;
                }
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error on click event : " + ex.Message, "ERROR!");
            }
        }

        // Handles the Click event of the exit control
		void Exit_Click(object sender, EventArgs e)
		{
			// Quit without further ado.
            //MessageBox.Show("Exit");
			Application.Exit();
		}
	}
}