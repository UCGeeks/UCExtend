namespace UCExtend
{
	partial class AboutBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelContact = new System.Windows.Forms.Label();
            this.labelWeb = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelPhone = new System.Windows.Forms.Label();
            this.linkLabelPhone = new System.Windows.Forms.LinkLabel();
            this.linkLabelWeb = new System.Windows.Forms.LinkLabel();
            this.linkLabelEmail = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(336, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelContact
            // 
            this.labelContact.AutoSize = true;
            this.labelContact.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContact.Location = new System.Drawing.Point(12, 97);
            this.labelContact.Name = "labelContact";
            this.labelContact.Size = new System.Drawing.Size(123, 21);
            this.labelContact.TabIndex = 1;
            this.labelContact.Text = "Contact Support";
            // 
            // labelWeb
            // 
            this.labelWeb.AutoSize = true;
            this.labelWeb.Location = new System.Drawing.Point(21, 173);
            this.labelWeb.Name = "labelWeb";
            this.labelWeb.Size = new System.Drawing.Size(34, 13);
            this.labelWeb.TabIndex = 2;
            this.labelWeb.Text = "Web:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(18, 151);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(37, 13);
            this.labelEmail.TabIndex = 3;
            this.labelEmail.Text = "Email:";
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(15, 128);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(43, 13);
            this.labelPhone.TabIndex = 4;
            this.labelPhone.Text = "Phone:";
            // 
            // linkLabelPhone
            // 
            this.linkLabelPhone.AutoSize = true;
            this.linkLabelPhone.Location = new System.Drawing.Point(57, 128);
            this.linkLabelPhone.Name = "linkLabelPhone";
            this.linkLabelPhone.Size = new System.Drawing.Size(84, 13);
            this.linkLabelPhone.TabIndex = 5;
            this.linkLabelPhone.TabStop = true;
            this.linkLabelPhone.Text = "+64 9 500 5000";
            this.linkLabelPhone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPhone_LinkClicked);
            // 
            // linkLabelWeb
            // 
            this.linkLabelWeb.AutoSize = true;
            this.linkLabelWeb.Location = new System.Drawing.Point(57, 173);
            this.linkLabelWeb.Name = "linkLabelWeb";
            this.linkLabelWeb.Size = new System.Drawing.Size(126, 13);
            this.linkLabelWeb.TabIndex = 6;
            this.linkLabelWeb.TabStop = true;
            this.linkLabelWeb.Text = "http://www.ucgeek.nz/";
            this.linkLabelWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWeb_LinkClicked);
            // 
            // linkLabelEmail
            // 
            this.linkLabelEmail.AutoSize = true;
            this.linkLabelEmail.Location = new System.Drawing.Point(57, 151);
            this.linkLabelEmail.Name = "linkLabelEmail";
            this.linkLabelEmail.Size = new System.Drawing.Size(111, 13);
            this.linkLabelEmail.TabIndex = 7;
            this.linkLabelEmail.TabStop = true;
            this.linkLabelEmail.Text = "support@ucgeek.nz";
            this.linkLabelEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEmail_LinkClicked);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 215);
            this.Controls.Add(this.linkLabelEmail);
            this.Controls.Add(this.linkLabelWeb);
            this.Controls.Add(this.linkLabelPhone);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelWeb);
            this.Controls.Add(this.labelContact);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelContact;
        private System.Windows.Forms.Label labelWeb;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.LinkLabel linkLabelPhone;
        private System.Windows.Forms.LinkLabel linkLabelWeb;
        private System.Windows.Forms.LinkLabel linkLabelEmail;

    }
}
