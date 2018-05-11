using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Management;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Forms;


//http://www.codeproject.com/Articles/3389/Read-write-and-delete-from-registry-with-C

namespace UCExtend
{
    public class RegMon
    {
        ModifyRegistry regMod;

        ////Set BaseRegistryKey (LEGACY)
        //private RegistryKey baseRegistryKey = Registry.LocalMachine;
        ///// <summary>
        ///// A property to set the BaseRegistryKey value.
        ///// (default = Registry.LocalMachine)
        ///// </summary>
        //public RegistryKey BaseRegistryKey
        //{
        //    get { return baseRegistryKey; }
        //    set { baseRegistryKey = value; }
        //}

        //Added ability to specifiy x86 or x64 registry targets using RegistryKey.OpenBaseKey Method rather than BaseRegistryKey. 
        //Orignally written for the write method to stop re-direction to the Wow6432Node under software

        //Set RegistryView
        private RegistryView registryView = RegistryView.Default;
        /// <summary>
        /// A property to set the registryView value
        /// (default = RegistryView.Default)
        /// </summary>
        public RegistryView RegistryView
        {
            get { return registryView; }
            set { registryView = value; }
        }

        //Set RegistryHive
        private RegistryHive registryHive = RegistryHive.LocalMachine;
        /// <summary>
        /// A property to set the RegistryHive value
        /// (default = RegistryHive.LocalMachine)
        /// </summary>
        public RegistryHive RegistryHive
        {
            get { return registryHive; }
            set { registryHive = value; }
        }

        public void Monitor(string SubKey, string RegItem, string RegItemValue, RegistryValueKind Type)
        {
            #region Lync SDK
            //MessageBox.Show(Registry.Users.ToString());
            //HKEY_USERS
            //MessageBox.Show(Registry.CurrentUser.ToString());
            //HKEY_CURRENT_USER
            //MessageBox.Show(Registry.LocalMachine.ToString());
            //HKEY_LOCAL_MACHINE

//LYNC SDK STUFF
            //MessageBox.Show(noddy.UserName.ToString);

            //Self x = Self;

            // MessageBox.Show(Microsoft.Lync.Model.Self);

            // var client = LyncClient.GetClient();

            // var a = CredentialRequestedEventArgs;


            // CredentialRequestedEventArgs noddy = new CredentialRequestedEventArgs();
            // noddy.Start();
            // noddy.EventArrived += (sender, args) => KeyValueChanged(SubKey, RegItem, RegItemValue);
            #endregion Lync SDK
            
            var currentUser = WindowsIdentity.GetCurrent();
            WqlEventQuery query = null;
            
            //MessageBox.Show("Hive: " + registryHive.ToString());
            //MessageBox.Show("Base: " + baseRegistryKey.ToString());

            //if users key
            //if (registryHive.ToString() == "HKEY_USERS" || registryHive.ToString() == "HKEY_CURRENT_USER")
            if (registryHive.ToString() == "CurrentUser")
            {
                query = new WqlEventQuery(string.Format(
                "SELECT * FROM RegistryValueChangeEvent WHERE Hive='{0}' AND KeyPath='{1}\\\\{2}' AND ValueName='{3}'", Registry.Users.ToString(),
                currentUser.User.Value, SubKey.Replace("\\", "\\\\"), RegItem));
            }

            //if local machine key
            else if (registryHive.ToString() == "LocalMachine")
            {
                query = new WqlEventQuery(string.Format(
                "SELECT * FROM RegistryValueChangeEvent WHERE Hive='{0}' AND KeyPath='{1}\\\\{2}' AND ValueName='{3}'", Registry.Users.ToString(),
                currentUser.User.Value, SubKey.Replace("\\", "\\\\"), RegItem));
            }
            else
            {
                query = null;
            }

            if (query.QueryString != null)
            {
            //Need to check if query contains data!!
                //MessageBox.Show(query.QueryString.ToString());
                ManagementEventWatcher _watcher = new ManagementEventWatcher(query);
                //_watcher.EventArrived += _watcher_EventArrived;
                _watcher.Start();
                _watcher.EventArrived += (sender, args) => KeyValueChanged(SubKey, RegItem, RegItemValue, Type);

            //var regPath = "Software\\Microsoft\\Office\\15.0\\Lync\\andrew.morpeth@lexel.co.nz\\Autodiscovery";
            //var regValueName = "InternalEcpUrl";
            }
        }

