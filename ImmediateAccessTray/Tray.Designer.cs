namespace ImmediateAccessTray
{
    partial class Tray
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpStatus = new System.Windows.Forms.TabPage();
            this.tlpStatus = new System.Windows.Forms.TableLayoutPanel();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.lblServiceToggle = new System.Windows.Forms.Label();
            this.lblServStatus = new System.Windows.Forms.Label();
            this.btnToggleService = new System.Windows.Forms.Button();
            this.tpLogs = new System.Windows.Forms.TabPage();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.tlpAbout = new System.Windows.Forms.TableLayoutPanel();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblVersionTitle = new System.Windows.Forms.Label();
            this.lblWebsite = new System.Windows.Forms.LinkLabel();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblAuthorTitle = new System.Windows.Forms.Label();
            this.lblWebsiteTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tpStatus.SuspendLayout();
            this.tlpStatus.SuspendLayout();
            this.tpAbout.SuspendLayout();
            this.tlpAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tpStatus);
            this.tabControl.Controls.Add(this.tpLogs);
            this.tabControl.Controls.Add(this.tpAbout);
            this.tabControl.Location = new System.Drawing.Point(5, 87);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(707, 279);
            this.tabControl.TabIndex = 0;
            // 
            // tpStatus
            // 
            this.tpStatus.AutoScroll = true;
            this.tpStatus.Controls.Add(this.tlpStatus);
            this.tpStatus.Location = new System.Drawing.Point(4, 26);
            this.tpStatus.Margin = new System.Windows.Forms.Padding(0);
            this.tpStatus.Name = "tpStatus";
            this.tpStatus.Padding = new System.Windows.Forms.Padding(10);
            this.tpStatus.Size = new System.Drawing.Size(699, 249);
            this.tpStatus.TabIndex = 0;
            this.tpStatus.Text = "Status";
            this.tpStatus.UseVisualStyleBackColor = true;
            // 
            // tlpStatus
            // 
            this.tlpStatus.ColumnCount = 2;
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatus.Controls.Add(this.lblServiceStatus, 1, 0);
            this.tlpStatus.Controls.Add(this.lblServiceToggle, 0, 1);
            this.tlpStatus.Controls.Add(this.lblServStatus, 0, 0);
            this.tlpStatus.Controls.Add(this.btnToggleService, 1, 1);
            this.tlpStatus.Location = new System.Drawing.Point(10, 10);
            this.tlpStatus.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatus.Name = "tlpStatus";
            this.tlpStatus.RowCount = 3;
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatus.Size = new System.Drawing.Size(257, 82);
            this.tlpStatus.TabIndex = 0;
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServiceStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblServiceStatus.Location = new System.Drawing.Point(130, 0);
            this.lblServiceStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(127, 30);
            this.lblServiceStatus.TabIndex = 5;
            this.lblServiceStatus.Text = "Unknown";
            this.lblServiceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServiceToggle
            // 
            this.lblServiceToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServiceToggle.Location = new System.Drawing.Point(0, 30);
            this.lblServiceToggle.Margin = new System.Windows.Forms.Padding(0);
            this.lblServiceToggle.Name = "lblServiceToggle";
            this.lblServiceToggle.Size = new System.Drawing.Size(130, 30);
            this.lblServiceToggle.TabIndex = 3;
            this.lblServiceToggle.Text = "Service Toggle";
            this.lblServiceToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServStatus
            // 
            this.lblServStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServStatus.Location = new System.Drawing.Point(0, 0);
            this.lblServStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblServStatus.Name = "lblServStatus";
            this.lblServStatus.Size = new System.Drawing.Size(130, 30);
            this.lblServStatus.TabIndex = 2;
            this.lblServStatus.Text = "Service Status";
            this.lblServStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnToggleService
            // 
            this.btnToggleService.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnToggleService.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnToggleService.Location = new System.Drawing.Point(130, 30);
            this.btnToggleService.Margin = new System.Windows.Forms.Padding(0);
            this.btnToggleService.Name = "btnToggleService";
            this.btnToggleService.Size = new System.Drawing.Size(93, 30);
            this.btnToggleService.TabIndex = 4;
            this.btnToggleService.Text = "Start / Stop";
            this.btnToggleService.UseVisualStyleBackColor = true;
            this.btnToggleService.Click += new System.EventHandler(this.btnToggleService_Click);
            // 
            // tpLogs
            // 
            this.tpLogs.Location = new System.Drawing.Point(4, 26);
            this.tpLogs.Margin = new System.Windows.Forms.Padding(4);
            this.tpLogs.Name = "tpLogs";
            this.tpLogs.Padding = new System.Windows.Forms.Padding(4);
            this.tpLogs.Size = new System.Drawing.Size(699, 249);
            this.tpLogs.TabIndex = 1;
            this.tpLogs.Text = "Logs";
            this.tpLogs.UseVisualStyleBackColor = true;
            // 
            // tpAbout
            // 
            this.tpAbout.AutoScroll = true;
            this.tpAbout.Controls.Add(this.tlpAbout);
            this.tpAbout.Location = new System.Drawing.Point(4, 26);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Padding = new System.Windows.Forms.Padding(10);
            this.tpAbout.Size = new System.Drawing.Size(699, 249);
            this.tpAbout.TabIndex = 2;
            this.tpAbout.Text = "About";
            this.tpAbout.UseVisualStyleBackColor = true;
            // 
            // tlpAbout
            // 
            this.tlpAbout.ColumnCount = 2;
            this.tlpAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAbout.Controls.Add(this.lblVersionTitle, 0, 0);
            this.tlpAbout.Controls.Add(this.tbDescription, 0, 3);
            this.tlpAbout.Controls.Add(this.lblWebsite, 1, 2);
            this.tlpAbout.Controls.Add(this.lblAuthor, 1, 1);
            this.tlpAbout.Controls.Add(this.lblAuthorTitle, 0, 1);
            this.tlpAbout.Controls.Add(this.lblWebsiteTitle, 0, 2);
            this.tlpAbout.Controls.Add(this.lblVersion, 1, 0);
            this.tlpAbout.Location = new System.Drawing.Point(10, 10);
            this.tlpAbout.MinimumSize = new System.Drawing.Size(150, 150);
            this.tlpAbout.Name = "tlpAbout";
            this.tlpAbout.RowCount = 4;
            this.tlpAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAbout.Size = new System.Drawing.Size(679, 229);
            this.tlpAbout.TabIndex = 8;
            // 
            // tbDescription
            // 
            this.tlpAbout.SetColumnSpan(this.tbDescription, 2);
            this.tbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDescription.Location = new System.Drawing.Point(0, 100);
            this.tbDescription.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(679, 129);
            this.tbDescription.TabIndex = 0;
            this.tbDescription.Text = "Description goes here...";
            // 
            // lblVersionTitle
            // 
            this.lblVersionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionTitle.Location = new System.Drawing.Point(3, 0);
            this.lblVersionTitle.Name = "lblVersionTitle";
            this.lblVersionTitle.Size = new System.Drawing.Size(94, 30);
            this.lblVersionTitle.TabIndex = 1;
            this.lblVersionTitle.Text = "Version:";
            this.lblVersionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebsite.Location = new System.Drawing.Point(103, 60);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(573, 30);
            this.lblWebsite.TabIndex = 5;
            this.lblWebsite.TabStop = true;
            this.lblWebsite.Text = "Website goes here...";
            this.lblWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblWebsite_LinkClicked);
            // 
            // lblAuthor
            // 
            this.lblAuthor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthor.Location = new System.Drawing.Point(103, 30);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(573, 30);
            this.lblAuthor.TabIndex = 7;
            this.lblAuthor.Text = "Author goes here...";
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAuthorTitle
            // 
            this.lblAuthorTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthorTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthorTitle.Location = new System.Drawing.Point(3, 30);
            this.lblAuthorTitle.Name = "lblAuthorTitle";
            this.lblAuthorTitle.Size = new System.Drawing.Size(94, 30);
            this.lblAuthorTitle.TabIndex = 6;
            this.lblAuthorTitle.Text = "Author:";
            this.lblAuthorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWebsiteTitle
            // 
            this.lblWebsiteTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebsiteTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebsiteTitle.Location = new System.Drawing.Point(3, 60);
            this.lblWebsiteTitle.Name = "lblWebsiteTitle";
            this.lblWebsiteTitle.Size = new System.Drawing.Size(94, 30);
            this.lblWebsiteTitle.TabIndex = 3;
            this.lblWebsiteTitle.Text = "Website:";
            this.lblWebsiteTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersion.Location = new System.Drawing.Point(103, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(573, 30);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version goes here...";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.Image = global::ImmediateAccessTray.Properties.Resources.Logo;
            this.pbLogo.Location = new System.Drawing.Point(627, 12);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(80, 80);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 16F);
            this.lblTitle.Location = new System.Drawing.Point(12, 28);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(593, 36);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Immediate Access";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Tray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(717, 371);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tray";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Immediate Access - VPN Service";
            this.Load += new System.EventHandler(this.Tray_Load);
            this.tabControl.ResumeLayout(false);
            this.tpStatus.ResumeLayout(false);
            this.tlpStatus.ResumeLayout(false);
            this.tpAbout.ResumeLayout(false);
            this.tlpAbout.ResumeLayout(false);
            this.tlpAbout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpStatus;
        private System.Windows.Forms.TabPage tpLogs;
        private System.Windows.Forms.TabPage tpAbout;
        private System.Windows.Forms.TableLayoutPanel tlpStatus;
        private System.Windows.Forms.Button btnToggleService;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.Label lblServiceToggle;
        private System.Windows.Forms.Label lblServStatus;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblWebsiteTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblVersionTitle;
        private System.Windows.Forms.LinkLabel lblWebsite;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblAuthorTitle;
        private System.Windows.Forms.TableLayoutPanel tlpAbout;
    }
}

