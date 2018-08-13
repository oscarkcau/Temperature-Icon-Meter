namespace TemperatureIconMeter
{
	partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.treeViewSensors = new System.Windows.Forms.TreeView();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelSeparationBar3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelWarningTemperature = new System.Windows.Forms.Label();
            this.trackBarWarningThreshold = new System.Windows.Forms.TrackBar();
            this.trackBarDangerThreshold = new System.Windows.Forms.TrackBar();
            this.labelDangerTemperature = new System.Windows.Forms.Label();
            this.labelSafeTemperature = new System.Windows.Forms.Label();
            this.buttonSafeColor = new System.Windows.Forms.Button();
            this.buttonWarnColor = new System.Windows.Forms.Button();
            this.buttonDangerColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxMain = new System.Windows.Forms.ListBox();
            this.buttonItemUp = new System.Windows.Forms.Button();
            this.buttonItemDown = new System.Windows.Forms.Button();
            this.colorDialogMain = new System.Windows.Forms.ColorDialog();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxUseVerticalBar = new System.Windows.Forms.CheckBox();
            this.checkBoxRunAtStartup = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWarningThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDangerThreshold)).BeginInit();
            this.contextMenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewSensors
            // 
            this.treeViewSensors.AllowDrop = true;
            this.treeViewSensors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewSensors.CheckBoxes = true;
            this.treeViewSensors.Location = new System.Drawing.Point(9, 34);
            this.treeViewSensors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeViewSensors.Name = "treeViewSensors";
            this.treeViewSensors.Size = new System.Drawing.Size(523, 178);
            this.treeViewSensors.TabIndex = 0;
            this.treeViewSensors.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSensors_AfterCheck);
            // 
            // timerMain
            // 
            this.timerMain.Interval = 1000;
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "All Temperature Sensors";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(416, 664);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 35);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(297, 664);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(112, 35);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelSeparationBar3
            // 
            this.labelSeparationBar3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparationBar3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparationBar3.Location = new System.Drawing.Point(9, 648);
            this.labelSeparationBar3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSeparationBar3.Name = "labelSeparationBar3";
            this.labelSeparationBar3.Size = new System.Drawing.Size(522, 3);
            this.labelSeparationBar3.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(9, 416);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(522, 3);
            this.label4.TabIndex = 21;
            // 
            // labelWarningTemperature
            // 
            this.labelWarningTemperature.AutoSize = true;
            this.labelWarningTemperature.Location = new System.Drawing.Point(9, 486);
            this.labelWarningTemperature.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelWarningTemperature.Name = "labelWarningTemperature";
            this.labelWarningTemperature.Size = new System.Drawing.Size(236, 20);
            this.labelWarningTemperature.TabIndex = 25;
            this.labelWarningTemperature.Text = "Warning Temperature: 60 - 80°C";
            this.labelWarningTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarWarningThreshold
            // 
            this.trackBarWarningThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarWarningThreshold.Location = new System.Drawing.Point(279, 478);
            this.trackBarWarningThreshold.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trackBarWarningThreshold.Maximum = 99;
            this.trackBarWarningThreshold.Name = "trackBarWarningThreshold";
            this.trackBarWarningThreshold.Size = new System.Drawing.Size(146, 69);
            this.trackBarWarningThreshold.TabIndex = 26;
            this.trackBarWarningThreshold.TickFrequency = 10;
            this.trackBarWarningThreshold.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarWarningThreshold.Scroll += new System.EventHandler(this.trackBarWarningThreshold_Scroll);
            // 
            // trackBarDangerThreshold
            // 
            this.trackBarDangerThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarDangerThreshold.Location = new System.Drawing.Point(279, 520);
            this.trackBarDangerThreshold.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trackBarDangerThreshold.Maximum = 100;
            this.trackBarDangerThreshold.Minimum = 1;
            this.trackBarDangerThreshold.Name = "trackBarDangerThreshold";
            this.trackBarDangerThreshold.Size = new System.Drawing.Size(146, 69);
            this.trackBarDangerThreshold.TabIndex = 29;
            this.trackBarDangerThreshold.TickFrequency = 10;
            this.trackBarDangerThreshold.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarDangerThreshold.Value = 1;
            this.trackBarDangerThreshold.Scroll += new System.EventHandler(this.trackBarDangerThreshold_Scroll);
            // 
            // labelDangerTemperature
            // 
            this.labelDangerTemperature.AutoSize = true;
            this.labelDangerTemperature.Location = new System.Drawing.Point(9, 528);
            this.labelDangerTemperature.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDangerTemperature.Name = "labelDangerTemperature";
            this.labelDangerTemperature.Size = new System.Drawing.Size(212, 20);
            this.labelDangerTemperature.TabIndex = 28;
            this.labelDangerTemperature.Text = "Danger Temperature: > 80°C";
            this.labelDangerTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSafeTemperature
            // 
            this.labelSafeTemperature.AutoSize = true;
            this.labelSafeTemperature.Location = new System.Drawing.Point(9, 442);
            this.labelSafeTemperature.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSafeTemperature.Name = "labelSafeTemperature";
            this.labelSafeTemperature.Size = new System.Drawing.Size(193, 20);
            this.labelSafeTemperature.TabIndex = 31;
            this.labelSafeTemperature.Text = "Safe Temperature: < 60°C";
            this.labelSafeTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSafeColor
            // 
            this.buttonSafeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSafeColor.Location = new System.Drawing.Point(430, 434);
            this.buttonSafeColor.Name = "buttonSafeColor";
            this.buttonSafeColor.Size = new System.Drawing.Size(100, 35);
            this.buttonSafeColor.TabIndex = 32;
            this.buttonSafeColor.UseVisualStyleBackColor = true;
            this.buttonSafeColor.Click += new System.EventHandler(this.buttonSafeColor_Click);
            // 
            // buttonWarnColor
            // 
            this.buttonWarnColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWarnColor.Location = new System.Drawing.Point(430, 478);
            this.buttonWarnColor.Name = "buttonWarnColor";
            this.buttonWarnColor.Size = new System.Drawing.Size(100, 35);
            this.buttonWarnColor.TabIndex = 33;
            this.buttonWarnColor.UseVisualStyleBackColor = true;
            this.buttonWarnColor.Click += new System.EventHandler(this.buttonWarnColor_Click);
            // 
            // buttonDangerColor
            // 
            this.buttonDangerColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDangerColor.Location = new System.Drawing.Point(430, 520);
            this.buttonDangerColor.Name = "buttonDangerColor";
            this.buttonDangerColor.Size = new System.Drawing.Size(100, 35);
            this.buttonDangerColor.TabIndex = 34;
            this.buttonDangerColor.UseVisualStyleBackColor = true;
            this.buttonDangerColor.Click += new System.EventHandler(this.buttonDangerColor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(9, 222);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(258, 20);
            this.label3.TabIndex = 35;
            this.label3.Text = "Selected Temperature Sensors";
            // 
            // listBoxMain
            // 
            this.listBoxMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMain.FormattingEnabled = true;
            this.listBoxMain.ItemHeight = 20;
            this.listBoxMain.Location = new System.Drawing.Point(9, 249);
            this.listBoxMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBoxMain.Name = "listBoxMain";
            this.listBoxMain.Size = new System.Drawing.Size(446, 144);
            this.listBoxMain.TabIndex = 36;
            // 
            // buttonItemUp
            // 
            this.buttonItemUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonItemUp.Location = new System.Drawing.Point(463, 249);
            this.buttonItemUp.Name = "buttonItemUp";
            this.buttonItemUp.Size = new System.Drawing.Size(68, 35);
            this.buttonItemUp.TabIndex = 37;
            this.buttonItemUp.UseVisualStyleBackColor = true;
            this.buttonItemUp.Click += new System.EventHandler(this.buttonItemUp_Click);
            // 
            // buttonItemDown
            // 
            this.buttonItemDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonItemDown.Location = new System.Drawing.Point(463, 290);
            this.buttonItemDown.Name = "buttonItemDown";
            this.buttonItemDown.Size = new System.Drawing.Size(68, 35);
            this.buttonItemDown.TabIndex = 38;
            this.buttonItemDown.UseVisualStyleBackColor = true;
            this.buttonItemDown.Click += new System.EventHandler(this.buttonItemDown_Click);
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripMain;
            this.notifyIconMain.Text = "notifyIconMain";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseDoubleClick);
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Size = new System.Drawing.Size(171, 124);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("setupToolStripMenuItem.Image")));
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(170, 38);
            this.setupToolStripMenuItem.Text = "Setings...";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(170, 38);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(167, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(170, 38);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // checkBoxUseVerticalBar
            // 
            this.checkBoxUseVerticalBar.AutoSize = true;
            this.checkBoxUseVerticalBar.Location = new System.Drawing.Point(13, 569);
            this.checkBoxUseVerticalBar.Name = "checkBoxUseVerticalBar";
            this.checkBoxUseVerticalBar.Size = new System.Drawing.Size(148, 24);
            this.checkBoxUseVerticalBar.TabIndex = 39;
            this.checkBoxUseVerticalBar.Text = "Use Vertical bar";
            this.checkBoxUseVerticalBar.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunAtStartup
            // 
            this.checkBoxRunAtStartup.AutoSize = true;
            this.checkBoxRunAtStartup.Location = new System.Drawing.Point(13, 603);
            this.checkBoxRunAtStartup.Name = "checkBoxRunAtStartup";
            this.checkBoxRunAtStartup.Size = new System.Drawing.Size(137, 24);
            this.checkBoxRunAtStartup.TabIndex = 40;
            this.checkBoxRunAtStartup.Text = "Run at startup";
            this.checkBoxRunAtStartup.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 713);
            this.Controls.Add(this.checkBoxRunAtStartup);
            this.Controls.Add(this.checkBoxUseVerticalBar);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelSeparationBar3);
            this.Controls.Add(this.buttonItemDown);
            this.Controls.Add(this.buttonItemUp);
            this.Controls.Add(this.listBoxMain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDangerColor);
            this.Controls.Add(this.buttonWarnColor);
            this.Controls.Add(this.buttonSafeColor);
            this.Controls.Add(this.trackBarDangerThreshold);
            this.Controls.Add(this.labelDangerTemperature);
            this.Controls.Add(this.trackBarWarningThreshold);
            this.Controls.Add(this.labelWarningTemperature);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewSensors);
            this.Controls.Add(this.labelSafeTemperature);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Temperature Icon Meter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWarningThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDangerThreshold)).EndInit();
            this.contextMenuStripMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView treeViewSensors;
		private System.Windows.Forms.Timer timerMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelSeparationBar3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelWarningTemperature;
		private System.Windows.Forms.TrackBar trackBarWarningThreshold;
		private System.Windows.Forms.TrackBar trackBarDangerThreshold;
		private System.Windows.Forms.Label labelDangerTemperature;
		private System.Windows.Forms.Label labelSafeTemperature;
		private System.Windows.Forms.Button buttonSafeColor;
		private System.Windows.Forms.Button buttonWarnColor;
		private System.Windows.Forms.Button buttonDangerColor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listBoxMain;
		private System.Windows.Forms.Button buttonItemUp;
		private System.Windows.Forms.Button buttonItemDown;
		private System.Windows.Forms.ColorDialog colorDialogMain;
		private System.Windows.Forms.NotifyIcon notifyIconMain;
		private System.Windows.Forms.CheckBox checkBoxUseVerticalBar;
		private System.Windows.Forms.CheckBox checkBoxRunAtStartup;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
		private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
	}
}

