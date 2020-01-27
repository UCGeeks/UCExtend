namespace UCExtend
{
    partial class VideoPlayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.btnGo = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnCtoJ = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(707, 9);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(37, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(701, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // btnCtoJ
            // 
            this.btnCtoJ.Location = new System.Drawing.Point(916, 12);
            this.btnCtoJ.Name = "btnCtoJ";
            this.btnCtoJ.Size = new System.Drawing.Size(56, 23);
            this.btnCtoJ.TabIndex = 5;
            this.btnCtoJ.Text = "C#->JS";
            this.btnCtoJ.UseVisualStyleBackColor = true;
            this.btnCtoJ.Click += new System.EventHandler(this.btnCtoJ_Click);
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 711);
            this.Controls.Add(this.btnCtoJ);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnGo);
            this.Name = "VideoPlayer";
            this.Text = "VideoPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoPlayer_FormClosing);
            this.Load += new System.EventHandler(this.VideoPlayer_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnCtoJ;
    }
}