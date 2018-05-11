using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace UCExtend
{    
    public static class Shared
    {
        //Error message box
        public static void MB(string Message, string Title)
        {
            MessageBox.Show(Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Start on login
        private static void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //if (chkStartUp.Checked)
            //    rk.SetValue(AppName, Application.ExecutablePath.ToString());
            //else
            //    rk.DeleteValue(AppName, false);

        }
    }

}
