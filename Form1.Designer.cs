namespace TechniqueSharingAsyncTask
{
    partial class Form1
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
			this.btnSyncWork = new System.Windows.Forms.Button();
			this.btnDoEventsWork = new System.Windows.Forms.Button();
			this.btnTaskWork = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnClickMe = new System.Windows.Forms.Button();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.chkGuardReentrancy = new System.Windows.Forms.CheckBox();
			this.btnLockCounter = new System.Windows.Forms.Button();
			this.chkUseLock = new System.Windows.Forms.CheckBox();
			this.btnVersionSaveA = new System.Windows.Forms.Button();
			this.btnVersionSaveB = new System.Windows.Forms.Button();
			this.txtEditorA = new System.Windows.Forms.TextBox();
			this.txtEditorB = new System.Windows.Forms.TextBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSyncWork
			// 
			this.btnSyncWork.AutoSize = true;
			this.btnSyncWork.Location = new System.Drawing.Point(12, 12);
			this.btnSyncWork.Name = "btnSyncWork";
			this.btnSyncWork.Size = new System.Drawing.Size(201, 32);
			this.btnSyncWork.TabIndex = 0;
			this.btnSyncWork.Text = "Sync Work (UI Freeze)";
			this.btnSyncWork.UseVisualStyleBackColor = true;
			this.btnSyncWork.Click += new System.EventHandler(this.btnSyncWork_Click);
			// 
			// btnDoEventsWork
			// 
			this.btnDoEventsWork.AutoSize = true;
			this.btnDoEventsWork.Location = new System.Drawing.Point(310, 12);
			this.btnDoEventsWork.Name = "btnDoEventsWork";
			this.btnDoEventsWork.Size = new System.Drawing.Size(309, 32);
			this.btnDoEventsWork.TabIndex = 1;
			this.btnDoEventsWork.Text = "DoEvents Work (Looks Responsive)";
			this.btnDoEventsWork.UseVisualStyleBackColor = true;
			this.btnDoEventsWork.Click += new System.EventHandler(this.btnDoEventsWork_Click);
			// 
			// btnTaskWork
			// 
			this.btnTaskWork.AutoSize = true;
			this.btnTaskWork.Location = new System.Drawing.Point(640, 12);
			this.btnTaskWork.Name = "btnTaskWork";
			this.btnTaskWork.Size = new System.Drawing.Size(298, 32);
			this.btnTaskWork.TabIndex = 2;
			this.btnTaskWork.Text = "Task/Async Work (Recommended)";
			this.btnTaskWork.UseVisualStyleBackColor = true;
			this.btnTaskWork.Click += new System.EventHandler(this.btnTaskWork_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AutoSize = true;
			this.btnCancel.Location = new System.Drawing.Point(1005, 12);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 32);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnClickMe
			// 
			this.btnClickMe.AutoSize = true;
			this.btnClickMe.Location = new System.Drawing.Point(1138, 12);
			this.btnClickMe.Name = "btnClickMe";
			this.btnClickMe.Size = new System.Drawing.Size(205, 32);
			this.btnClickMe.TabIndex = 4;
			this.btnClickMe.Text = "Click Me (UI still alive?)";
			this.btnClickMe.UseVisualStyleBackColor = true;
			this.btnClickMe.Click += new System.EventHandler(this.btnClickMe_Click);
			// 
			// lstLog
			// 
			this.lstLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstLog.Font = new System.Drawing.Font("Consolas", 9.5F);
			this.lstLog.FormattingEnabled = true;
			this.lstLog.ItemHeight = 22;
			this.lstLog.Location = new System.Drawing.Point(12, 90);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(1344, 488);
			this.lstLog.TabIndex = 8;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgress});
			this.statusStrip1.Location = new System.Drawing.Point(0, 669);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1378, 31);
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(64, 24);
			this.statusLabel.Text = "Ready";
			// 
			// statusProgress
			// 
			this.statusProgress.Name = "statusProgress";
			this.statusProgress.Size = new System.Drawing.Size(400, 23);
			this.statusProgress.Step = 1;
			// 
			// chkGuardReentrancy
			// 
			this.chkGuardReentrancy.AutoSize = true;
			this.chkGuardReentrancy.Location = new System.Drawing.Point(12, 54);
			this.chkGuardReentrancy.Name = "chkGuardReentrancy";
			this.chkGuardReentrancy.Size = new System.Drawing.Size(259, 26);
			this.chkGuardReentrancy.TabIndex = 5;
			this.chkGuardReentrancy.Text = "Guard DoEvents reentrancy";
			this.chkGuardReentrancy.UseVisualStyleBackColor = true;
			// 
			// btnLockCounter
			// 
			this.btnLockCounter.AutoSize = true;
			this.btnLockCounter.Location = new System.Drawing.Point(310, 50);
			this.btnLockCounter.Name = "btnLockCounter";
			this.btnLockCounter.Size = new System.Drawing.Size(226, 32);
			this.btnLockCounter.TabIndex = 6;
			this.btnLockCounter.Text = "Counter with/without Lock";
			this.btnLockCounter.UseVisualStyleBackColor = true;
			this.btnLockCounter.Click += new System.EventHandler(this.btnLockCounter_Click);
			// 
			// chkUseLock
			// 
			this.chkUseLock.AutoSize = true;
			this.chkUseLock.Location = new System.Drawing.Point(640, 58);
			this.chkUseLock.Name = "chkUseLock";
			this.chkUseLock.Size = new System.Drawing.Size(288, 26);
			this.chkUseLock.TabIndex = 7;
			this.chkUseLock.Text = "Use lock/Interlocked for counter";
			this.chkUseLock.UseVisualStyleBackColor = true;
			// 
			// btnVersionSaveA
			// 
			this.btnVersionSaveA.AutoSize = true;
			this.btnVersionSaveA.Location = new System.Drawing.Point(1005, 54);
			this.btnVersionSaveA.Name = "btnVersionSaveA";
			this.btnVersionSaveA.Size = new System.Drawing.Size(110, 32);
			this.btnVersionSaveA.TabIndex = 10;
			this.btnVersionSaveA.Text = "Save A";
			this.btnVersionSaveA.UseVisualStyleBackColor = true;
			this.btnVersionSaveA.Click += new System.EventHandler(this.btnVersionSaveA_Click);
			// 
			// btnVersionSaveB
			// 
			this.btnVersionSaveB.AutoSize = true;
			this.btnVersionSaveB.Location = new System.Drawing.Point(1138, 54);
			this.btnVersionSaveB.Name = "btnVersionSaveB";
			this.btnVersionSaveB.Size = new System.Drawing.Size(117, 32);
			this.btnVersionSaveB.TabIndex = 11;
			this.btnVersionSaveB.Text = "Save B";
			this.btnVersionSaveB.UseVisualStyleBackColor = true;
			this.btnVersionSaveB.Click += new System.EventHandler(this.btnVersionSaveB_Click);
			// 
			// txtEditorA
			// 
			this.txtEditorA.Location = new System.Drawing.Point(12, 610);
			this.txtEditorA.Name = "txtEditorA";
			this.txtEditorA.Size = new System.Drawing.Size(400, 28);
			this.txtEditorA.TabIndex = 12;
			this.txtEditorA.Text = "Initial content";
			// 
			// txtEditorB
			// 
			this.txtEditorB.Location = new System.Drawing.Point(418, 610);
			this.txtEditorB.Name = "txtEditorB";
			this.txtEditorB.Size = new System.Drawing.Size(400, 28);
			this.txtEditorB.TabIndex = 13;
			this.txtEditorB.Text = "Initial content";
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(824, 613);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(91, 22);
			this.lblVersion.TabIndex = 14;
			this.lblVersion.Text = "Version: 1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1378, 700);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.txtEditorB);
			this.Controls.Add(this.txtEditorA);
			this.Controls.Add(this.btnVersionSaveB);
			this.Controls.Add(this.btnVersionSaveA);
			this.Controls.Add(this.chkUseLock);
			this.Controls.Add(this.btnLockCounter);
			this.Controls.Add(this.chkGuardReentrancy);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.btnClickMe);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnTaskWork);
			this.Controls.Add(this.btnDoEventsWork);
			this.Controls.Add(this.btnSyncWork);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.Name = "Form1";
			this.Text = "Async / DoEvents / Lock / Version Demo";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSyncWork;
        private System.Windows.Forms.Button btnDoEventsWork;
        private System.Windows.Forms.Button btnTaskWork;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClickMe;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.CheckBox chkGuardReentrancy;
        private System.Windows.Forms.Button btnLockCounter;
        private System.Windows.Forms.CheckBox chkUseLock;
        private System.Windows.Forms.Button btnVersionSaveA;
        private System.Windows.Forms.Button btnVersionSaveB;
        private System.Windows.Forms.TextBox txtEditorA;
        private System.Windows.Forms.TextBox txtEditorB;
        private System.Windows.Forms.Label lblVersion;
	}
}
