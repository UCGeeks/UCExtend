using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Win32;
//using Microsoft.Lync.Controls;
//using Microsoft.Lync.Model;
//using Microsoft.Lync.Utilities;

namespace UCExtend
{
    public class Lync
    {
        public UserAccessControl uacLync = null;
        public static void CreateLyncMenu()
        {
            //Get UAC status
            UserAccessControl uacLyncMenu = new UserAccessControl();

            try
            {
                if (Settings.appSettings.Descendants("CustomMenuItems").Elements("MenuItem").Any())
                {
                    ////Request to elevate if required
                    //if (!uacLyncMenu.IsRunAsAdmin())
                    //{
                    //    if (MessageBox.Show("Application settings have enabled Lync custom menus, however you do not have the required permissions. To avoid seeing this message again run the application as admin, or remove this configuration from application settings." + Environment.NewLine + Environment.NewLine + "Would you like to restart as admin?", "Error creating Lync custom menus", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    //    {
                    //        uacLyncMenu.Elevate();
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }
                    //}
                
                    if (uacLyncMenu.IsRunAsAdmin())
                    {
                        //Read MenuItems from XML
                        foreach (var CustomMenuItem in Settings.appSettings.Descendants("CustomMenuItems").Descendants("MenuItem"))
                        {
                            //string GUID = CustomMenuItem.Element("GUID").Value;
                            //string extensibleMenu = CustomMenuItem.Element("ExtensibleMenu").Value;
                            //string name = CustomMenuItem.Element("Name").Value;
                            //string path = CustomMenuItem.Element("Path").Value;

                            //MessageBox.Show(GUID + extensibleMenu + name + path);

                            //Write registry entry
                            //string regSubKey = "Software\\Test\\" + CustomMenuItem.Element("GUID").Value;
                            string regSubKey = "SOFTWARE\\Microsoft\\Office\\15.0\\Lync\\SessionManager\\Apps\\" + CustomMenuItem.Element("GUID").Value;

                            foreach (var CustomMenuItemValue in CustomMenuItem.Elements())
                            {
                                if (CustomMenuItemValue.Name.ToString() != "GUID")
                                {
                                    //If Reg kind REG_SZ
                                    if (CustomMenuItemValue.Name == "ExtensibleMenu" ||
                                        CustomMenuItemValue.Name == "Path" ||
                                        CustomMenuItemValue.Name == "Name" ||
                                        CustomMenuItemValue.Name == "ApplicationInstallPath")
                                    {
                                        if (CustomMenuItemValue.Value != null || CustomMenuItemValue.Value != "")
                                        {
                                            //MessageBox.Show(CustomMenuItemValue.Name.ToString() + ":" + CustomMenuItemValue.Value.ToString());
                                            ModifyRegistry addKey = new ModifyRegistry();

                                            addKey.RegistryView = RegistryView.Registry32; //specifies to use the x64 registry node for x32 application. If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
                                            addKey.RegistryHive = RegistryHive.LocalMachine;
                                            addKey.ShowError = true;

                                            string regKeyName = CustomMenuItemValue.Name.ToString();
                                            string regValue = CustomMenuItemValue.Value.ToString();
                                            addKey.SubKey = regSubKey;

                                            addKey.Write(regKeyName, regValue, RegistryValueKind.String);
                                        }
                                    }
                                    //If Reg kind DWORD
                                    else if (CustomMenuItemValue.Name == "ApplicationType" ||
                                             CustomMenuItemValue.Name == "SessionType")
                                    {
                                        if (CustomMenuItemValue.Value != null || CustomMenuItemValue.Value != "")
                                        {
                                            ModifyRegistry addKey = new ModifyRegistry();

                                            addKey.RegistryView = RegistryView.Registry32; //specifies to use the x64 registry node for x32 application. If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
                                            addKey.RegistryHive = RegistryHive.LocalMachine;
                                            addKey.ShowError = true;

                                            string regKeyName = CustomMenuItemValue.Name.ToString();
                                            string regValue = CustomMenuItemValue.Value.ToString();
                                            addKey.SubKey = regSubKey;

                                            addKey.Write(regKeyName, regValue, RegistryValueKind.DWord);
                                        }
                                    }
                                }

                                //Add Application Type
                                //ModifyRegistry addKeyAppType = new ModifyRegistry();
                                //addKeyAppType.RegistryView = RegistryView.Registry32;
                                //addKeyAppType.RegistryHive = RegistryHive.LocalMachine;
                                //addKeyAppType.SubKey = regSubKey;
                                //addKeyAppType.ShowError = true;
                                //addKeyAppType.Write("ApplicationType", 1, RegistryValueKind.DWord);
                            }

                            #region No longer required
                            //Read Lync custom menu items from XML
                            //Delete the existing menu items
                            //Read existing subkeys
                            //ModifyRegistry readChildSubKeys = new ModifyRegistry();
                            //readChildSubKeys.RegistryView = RegistryView.Registry64;//specifies to use the x64 registry node for x32 application. If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
                            //readChildSubKeys.RegistryHive = RegistryHive.LocalMachine;
                            //readChildSubKeys.SubKey = "Software\\Test";
                            //readChildSubKeys.ShowError = true;
                            //string[] xxxx = readChildSubKeys.ReadChildSubKeys();

                            //foreach (var item in xxxx)
                            //{
                            //Delete found subkeys
                            //MessageBox.Show(item);
                            //}

                            //ModifyRegistry deleteKey = new ModifyRegistry();
                            //deleteKey.RegistryView = RegistryView.Registry64;//specifies to use the x64 registry node for x32 application. If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
                            //deleteKey.RegistryHive = RegistryHive.LocalMachine;
                            //deleteKey.SubKey = "Software\\Test";
                            //deleteKey.ShowError = true;
                            //deleteKey.DeleteSubKeyTree();

                            //Generate GUID
                            //Guid id = Guid.NewGuid();
                            #endregion OLD
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //AAAAAAAAAAARGH, an error!
                Shared.MB("Error adding Lync custom menus : " + ex.Message, "ERROR!");
            }
    }

        public static void RegistryWatcher()
        {
            //Monitor registry for changes, and on change update/revert
            try
            {
                //Read MenuItems from XML
                foreach (var registryWatcher in Settings.appSettings.Descendants("RegistryWatchers").Descendants("RegistryWatcher"))
                {
                    string registryHive = registryWatcher.Element("RegistryHive").Value;
                    string registryView = registryWatcher.Element("RegistryView").Value;
                    string subKey = registryWatcher.Element("SubKey").Value;
                    string regItem = registryWatcher.Element("RegItem").Value;
                    string regType = registryWatcher.Element("RegType").Value;
                    string regItemValue = registryWatcher.Element("RegItemValue").Value;

                    RegistryValueKind regValueKind = RegistryValueKind.String;

                    //string baseRegistryKey = "HKEY_USERS";
                    //string subKey = "Software\\Microsoft\\Office\\15.0\\Lync\\andrew.morpeth@lexel.co.nz\\Autodiscovery";
                    //string regItem = "InternalEcpUrl";
                    //string regItemValue = @"https://mail.lynconline.co.nz/ecp/?rfr=olk&p=customize/voicemail.aspx&exsvurl=1&realm=lynconline.co.nz";
                    //string regItemValue = @"TEST1";
                    //MessageBox.Show("Watcher: " + baseRegistryKey + " :: " + registryView + " :: " + subKey + " :: " + regItem + " :: " + regItemValue);

                    //Monitor InternalEcpUrl
                    //InternalEcpUrl.BaseRegistryKey = Registry.CurrentUser;
                    RegMon regMon = new RegMon();

                    //Registry View
                    if (registryView == "Registry32")
                    {
                        regMon.RegistryView = RegistryView.Registry32;
                    }
                    else if (registryView == "Registry64")
                    {
                        regMon.RegistryView = RegistryView.Registry64;
                    }

                    //Registry Value kind
                    if (regType.ToLower() == "string")
                    {
                        regValueKind = RegistryValueKind.String;
                    }
                    else if (regType.ToLower() == "binary")
                    {
                        regValueKind = RegistryValueKind.Binary;
                    }
                    else if (regType.ToLower() == "dword")
                    {
                        regValueKind = RegistryValueKind.DWord;
                    }
                    else if (regType.ToLower() == "qword")
                    {
                        regValueKind = RegistryValueKind.QWord;
                    }

                    //Registry Hive
                    if (registryHive == "CurrentUser")
                    {
                        regMon.RegistryHive = RegistryHive.CurrentUser;
                    }
                    else if (registryHive == "LocalMachine")
                    {
                        regMon.RegistryHive = RegistryHive.LocalMachine;

                        //Prompt to elevate if required
                        UserAccessControl uacRegWatcher = new UserAccessControl();

                        if (!uacRegWatcher.IsRunAsAdmin())
                        {
                            if (MessageBox.Show("The application does not have the required permissions to monitor the LocalMachine registry hive, please try running as administrator or check your permissions.", "Registry Watcher", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                uacRegWatcher.Elevate();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    //MessageBox.Show(subKey + regItem + regItemValue + RegistryValueKind.String);
                    regMon.Monitor(subKey, regItem, regItemValue, regValueKind);

                    ////Read registry key
                    //regMod.BaseRegistryKey = Registry.CurrentUser;
                    //regMod.SubKey = "Software\\Microsoft\\Office\\15.0\\Lync\\andrew.morpeth@lexel.co.nz\\Autodiscovery";
                    //var x = regMod.Read("InternalEcpUrl");
                    //MessageBox.Show(x);
                }
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error monitoring regisrty for changes : " + e.Message, "ERROR!");
            }
        }
    }
}
