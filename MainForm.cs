// Copyright (c) 2017-2018, Paul Amonson
// SPDX-License-Identifier: MIT

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Backup;
using BackupPlan;

namespace LocalCopyBackup
{
    public partial class MainForm : Form
    {
        private delegate void BoolDelegate(bool state);
        private delegate void PlanDelegate(BackupPlan.BackupPlan plan, bool result);
        private delegate void FormDelegate(object sender, EventArgs e);

        public MainForm()
        {
            if (!Directory.Exists(GetSettingsFolder()))
                Directory.CreateDirectory(GetSettingsFolder());
            config_ = new Config(Path.Combine(GetSettingsFolder(), "config"));
            InitializeComponent();
        }

        public void AddLogEntry(string text)
        {
            if (InvokeRequired)
                Invoke(new LogCallback(AddLogEntry), text);
            else
                logRichTextBox_.AppendText(text + "\n");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            scheduleToolStripButton_.Checked = config_.TimedEnabled;
            LoadPlans();
            font_ = new System.Drawing.Font(planListView_.Font, System.Drawing.FontStyle.Bold);
            InitListView();
            RunState(false);
            PlanListViewSelectedIndexChanged(null, null);
        }

        private void MainForm_Closed(object sender, FormClosedEventArgs e)
        {
            SavePlans();
        }

        private void PlanListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (planListView_ == null || planListView_.SelectedItems.Count == 0 || currentRunningPlan_ != null)
            {
                editPlanButton_.Enabled = false;
                deletePlanButton_.Enabled = false;
                restoreToolButton_.Enabled = false;
                exportTButton_.Enabled = false;
            }
            else
            {
                
                editPlanButton_.Enabled = true;
                deletePlanButton_.Enabled = true;
                restoreToolButton_.Enabled = true;
                exportTButton_.Enabled = true;
            }
        }

        private void AddPlanTButtonClick(object sender, EventArgs e)
        {
            var dlg = new PlanEditor {
                Text = "Add New Backup Plan",
                PlanName = "Untitled Plan"
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var plan = new BackupPlan.BackupPlan(dlg.PlanName, dlg.SourceFolder, dlg.DestinationFolder, null, AddLogEntry);
                AddPlan(plan);
            }
        }

        private void EditPlanButtonClick(object sender, EventArgs e)
        {
            BackupPlan.BackupPlan plan = (BackupPlan.BackupPlan)planListView_.SelectedItems[0].Tag;
            var dlg = new PlanEditor {
                Text = "Edit Backup Plan",
                PlanName = plan.Name,
                EnableSource = false,
                EnableDestination = false,
                SourceFolder = plan.FolderToBackup,
                DestinationFolder = plan.DestinationFolder
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                plan.Name = dlg.Name;
                SavePlans();
            }
        }

        private void DeletePlanButtonClick(object sender, EventArgs e)
        {
            var item = planListView_.SelectedItems[0];
            var plan = (BackupPlan.BackupPlan)item.Tag;
            plans_.Remove(plan);
            planListView_.Items.Remove(item);
            runNowToolStripButton_.Enabled = (plans_.Count > 0);
        }

        private void AddPlan(BackupPlan.BackupPlan plan)
        {
            var item = new ListViewItem(plan.Name);
            var subitems = new ListViewItem.ListViewSubItemCollection(item) {
                plan.FolderToBackup,
                plan.DestinationFolder,
                "Never Run"
            };
            item.Tag = plan;
            planListView_.Items.Add(item);
            if(!plans_.Contains(plan))
                plans_.Add(plan);
            runNowToolStripButton_.Enabled = (plans_.Count > 0);
        }

        private void UpdateItem(ListViewItem item)
        {
            BackupPlan.BackupPlan plan = (BackupPlan.BackupPlan)item.Tag;
            item.Text = plan.Name;
            item.SubItems[0].Text = plan.FolderToBackup;
            item.SubItems[1].Text = plan.DestinationFolder;
            item.SubItems[2].Text = "Never Run Since Edit";
        }

