using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

//http://www.codeproject.com/Articles/290013/Formless-System-Tray-Application

namespace UCExtend
{
	partial class AboutBox : Form
	{

        //Store relevant app settings in variables
        string supportTitle = Settings.appSettings.Element("Configuration").Element("Support").Element("SupportTitle").Value;
        string phone = Settings.appSettings.Element("Configuration").Element("Support").Element("Phone").Value;
        string email = Settings.appSettings.Element("Configuration").Element("Support").Element("Email").Value;
        string emailSubject = Settings.appSettings.Element("Configuration").Element("Support").Element("EmailSubject").Value;
        string web = Settings.appSettings.Element("Configuration").Element("Support").Element("Web").Value;

        public AboutBox()
		{
			try
            {
                InitializeComponent();

                this.Text = supportTitle;
                this.pictureBox1.Image = System.Drawing.Image.FromFile(@"Images\support_logo.png");

                labelContact.Text = supportTitle;
                linkLabelPhone.Text = phone;
                linkLabelEmail.Text = email;
                linkLabelWeb.Text = web;
            }
            catch (Exception ex)
            {
                // AAAAAAAAAAARGH, an error!
                Shared.MB("Error initialising about box  : " + ex.Message, "ERROR!");
            }
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

        private void linkLabelPhone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("tel:" + phone);
            }
            catch
            {
            }
        }

        private void linkLabelEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string mailto = "mailto:" + email + "?subject=" + emailSubject;
                Process.Start(mailto);
            }
            catch
            {
            }
        }

        private void linkLabelWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(web);
            }
            catch
            {
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(web);
            }
            catch
            {
            }
        }
	}
}