        private void KeyValueChanged(string SubKey, string RegItem, string RegItemValue, RegistryValueKind Type)
        {
            object RegItemValueConverted;

            regMod = new ModifyRegistry();
            //Read registry key
            regMod.RegistryView = RegistryView;
            regMod.RegistryHive = RegistryHive;
            //regMod.BaseRegistryKey = BaseRegistryKey;
            //regMod.BaseRegistryKey = Registry.LocalMachine;
            regMod.SubKey = SubKey;
            //regMod.SubKey = "Software\\Test";
            var regRead = regMod.Read(RegItem);
            ////////////////////////////MessageBox.Show("KeyValueChanged: " + regRead);
            //RegItem = "Test";
            //RegItemValue = "098765432";

            //MessageBox.Show(regMod.BaseRegistryKey.ToString());
            //MessageBox.Show("RegItem: " + RegItem);
            //MessageBox.Show("SubKey: " + regMod.SubKey);
            //MessageBox.Show("RegItem: " + RegItem);
            //MessageBox.Show("RegItemValue: " + RegItemValue);

            //Convert string value to correct type
            if (Type == RegistryValueKind.Binary)
            {
                int numOfBytes = RegItemValue.Length;
                byte[] bytes = new byte[numOfBytes/2];
                for (int i = 0; i < numOfBytes/2; ++i)
                {
                    bytes[i] = Convert.ToByte(RegItemValue.Substring((i*2),2));
                }

                RegItemValueConverted = bytes;
            }
            else if (Type == RegistryValueKind.DWord)
            {
                RegItemValueConverted = Convert.ToInt32(RegItemValue);
            }
            else if (Type == RegistryValueKind.QWord)
            {
                RegItemValueConverted = Convert.ToInt64(RegItemValue);
            }
            else
            {
                RegItemValueConverted = RegItemValue;
            }

            if (regRead != null)
            {
                regMod.Write(RegItem, RegItemValueConverted, Type);//RegistryValueKind.String
            }
            // e.NewEvent.
            // MessageBox.Show(e.ToString());
            //https://lxlexc5.lexel.local/ecp/?rfr=olk&p=customize/voicemail.aspx&exsvurl=1&realm=lexel.co.nz
        }

        //void _watcher_EventArrived(object sender, EventArrivedEventArgs e)
        //{
        //    regMod = new ModifyRegistry();
        //    //Read registry key
        //    regMod.BaseRegistryKey = Registry.CurrentUser;
        //    regMod.SubKey = "Software\\Microsoft\\Office\\15.0\\Lync\\andrew.morpeth@lexel.co.nz\\Autodiscovery";
        //    var regRead = regMod.Read("InternalEcpUrl");
            
        //    MessageBox.Show(regRead);

        //    if(regRead != null)
        //    {
        //        regMod.Write("InternalEcpUrl", "https://mail.lynconline.co.nz/ecp/?rfr=olk&p=customize/voicemail.aspx&exsvurl=1&realm=lynconline.co.nz");
        //    }
        //    // e.NewEvent.
        //    // MessageBox.Show(e.ToString());
        //    //https://lxlexc5.lexel.local/ecp/?rfr=olk&p=customize/voicemail.aspx&exsvurl=1&realm=lexel.co.nz
        //}
    }

    public class ModifyRegistry
    {
        private bool showError = false;
        /// <summary>
        /// A property to show or hide error messages 
        /// (default = false)
        /// </summary>
        public bool ShowError
        {
            get { return showError; }
            set { showError = value; }
        }

        private string subKey = "SOFTWARE\\" + Application.ProductName.ToUpper();
        /// <summary>
        /// A property to set the SubKey value
        /// (default = "SOFTWARE\\" + Application.ProductName.ToUpper())
        /// </summary>
        public string SubKey
        {
            get { return subKey; }
            set { subKey = value; }
        }

