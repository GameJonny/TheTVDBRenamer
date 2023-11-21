namespace TheTVDBFileRename
{
    partial class frmRename
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRename));
            this.txtUrls = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxTranlate = new System.Windows.Forms.CheckBox();
            this.pbUpdate = new System.Windows.Forms.ProgressBar();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUrls
            // 
            this.txtUrls.Location = new System.Drawing.Point(46, 76);
            this.txtUrls.Multiline = true;
            this.txtUrls.Name = "txtUrls";
            this.txtUrls.Size = new System.Drawing.Size(717, 261);
            this.txtUrls.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "TV.DB URLs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Folder";
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(108, 12);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(574, 26);
            this.txtFolder.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(52, 410);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(726, 116);
            this.label3.TabIndex = 6;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(688, 9);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 33);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(46, 676);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(717, 38);
            this.btnRename.TabIndex = 3;
            this.btnRename.Text = "Rename My Files";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(8, 21);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(91, 25);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click Here ";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(688, 119);
            this.label4.TabIndex = 8;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(46, 529);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(717, 141);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Translation Of Episode Titles";
            // 
            // cbxTranlate
            // 
            this.cbxTranlate.AutoSize = true;
            this.cbxTranlate.Checked = global::TheTVDBFileRename.Properties.Settings.Default.EnableTranslation;
            this.cbxTranlate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxTranlate.Location = new System.Drawing.Point(596, 47);
            this.cbxTranlate.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.cbxTranlate.Name = "cbxTranlate";
            this.cbxTranlate.Size = new System.Drawing.Size(167, 24);
            this.cbxTranlate.TabIndex = 9;
            this.cbxTranlate.Text = "Enable Translation";
            this.cbxTranlate.UseVisualStyleBackColor = true;
            this.cbxTranlate.CheckedChanged += new System.EventHandler(this.cbxTranlate_CheckedChanged);
            // 
            // pbUpdate
            // 
            this.pbUpdate.Location = new System.Drawing.Point(46, 343);
            this.pbUpdate.Name = "pbUpdate";
            this.pbUpdate.Size = new System.Drawing.Size(717, 26);
            this.pbUpdate.TabIndex = 10;
            // 
            // lblUpdate
            // 
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.Location = new System.Drawing.Point(46, 372);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(717, 31);
            this.lblUpdate.TabIndex = 11;
            this.lblUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmRename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 726);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.pbUpdate);
            this.Controls.Add(this.cbxTranlate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUrls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(822, 782);
            this.Name = "frmRename";
            this.Text = "TheTVDBFileRename";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbxTranlate;
        private System.Windows.Forms.ProgressBar pbUpdate;
        private System.Windows.Forms.Label lblUpdate;
    }
}

