/*
Copyright (c) 2017 Paul Amonson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.IO;
using System.Windows.Forms;

namespace LocalCopyBackup
{
    public partial class PlanEditor : Form
    {
        public PlanEditor()
        {
            InitializeComponent();
        }

        public string PlanName { get { return planNameTextBox_.Text; } set { planNameTextBox_.Text = value; } }
        public string SourceFolder { get { return sourceTextBox_.Text; } set { sourceTextBox_.Text = value; } }
        public string DestinationFolder { get { return destinationTextBox_.Text; } set { destinationTextBox_.Text = value; } }

        public bool EnableSource
        {
            set
            {
                sourceTextBox_.Enabled = value;
                button1.Enabled = value;
            }
        }

        public bool EnableDestination
        {
            set
            {
                destinationTextBox_.Enabled = value;
                button2.Enabled = value;
            }
        }

        private void SourceFolderButtonClick(object sender, EventArgs e)
        {
            folderBrowserDialog_.Description = "Source Folder to Backup";
            folderBrowserDialog_.ShowNewFolderButton = false;
            if (SourceFolder.Length > 0 && Directory.Exists(SourceFolder))
                folderBrowserDialog_.SelectedPath = SourceFolder;
            if (folderBrowserDialog_.ShowDialog() == DialogResult.OK)
                SourceFolder = folderBrowserDialog_.SelectedPath;
        }

        private void DestinationFolderButtonClick(object sender, EventArgs e)
        {
            folderBrowserDialog_.Description = "Destination Folder for the Backup";
            folderBrowserDialog_.ShowNewFolderButton = true;
            if (DestinationFolder.Length > 0 && Directory.Exists(DestinationFolder))
                folderBrowserDialog_.SelectedPath = DestinationFolder;
            if (folderBrowserDialog_.ShowDialog() == DialogResult.OK)
                DestinationFolder = folderBrowserDialog_.SelectedPath;
        }

        private void PlanNameTextBoxTextChanged(object sender, EventArgs e)
        {
            ChangeState();
        }

        private void SourceFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            ChangeState();
        }

        private void DestinationTextBoxTextChanged(object sender, EventArgs e)
        {
            ChangeState();
        }

        private void ChangeState()
        {
            if (PlanName.Length > 0 && SourceFolder.Length > 0 && DestinationFolder.Length > 0)
                if (Directory.Exists(SourceFolder) && Directory.Exists(DestinationFolder))
                    SetOK(true);
                else
                    SetOK(false);
            else
                SetOK(false);
        }

        private void SetOK(bool state)
        {
            if (button4.Enabled != state)
                button4.Enabled = state;
        }
    }
}
