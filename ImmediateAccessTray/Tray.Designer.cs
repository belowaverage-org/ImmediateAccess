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
            this.lblServicePolicy = new System.Windows.Forms.Label();
            this.lblServicePolicyTitle = new System.Windows.Forms.Label();
            this.lblNetLocation = new System.Windows.Forms.Label();
            this.lblNetStatus = new System.Windows.Forms.Label();
            this.lblVpnStatus = new System.Windows.Forms.Label();
            this.lblNetLocationTitle = new System.Windows.Forms.Label();
            this.lblNetStatusTitle = new System.Windows.Forms.Label();
            this.lblVpnStatusTitle = new System.Windows.Forms.Label();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.lblServiceToggle = new System.Windows.Forms.Label();
            this.lblServStatus = new System.Windows.Forms.Label();
            this.btnToggleService = new System.Windows.Forms.Button();
            this.pbShieldIcon = new System.Windows.Forms.PictureBox();
            this.pnlLine1 = new System.Windows.Forms.Panel();
            this.tpLogs = new System.Windows.Forms.TabPage();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.tlpAbout = new System.Windows.Forms.TableLayoutPanel();
            this.lblVersionTitle = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblWebsite = new System.Windows.Forms.LinkLabel();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblAuthorTitle = new System.Windows.Forms.Label();
            this.lblWebsiteTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.HelpProvider = new System.Windows.Forms.HelpProvider();
            this.tabControl.SuspendLayout();
            this.tpStatus.SuspendLayout();
            this.tlpStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbShieldIcon)).BeginInit();
            this.tpLogs.SuspendLayout();
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
            this.tabControl.Location = new System.Drawing.Point(5, 45);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(588, 292);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tpStatus
            // 
            this.tpStatus.AutoScroll = true;
            this.tpStatus.Controls.Add(this.tlpStatus);
            this.tpStatus.Location = new System.Drawing.Point(4, 26);
            this.tpStatus.Margin = new System.Windows.Forms.Padding(0);
            this.tpStatus.Name = "tpStatus";
            this.tpStatus.Padding = new System.Windows.Forms.Padding(10);
            this.tpStatus.Size = new System.Drawing.Size(580, 262);
            this.tpStatus.TabIndex = 0;
            this.tpStatus.Text = "Status";
            this.tpStatus.UseVisualStyleBackColor = true;
            // 
            // tlpStatus
            // 
            this.tlpStatus.ColumnCount = 3;
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatus.Controls.Add(this.lblServicePolicy, 2, 1);
            this.tlpStatus.Controls.Add(this.lblServicePolicyTitle, 0, 1);
            this.tlpStatus.Controls.Add(this.lblNetLocation, 2, 6);
            this.tlpStatus.Controls.Add(this.lblNetStatus, 2, 5);
            this.tlpStatus.Controls.Add(this.lblVpnStatus, 2, 4);
            this.tlpStatus.Controls.Add(this.lblNetLocationTitle, 0, 6);
            this.tlpStatus.Controls.Add(this.lblNetStatusTitle, 0, 5);
            this.tlpStatus.Controls.Add(this.lblVpnStatusTitle, 0, 4);
            this.tlpStatus.Controls.Add(this.lblServiceStatus, 2, 0);
            this.tlpStatus.Controls.Add(this.lblServiceToggle, 0, 2);
            this.tlpStatus.Controls.Add(this.lblServStatus, 0, 0);
            this.tlpStatus.Controls.Add(this.btnToggleService, 2, 2);
            this.tlpStatus.Controls.Add(this.pbShieldIcon, 1, 2);
            this.tlpStatus.Controls.Add(this.pnlLine1, 0, 3);
            this.tlpStatus.Location = new System.Drawing.Point(10, 10);
            this.tlpStatus.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatus.Name = "tlpStatus";
            this.tlpStatus.RowCount = 8;
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpStatus.Size = new System.Drawing.Size(276, 217);
            this.tlpStatus.TabIndex = 0;
            // 
            // lblServicePolicy
            // 
            this.lblServicePolicy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServicePolicy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.HelpProvider.SetHelpString(this.lblServicePolicy, "This row shows whether or not the Immediate Access service is configured to run i" +
        "n Group Policy.");
            this.lblServicePolicy.Location = new System.Drawing.Point(155, 30);
            this.lblServicePolicy.Margin = new System.Windows.Forms.Padding(0);
            this.lblServicePolicy.Name = "lblServicePolicy";
            this.HelpProvider.SetShowHelp(this.lblServicePolicy, true);
            this.lblServicePolicy.Size = new System.Drawing.Size(121, 30);
            this.lblServicePolicy.TabIndex = 15;
            this.lblServicePolicy.Text = "Unknown";
            this.lblServicePolicy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServicePolicyTitle
            // 
            this.lblServicePolicyTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServicePolicyTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProvider.SetHelpString(this.lblServicePolicyTitle, "This row shows whether or not the Immediate Access service is configured to run i" +
        "n Group Policy.");
            this.lblServicePolicyTitle.Location = new System.Drawing.Point(0, 30);
            this.lblServicePolicyTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblServicePolicyTitle.Name = "lblServicePolicyTitle";
            this.HelpProvider.SetShowHelp(this.lblServicePolicyTitle, true);
            this.lblServicePolicyTitle.Size = new System.Drawing.Size(124, 30);
            this.lblServicePolicyTitle.TabIndex = 14;
            this.lblServicePolicyTitle.Text = "Service Policy";
            this.lblServicePolicyTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNetLocation
            // 
            this.lblNetLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.HelpProvider.SetHelpString(this.lblNetLocation, "This row displays the network location of the current PC. (Internal / External)");
            this.lblNetLocation.Location = new System.Drawing.Point(155, 181);
            this.lblNetLocation.Margin = new System.Windows.Forms.Padding(0);
            this.lblNetLocation.Name = "lblNetLocation";
            this.HelpProvider.SetShowHelp(this.lblNetLocation, true);
            this.lblNetLocation.Size = new System.Drawing.Size(121, 30);
            this.lblNetLocation.TabIndex = 13;
            this.lblNetLocation.Text = "Unknown";
            this.lblNetLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNetStatus
            // 
            this.lblNetStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.HelpProvider.SetHelpString(this.lblNetStatus, "This row shows whether or not this PC can reach the internal network either throu" +
        "gh the VPN or directly connected.");
            this.lblNetStatus.Location = new System.Drawing.Point(155, 151);
            this.lblNetStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblNetStatus.Name = "lblNetStatus";
            this.HelpProvider.SetShowHelp(this.lblNetStatus, true);
            this.lblNetStatus.Size = new System.Drawing.Size(121, 30);
            this.lblNetStatus.TabIndex = 12;
            this.lblNetStatus.Text = "Unknown";
            this.lblNetStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVpnStatus
            // 
            this.lblVpnStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVpnStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.HelpProvider.SetHelpString(this.lblVpnStatus, "This row displays the VPN connection status.");
            this.lblVpnStatus.Location = new System.Drawing.Point(155, 121);
            this.lblVpnStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblVpnStatus.Name = "lblVpnStatus";
            this.HelpProvider.SetShowHelp(this.lblVpnStatus, true);
            this.lblVpnStatus.Size = new System.Drawing.Size(121, 30);
            this.lblVpnStatus.TabIndex = 11;
            this.lblVpnStatus.Text = "Unknown";
            this.lblVpnStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNetLocationTitle
            // 
            this.lblNetLocationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetLocationTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProvider.SetHelpString(this.lblNetLocationTitle, "This row displays the network location of the current PC. (Internal / External)");
            this.lblNetLocationTitle.Location = new System.Drawing.Point(0, 181);
            this.lblNetLocationTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblNetLocationTitle.Name = "lblNetLocationTitle";
            this.HelpProvider.SetShowHelp(this.lblNetLocationTitle, true);
            this.lblNetLocationTitle.Size = new System.Drawing.Size(124, 30);
            this.lblNetLocationTitle.TabIndex = 10;
            this.lblNetLocationTitle.Text = "Network Location";
            this.lblNetLocationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNetStatusTitle
            // 
            this.lblNetStatusTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetStatusTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProvider.SetHelpString(this.lblNetStatusTitle, "This row shows whether or not this PC can reach the internal network either throu" +
        "gh the VPN or directly connected.");
            this.lblNetStatusTitle.Location = new System.Drawing.Point(0, 151);
            this.lblNetStatusTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblNetStatusTitle.Name = "lblNetStatusTitle";
            this.HelpProvider.SetShowHelp(this.lblNetStatusTitle, true);
            this.lblNetStatusTitle.Size = new System.Drawing.Size(124, 30);
            this.lblNetStatusTitle.TabIndex = 9;
            this.lblNetStatusTitle.Text = "Network Status";
            this.lblNetStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVpnStatusTitle
            // 
            this.lblVpnStatusTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVpnStatusTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProvider.SetHelpString(this.lblVpnStatusTitle, "This row displays the VPN connection status.");
            this.lblVpnStatusTitle.Location = new System.Drawing.Point(0, 121);
            this.lblVpnStatusTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblVpnStatusTitle.Name = "lblVpnStatusTitle";
            this.HelpProvider.SetShowHelp(this.lblVpnStatusTitle, true);
            this.lblVpnStatusTitle.Size = new System.Drawing.Size(124, 30);
            this.lblVpnStatusTitle.TabIndex = 8;
            this.lblVpnStatusTitle.Text = "VPN Status";
            this.lblVpnStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServiceStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.HelpProvider.SetHelpString(this.lblServiceStatus, "This row displays the Immediate Access Service Status. (Windows Service)");
            this.lblServiceStatus.Location = new System.Drawing.Point(155, 0);
            this.lblServiceStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.HelpProvider.SetShowHelp(this.lblServiceStatus, true);
            this.lblServiceStatus.Size = new System.Drawing.Size(121, 30);
            this.lblServiceStatus.TabIndex = 5;
            this.lblServiceStatus.Text = "Unknown";
            this.lblServiceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServiceToggle
            // 
            this.lblServiceToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServiceToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProvider.SetHelpString(this.lblServiceToggle, "This will start or stop the Immediate Access service.");
            this.lblServiceToggle.Location = new System.Drawing.Point(0, 60);
            this.lblServiceToggle.Margin = new System.Windows.Forms.Padding(0);
            this.lblServiceToggle.Name = "lblServiceToggle";
            this.HelpProvider.SetShowHelp(this.lblServiceToggle, true);
            this.lblServiceToggle.Size = new System.Drawing.Size(124, 31);
            this.lblServiceToggle.TabIndex = 3;
            this.lblServiceToggle.Text = "Service Toggle";
            this.lblServiceToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServStatus
            // 
            this.lblServStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpProvider.SetHelpString(this.lblServStatus, "This row displays the Immediate Access Service Status. (Windows Service)");
            this.lblServStatus.Location = new System.Drawing.Point(0, 0);
            this.lblServStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblServStatus.Name = "lblServStatus";
            this.HelpProvider.SetShowHelp(this.lblServStatus, true);
            this.lblServStatus.Size = new System.Drawing.Size(124, 30);
            this.lblServStatus.TabIndex = 2;
            this.lblServStatus.Text = "Service Status";
            this.lblServStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnToggleService
            // 
            this.btnToggleService.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnToggleService.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.HelpProvider.SetHelpString(this.btnToggleService, "This will start or stop the Immediate Access service.");
            this.btnToggleService.Location = new System.Drawing.Point(155, 60);
            this.btnToggleService.Margin = new System.Windows.Forms.Padding(0);
            this.btnToggleService.Name = "btnToggleService";
            this.HelpProvider.SetShowHelp(this.btnToggleService, true);
            this.btnToggleService.Size = new System.Drawing.Size(89, 31);
            this.btnToggleService.TabIndex = 4;
            this.btnToggleService.Text = "Start / Stop";
            this.btnToggleService.UseVisualStyleBackColor = true;
            this.btnToggleService.Click += new System.EventHandler(this.btnToggleService_Click);
            // 
            // pbShieldIcon
            // 
            this.HelpProvider.SetHelpString(this.pbShieldIcon, "This will start or stop the Immediate Access service.");
            this.pbShieldIcon.Location = new System.Drawing.Point(127, 63);
            this.pbShieldIcon.Name = "pbShieldIcon";
            this.HelpProvider.SetShowHelp(this.pbShieldIcon, true);
            this.pbShieldIcon.Size = new System.Drawing.Size(25, 24);
            this.pbShieldIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbShieldIcon.TabIndex = 6;
            this.pbShieldIcon.TabStop = false;
            // 
            // pnlLine1
            // 
            this.pnlLine1.BackColor = System.Drawing.Color.Gainsboro;
            this.tlpStatus.SetColumnSpan(this.pnlLine1, 3);
            this.pnlLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLine1.Location = new System.Drawing.Point(5, 106);
            this.pnlLine1.Margin = new System.Windows.Forms.Padding(5, 15, 0, 15);
            this.pnlLine1.Name = "pnlLine1";
            this.pnlLine1.Size = new System.Drawing.Size(271, 1);
            this.pnlLine1.TabIndex = 7;
            // 
            // tpLogs
            // 
            this.tpLogs.Controls.Add(this.rtbLogs);
            this.tpLogs.Location = new System.Drawing.Point(4, 26);
            this.tpLogs.Margin = new System.Windows.Forms.Padding(4);
            this.tpLogs.Name = "tpLogs";
            this.tpLogs.Padding = new System.Windows.Forms.Padding(4);
            this.tpLogs.Size = new System.Drawing.Size(580, 262);
            this.tpLogs.TabIndex = 1;
            this.tpLogs.Text = "Logs";
            this.tpLogs.UseVisualStyleBackColor = true;
            // 
            // rtbLogs
            // 
            this.rtbLogs.BackColor = System.Drawing.Color.Black;
            this.rtbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogs.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLogs.ForeColor = System.Drawing.Color.White;
            this.rtbLogs.Location = new System.Drawing.Point(4, 4);
            this.rtbLogs.Margin = new System.Windows.Forms.Padding(2);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(572, 254);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            // 
            // tpAbout
            // 
            this.tpAbout.AutoScroll = true;
            this.tpAbout.Controls.Add(this.tlpAbout);
            this.tpAbout.Location = new System.Drawing.Point(4, 26);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Padding = new System.Windows.Forms.Padding(10);
            this.tpAbout.Size = new System.Drawing.Size(580, 262);
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
            this.tlpAbout.Size = new System.Drawing.Size(557, 229);
            this.tlpAbout.TabIndex = 8;
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
            // tbDescription
            // 
            this.tlpAbout.SetColumnSpan(this.tbDescription, 2);
            this.tbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDescription.Location = new System.Drawing.Point(0, 100);
            this.tbDescription.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(557, 129);
            this.tbDescription.TabIndex = 0;
            this.tbDescription.Text = "Description goes here...";
            // 
            // lblWebsite
            // 
            this.lblWebsite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebsite.Location = new System.Drawing.Point(103, 60);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(451, 30);
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
            this.lblAuthor.Size = new System.Drawing.Size(451, 30);
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
            this.lblVersion.Size = new System.Drawing.Size(451, 30);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version goes here...";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.Image = global::ImmediateAccessTray.Properties.Resources.Logo;
            this.pbLogo.Location = new System.Drawing.Point(533, 5);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(60, 60);
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
            this.lblTitle.Location = new System.Drawing.Point(8, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(522, 36);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Immediate Access";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Tray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(598, 342);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(260, 260);
            this.Name = "Tray";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Immediate Access - VPN Service";
            this.Load += new System.EventHandler(this.Tray_Load);
            this.tabControl.ResumeLayout(false);
            this.tpStatus.ResumeLayout(false);
            this.tlpStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbShieldIcon)).EndInit();
            this.tpLogs.ResumeLayout(false);
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
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.PictureBox pbShieldIcon;
        private System.Windows.Forms.Panel pnlLine1;
        private System.Windows.Forms.Label lblNetLocationTitle;
        private System.Windows.Forms.Label lblNetStatusTitle;
        private System.Windows.Forms.Label lblVpnStatusTitle;
        private System.Windows.Forms.Label lblVpnStatus;
        private System.Windows.Forms.Label lblNetLocation;
        private System.Windows.Forms.Label lblNetStatus;
        private System.Windows.Forms.Label lblServicePolicyTitle;
        private System.Windows.Forms.Label lblServiceToggle;
        private System.Windows.Forms.HelpProvider HelpProvider;
        private System.Windows.Forms.Label lblServicePolicy;
    }
}