        private RegistryKey baseRegistryKey = Registry.LocalMachine;
        /// <summary>
        /// A property to set the BaseRegistryKey value.
        /// (default = Registry.LocalMachine)
        /// </summary>
        public RegistryKey BaseRegistryKey
        {
            get { return baseRegistryKey; }
            set { baseRegistryKey = value; }
        }

        //Added ability to specifiy x86 or x64 registry targets using RegistryKey.OpenBaseKey Method rather than BaseRegistryKey. 
        //Orignally written for the write method to stop re-direction to the Wow6432Node under software

        //Set RegistryView
        private RegistryView registryView = RegistryView.Default;
        /// <summary>
        /// A property to set the registryView value
        /// (default = RegistryView.Default)
        /// </summary>
        public RegistryView RegistryView
        {
            get { return registryView; }
            set { registryView = value; }
        }
        
        //Set RegistryHive
        private RegistryHive registryHive = RegistryHive.LocalMachine;
        /// <summary>
        /// A property to set the RegistryHive value
        /// (default = RegistryHive.LocalMachine)
        /// </summary>
        public RegistryHive RegistryHive
        {
            get { return registryHive; }
            set { registryHive = value; }
        }


        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To read a registry key.
        /// input: KeyName (string)
        /// output: value (string) 
        /// </summary>
        public string Read(string KeyName)
        {
            // Opening the registry key
            //RegistryKey rk = baseRegistryKey;
            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive, registryView);
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    //return (string)sk1.GetValue(KeyName.ToUpper());
                    return Convert.ToString(sk1.GetValue(KeyName.ToUpper()));
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
                    return null;
                }
            }
        }

        public string[] ReadChildSubKeys()
        {
            // Opening the registry key
            //RegistryKey rk = baseRegistryKey;
            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive, registryView);
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return (string[])sk1.GetSubKeyNames();
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Reading registry subkeys");
                    return null;
                }
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To write into a registry key.
        /// input: KeyName (string) , Value (object)
        /// output: true or false 
        /// </summary>
        public bool Write(string KeyName, object Value, RegistryValueKind Type)
        {
            try
            {
                // Setting

                //Updated by Andrew M to suppport RegistryView
                //RegistryKey rk = baseRegistryKey;
                RegistryKey rk = RegistryKey.OpenBaseKey(registryHive, registryView);

                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // Save the value
                //sk1.SetValue(KeyName.ToUpper(), Value);
                sk1.SetValue(KeyName, Value, Type);
                
                //MessageBox.Show(baseRegistryKey.ToString());
                //MessageBox.Show(subKey);
                //MessageBox.Show(KeyName.ToUpper());
                //MessageBox.Show(Value.ToString());
                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To delete a registry key.
        /// input: KeyName (string)
        /// output: true or false 
        /// </summary>
        public bool DeleteKey(string KeyName)
        {
            try
            {
                // Setting
                //RegistryKey rk = baseRegistryKey;
                RegistryKey rk = RegistryKey.OpenBaseKey(registryHive, registryView);
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                else
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + subKey);
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To delete a sub key and any child.
        /// input: void
        /// output: true or false 
        /// </summary>
        public bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                //RegistryKey rk = baseRegistryKey;
                RegistryKey rk = RegistryKey.OpenBaseKey(registryHive, registryView);
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + subKey);
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// Retrive the count of subkeys at the current key.
        /// input: void
        /// output: number of subkeys
        /// </summary>
        public int SubKeyCount()
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.SubKeyCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Retriving subkeys of " + subKey);
                return 0;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// Retrive the count of values in the key.
        /// input: void
        /// output: number of keys
        /// </summary>
        public int ValueCount()
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.ValueCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Retriving keys of " + subKey);
                return 0;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        private void ShowErrorMessage(Exception e, string Title)
        {
            if (showError == true)
                MessageBox.Show(e.Message,
                                Title
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Error);
        }
    }
}
