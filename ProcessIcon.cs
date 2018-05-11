using System;
using System.Diagnostics;
using System.Windows.Forms;
using UCExtend.Properties;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

//http://www.codeproject.com/Articles/290013/Formless-System-Tray-Application

namespace UCExtend
{
	/// <summary>
	/// 
	/// </summary>
	class ProcessIcon : IDisposable
	{
		/// <summary>
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;
	    private ContextMenus menu;
        static string appIcon = Application.StartupPath + @"\Images\app_icon.ico";

        //Use to pass shared LyncClientController instance invoked from Program
        public LyncClientController lync;

		public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();
		}

        public void BalloonTip(string title, string message, int timeToShow)
	    {
            ni.BalloonTipTitle = title;
            ni.BalloonTipText = message;
            ni.Visible = true;
            ni.ShowBalloonTip(timeToShow);
            //ni.BalloonTipIcon = ToolTipIcon.Error;
	    }

		/// <summary>
		/// Displays the icon in the system tray.
		/// </summary>
		public void Display()
		{
            try
            {
                // Put the icon in the system tray and allow it react to mouse clicks.			
                ni.MouseClick += new MouseEventHandler(ni_MouseClick);
                ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(appIcon);
                //(@"D:\Data\Documents\Visual Studio 2013\Projects\SystemTrayApp\SystemTrayApp\Images\app_icon.ico");
                //Resources.ucgeekicon;
                ni.Text = Settings.appSettings.Element("Configuration").Element("General").Element("AppName").Value;
                ni.Visible = true;

                // Attach a context menu.
                menu = new ContextMenus();
                //Pass Lync instance
                menu.lync = lync;
                ni.ContextMenuStrip = menu.Create();

                BalloonTip("HI! I'm UC Extend...", "I extend you Skype/Lync experience. I'll be running in the task tray should you need me :)" + 
                                        Environment.NewLine + Environment.NewLine + "Have a great day!!", 2000);

            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error loading task tray icon : " + e.Message, "ERROR!");
            }
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

		/// <summary>
		/// Handles the MouseClick event of the ni control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		void ni_MouseClick(object sender, MouseEventArgs mouseEvent)
		{
            try
            {
                // Handle mouse button clicks.
			    if (mouseEvent.Button == MouseButtons.Left)
			    {
				    // Start Windows Explorer.
				    Process.Start("lync.exe", null);
			    }
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error opening Lync client : " + e.Message, "ERROR!");
            }
		}
	}
}