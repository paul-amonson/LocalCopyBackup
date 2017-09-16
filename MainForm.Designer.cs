namespace LocalCopyBackup
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.planListView_ = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logRichTextBox_ = new System.Windows.Forms.RichTextBox();
            this.toolStrip_ = new System.Windows.Forms.ToolStrip();
            this.addPlanTButton_ = new System.Windows.Forms.ToolStripButton();
            this.editPlanButton_ = new System.Windows.Forms.ToolStripButton();
            this.deletePlanButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.runNowToolStripButton_ = new System.Windows.Forms.ToolStripButton();
            this.stopRunToolStripButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.scheduleToolStripButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreToolButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolButton_ = new System.Windows.Forms.ToolStripButton();
            this.exportTButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolButton_ = new System.Windows.Forms.ToolStripButton();
            this.timer_ = new System.Windows.Forms.Timer(this.components);
            this.trayIcon_ = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu_ = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayRunAllMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            this.trayStopRunMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.trayExitMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip_.SuspendLayout();
            this.trayMenu_.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.planListView_);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logRichTextBox_);
            this.splitContainer1.Size = new System.Drawing.Size(916, 526);
            this.splitContainer1.SplitterDistance = 197;
            this.splitContainer1.TabIndex = 0;
            // 
            // planListView_
            // 
            this.planListView_.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.planListView_.BackColor = System.Drawing.Color.Azure;
            this.planListView_.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.planListView_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.planListView_.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.planListView_.FullRowSelect = true;
            this.planListView_.GridLines = true;
            this.planListView_.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.planListView_.HideSelection = false;
            this.planListView_.LabelWrap = false;
            this.planListView_.Location = new System.Drawing.Point(0, 0);
            this.planListView_.MinimumSize = new System.Drawing.Size(916, 197);
            this.planListView_.MultiSelect = false;
            this.planListView_.Name = "planListView_";
            this.planListView_.ShowGroups = false;
            this.planListView_.Size = new System.Drawing.Size(916, 197);
            this.planListView_.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.planListView_.TabIndex = 0;
            this.planListView_.UseCompatibleStateImageBehavior = false;
            this.planListView_.View = System.Windows.Forms.View.Details;
            this.planListView_.SelectedIndexChanged += new System.EventHandler(this.PlanListViewSelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Backup Plan Name";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Source Folder";
            this.columnHeader2.Width = 300;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Destination Folder";
            this.columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Last Run Status";
            this.columnHeader4.Width = 120;
            // 
            // logRichTextBox_
            // 
            this.logRichTextBox_.DetectUrls = false;
            this.logRichTextBox_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logRichTextBox_.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logRichTextBox_.HideSelection = false;
            this.logRichTextBox_.Location = new System.Drawing.Point(0, 0);
            this.logRichTextBox_.Name = "logRichTextBox_";
            this.logRichTextBox_.ReadOnly = true;
            this.logRichTextBox_.Size = new System.Drawing.Size(916, 325);
            this.logRichTextBox_.TabIndex = 0;
            this.logRichTextBox_.Text = "";
            // 
            // toolStrip_
            // 
            this.toolStrip_.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPlanTButton_,
            this.editPlanButton_,
            this.deletePlanButton_,
            this.toolStripSeparator1,
            this.runNowToolStripButton_,
            this.stopRunToolStripButton_,
            this.toolStripSeparator2,
            this.scheduleToolStripButton_,
            this.toolStripSeparator3,
            this.restoreToolButton_,
            this.toolStripSeparator4,
            this.importToolButton_,
            this.exportTButton_,
            this.toolStripSeparator5,
            this.exitToolButton_});
            this.toolStrip_.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_.Name = "toolStrip_";
            this.toolStrip_.Size = new System.Drawing.Size(916, 25);
            this.toolStrip_.TabIndex = 2;
            this.toolStrip_.Text = "toolStrip1";
            // 
            // addPlanTButton_
            // 
            this.addPlanTButton_.Image = ((System.Drawing.Image)(resources.GetObject("addPlanTButton_.Image")));
            this.addPlanTButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPlanTButton_.Name = "addPlanTButton_";
            this.addPlanTButton_.Size = new System.Drawing.Size(75, 22);
            this.addPlanTButton_.Text = "Add Plan";
            this.addPlanTButton_.Click += new System.EventHandler(this.AddPlanTButtonClick);
            // 
            // editPlanButton_
            // 
            this.editPlanButton_.Enabled = false;
            this.editPlanButton_.Image = ((System.Drawing.Image)(resources.GetObject("editPlanButton_.Image")));
            this.editPlanButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editPlanButton_.Name = "editPlanButton_";
            this.editPlanButton_.Size = new System.Drawing.Size(73, 22);
            this.editPlanButton_.Text = "Edit Plan";
            this.editPlanButton_.Click += new System.EventHandler(this.EditPlanButtonClick);
            // 
            // deletePlanButton_
            // 
            this.deletePlanButton_.Enabled = false;
            this.deletePlanButton_.Image = ((System.Drawing.Image)(resources.GetObject("deletePlanButton_.Image")));
            this.deletePlanButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deletePlanButton_.Name = "deletePlanButton_";
            this.deletePlanButton_.Size = new System.Drawing.Size(86, 22);
            this.deletePlanButton_.Text = "Delete Plan";
            this.deletePlanButton_.Click += new System.EventHandler(this.DeletePlanButtonClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // runNowToolStripButton_
            // 
            this.runNowToolStripButton_.Enabled = false;
            this.runNowToolStripButton_.Image = ((System.Drawing.Image)(resources.GetObject("runNowToolStripButton_.Image")));
            this.runNowToolStripButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runNowToolStripButton_.Name = "runNowToolStripButton_";
            this.runNowToolStripButton_.Size = new System.Drawing.Size(93, 22);
            this.runNowToolStripButton_.Text = "Run All Now";
            this.runNowToolStripButton_.Click += new System.EventHandler(this.RunNowButtonClick);
            // 
            // stopRunToolStripButton_
            // 
            this.stopRunToolStripButton_.Enabled = false;
            this.stopRunToolStripButton_.Image = ((System.Drawing.Image)(resources.GetObject("stopRunToolStripButton_.Image")));
            this.stopRunToolStripButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopRunToolStripButton_.Name = "stopRunToolStripButton_";
            this.stopRunToolStripButton_.Size = new System.Drawing.Size(75, 22);
            this.stopRunToolStripButton_.Text = "Stop Run";
            this.stopRunToolStripButton_.Click += new System.EventHandler(this.StopRunButtonClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // scheduleToolStripButton_
            // 
            this.scheduleToolStripButton_.Enabled = false;
            this.scheduleToolStripButton_.Image = ((System.Drawing.Image)(resources.GetObject("scheduleToolStripButton_.Image")));
            this.scheduleToolStripButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scheduleToolStripButton_.Name = "scheduleToolStripButton_";
            this.scheduleToolStripButton_.Size = new System.Drawing.Size(148, 22);
            this.scheduleToolStripButton_.Text = "Enable 4 Hour Backups";
            this.scheduleToolStripButton_.Click += new System.EventHandler(this.ScheduleButtonClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // restoreToolButton_
            // 
            this.restoreToolButton_.Enabled = false;
            this.restoreToolButton_.Image = ((System.Drawing.Image)(resources.GetObject("restoreToolButton_.Image")));
            this.restoreToolButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.restoreToolButton_.Name = "restoreToolButton_";
            this.restoreToolButton_.Size = new System.Drawing.Size(66, 22);
            this.restoreToolButton_.Text = "Restore";
            this.restoreToolButton_.Click += new System.EventHandler(this.RestoreToolButtonClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // importToolButton_
            // 
            this.importToolButton_.Image = ((System.Drawing.Image)(resources.GetObject("importToolButton_.Image")));
            this.importToolButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importToolButton_.Name = "importToolButton_";
            this.importToolButton_.Size = new System.Drawing.Size(105, 22);
            this.importToolButton_.Text = "Import Backup";
            this.importToolButton_.Click += new System.EventHandler(this.ImportToolButtonClick);
            // 
            // exportTButton_
            // 
            this.exportTButton_.Enabled = false;
            this.exportTButton_.Image = ((System.Drawing.Image)(resources.GetObject("exportTButton_.Image")));
            this.exportTButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportTButton_.Name = "exportTButton_";
            this.exportTButton_.Size = new System.Drawing.Size(82, 22);
            this.exportTButton_.Text = "Export Key";
            this.exportTButton_.Click += new System.EventHandler(this.ExportTButtonClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // exitToolButton_
            // 
            this.exitToolButton_.Image = ((System.Drawing.Image)(resources.GetObject("exitToolButton_.Image")));
            this.exitToolButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitToolButton_.Name = "exitToolButton_";
            this.exitToolButton_.Size = new System.Drawing.Size(45, 22);
            this.exitToolButton_.Text = "Exit";
            this.exitToolButton_.Click += new System.EventHandler(this.ExitToolButtonClick);
            // 
            // timer_
            // 
            this.timer_.Enabled = true;
            this.timer_.Interval = 60000;
            this.timer_.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // trayIcon_
            // 
            this.trayIcon_.ContextMenuStrip = this.trayMenu_;
            this.trayIcon_.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon_.Icon")));
            this.trayIcon_.Text = "Local Backup Copy";
            this.trayIcon_.Visible = true;
            this.trayIcon_.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIconMouseDoubleClick);
            // 
            // trayMenu_
            // 
            this.trayMenu_.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayRunAllMenuItem_,
            this.trayStopRunMenuItem_,
            this.toolStripMenuItem2,
            this.trayExitMenuItem_});
            this.trayMenu_.Name = "trayMenu_";
            this.trayMenu_.Size = new System.Drawing.Size(123, 76);
            // 
            // trayRunAllMenuItem_
            // 
            this.trayRunAllMenuItem_.Name = "trayRunAllMenuItem_";
            this.trayRunAllMenuItem_.Size = new System.Drawing.Size(122, 22);
            this.trayRunAllMenuItem_.Text = "Run All";
            this.trayRunAllMenuItem_.Click += new System.EventHandler(this.TrayRunAllMenuItemClick);
            // 
            // trayStopRunMenuItem_
            // 
            this.trayStopRunMenuItem_.Name = "trayStopRunMenuItem_";
            this.trayStopRunMenuItem_.Size = new System.Drawing.Size(122, 22);
            this.trayStopRunMenuItem_.Text = "Stop Run";
            this.trayStopRunMenuItem_.Click += new System.EventHandler(this.TrayStopRunMenuItemClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(119, 6);
            // 
            // trayExitMenuItem_
            // 
            this.trayExitMenuItem_.Name = "trayExitMenuItem_";
            this.trayExitMenuItem_.Size = new System.Drawing.Size(122, 22);
            this.trayExitMenuItem_.Text = "Exit";
            this.trayExitMenuItem_.Click += new System.EventHandler(this.TrayExitMenuItemClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 551);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip_);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(932, 590);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Local Copy Backup";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_Closed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip_.ResumeLayout(false);
            this.toolStrip_.PerformLayout();
            this.trayMenu_.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView planListView_;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.RichTextBox logRichTextBox_;
        private System.Windows.Forms.ToolStrip toolStrip_;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripButton addPlanTButton_;
        private System.Windows.Forms.ToolStripButton editPlanButton_;
        private System.Windows.Forms.ToolStripButton deletePlanButton_;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton runNowToolStripButton_;
        private System.Windows.Forms.ToolStripButton stopRunToolStripButton_;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton scheduleToolStripButton_;
        private System.Windows.Forms.Timer timer_;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton restoreToolButton_;
        private System.Windows.Forms.ToolStripButton importToolButton_;
        private System.Windows.Forms.ToolStripButton exportTButton_;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.NotifyIcon trayIcon_;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton exitToolButton_;
        private System.Windows.Forms.ContextMenuStrip trayMenu_;
        private System.Windows.Forms.ToolStripMenuItem trayRunAllMenuItem_;
        private System.Windows.Forms.ToolStripMenuItem trayStopRunMenuItem_;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem trayExitMenuItem_;
    }
}