        private string GetSettingsFolder()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Replace("Desktop", "");
            return home + ".local_backup";
        }

        private void LoadPlans()
        {
            var settingsFolder = GetSettingsFolder();
            var plansFile = Path.Combine(settingsFolder, "plans_file");
            if (!File.Exists(plansFile))
                plans_.Save(plansFile);
            else
                plans_ = BackupPlanList.Load(plansFile, AddLogEntry);
        }

        private void SavePlans()
        {
            var settingsFolder = GetSettingsFolder();
            var plansFile = Path.Combine(settingsFolder, "plans_file");
            plans_.Save(plansFile);
        }

        private void InitListView()
        {
            foreach (var plan in plans_)
                AddPlan(plan);
            runNowToolStripButton_.Enabled = (plans_.Count > 0);
        }

        private ListViewItem LookupItem(BackupPlan.BackupPlan plan)
        {
            foreach (ListViewItem item in planListView_.Items)
                if (item.Tag as BackupPlan.BackupPlan == plan)
                    return item;
            return null;
        }
    
        private void RunNowButtonClick(object sender, EventArgs e)
        {
            runTask_ = Task.Run(() => {
                isRunning_ = true;
                haltRun_ = false;
                Invoke(new BoolDelegate(RunState), true);
                var start = DateTime.Now;
                AddLogEntry(string.Format("*** Backup Started on {0}...", start.ToString("G")));
                foreach (var plan in plans_)
                {
                    currentRunningPlan_ = plan;
                    Invoke(new FormDelegate(PlanListViewSelectedIndexChanged), null, null);
                    AddLogEntry(string.Format("*** Indexing {0}...", plan.FolderToBackup));
                    var result = plan.RunBackup();
                    Invoke(new PlanDelegate(SetRunResult), plan, result);
                    if (haltRun_)
                        break;
                }
                var diff = DateTime.Now - start;
                AddLogEntry(string.Format("*** Backup Finished after on {0}.", diff.ToString(@"hh\:mm\:ss")));
                currentRunningPlan_ = null;
                Invoke(new BoolDelegate(RunState), false);
                Invoke(new FormDelegate(PlanListViewSelectedIndexChanged), null, null);
                isRunning_ = false;
            });
        }

        private void SetRunResult(BackupPlan.BackupPlan plan, bool result)
        {
            var subitem = LookupItem(plan).SubItems[3];
            subitem.Text = result ? "Success" : "Failure";
            subitem.ForeColor = result ? System.Drawing.Color.DarkGreen : System.Drawing.Color.DarkRed;
        }

        private void RunState(bool state)
        {
            runNowToolStripButton_.Enabled = !state;
            stopRunToolStripButton_.Enabled = state;
            scheduleToolStripButton_.Enabled = !state;
            var text = scheduleToolStripButton_.Checked ? "Disable" : "Enable";
            scheduleToolStripButton_.Text = string.Format("{0} {1} Hour Backups", text, config_.Hour);
        }

        private void StopRunButtonClick(object sender, EventArgs e)
        {
            if (runTask_ != null && currentRunningPlan_ != null)
            {
                haltRun_ = true;
                currentRunningPlan_.HaltRun();
            }
        }

        private void ScheduleButtonClick(object sender, EventArgs e)
        {
            scheduleToolStripButton_.Checked = !scheduleToolStripButton_.Checked;
            config_.TimedEnabled = scheduleToolStripButton_.Checked;
            config_.Save();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            if (scheduleToolStripButton_.Checked && now.Minute == 0 && !isRunning_ && (now.Hour % config_.Hour) == 0)
            {
                AddLogEntry("*** Automatic Backup Starting...");
                RunNowButtonClick(null, null);
            }
        }

        private BackupPlan.BackupPlan GetSelectedPlan()
        {
            if(planListView_.SelectedItems != null && planListView_.SelectedItems.Count == 1)
                return (BackupPlan.BackupPlan)planListView_.SelectedItems[0].Tag;
            return null;
        }

        private void RestoreToolButtonClick(object sender, EventArgs e)
        {
            var plan = GetSelectedPlan();
            FolderBrowserDialog dlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select Folder to Put Restored Files",
                SelectedPath = plan.FolderToBackup
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                runTask_ = Task.Run(() =>
                {
                    currentRunningPlan_ = plan;
                    Invoke(new FormDelegate(PlanListViewSelectedIndexChanged), null, null);
                    plan.RunRestore(dlg.SelectedPath);
                    currentRunningPlan_ = null;
                    Invoke(new FormDelegate(PlanListViewSelectedIndexChanged), null, null);
                });
        }

        private void ExportTButtonClick(object sender, EventArgs e)
        {
            var plan = GetSelectedPlan();
            var dlg = new SaveFileDialog
            {
                FileName = plan.Name + ".key",
                DefaultExt = "key",
                Filter = "Key files (*.key)|*.key|All files (*.*)|*.*",
                CheckPathExists = true,
                InitialDirectory = GetSettingsFolder(),
                Title = "Save Encryption Key for the Plan: " + plan.Name,
                ValidateNames = true
            };
            if(dlg.ShowDialog(this) == DialogResult.OK)
                plan.Key.Save(dlg.FileName);
        }

        private void ImportToolButtonClick(object sender, EventArgs e)
        {
            KeyData key = null;
            string indexFileName = null;
            var dlg = new OpenFileDialog
            {
                Title = "Import Backup Index File...",
                DefaultExt = "index",
                Filter = "Backup Index files (*.index)|*.index|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false,
                ValidateNames = true
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                indexFileName = dlg.FileName;
            else
                return;
            dlg = new OpenFileDialog
            {
                Title = "Import Key This Backup (Wrong key will not produce a good restore!)",
                DefaultExt = "key",
                Filter = "Key files (*.key)|*.key|All files (*.*)|*.*",
                InitialDirectory = GetSettingsFolder(),
                CheckFileExists = true,
                Multiselect = false,
                ValidateNames = true
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                key = KeyData.Load(dlg.FileName);
            else
                return;
            var indexName = Path.GetFileNameWithoutExtension(indexFileName);
            var plan = BackupPlan.BackupPlan.ImportFromIndexFile(key, indexFileName, AddLogEntry);
            AddPlan(plan);
        }

        private void TrayRunAllMenuItemClick(object sender, EventArgs e)
        {
            RunNowButtonClick(sender, e);
        }

        private void TrayStopRunMenuItemClick(object sender, EventArgs e)
        {
            StopRunButtonClick(sender, e);
        }

        private void TrayExitMenuItemClick(object sender, EventArgs e)
        {
            ExitToolButtonClick(sender, e);
        }

        private void ExitToolButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void TrayIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Minimized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
                ShowInTaskbar = false;
            else if (FormWindowState.Normal == this.WindowState)
                ShowInTaskbar = true;
        }

        private BackupPlanList plans_ = new BackupPlanList();
        private Task runTask_ = null;
        private BackupPlan.BackupPlan currentRunningPlan_ = null;
        private bool haltRun_ = false;
        private bool isRunning_ = false;
        private System.Drawing.Font font_;
        private Config config_;
    }
}
