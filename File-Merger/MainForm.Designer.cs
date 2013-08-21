namespace File_Merger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.checkBoxUniqueFilePerExt = new System.Windows.Forms.CheckBox();
            this.labelExtensionsToMerge = new System.Windows.Forms.Label();
            this.txtBoxExtensions = new System.Windows.Forms.TextBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtBoxDirectorySearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxIncludeSubDirs = new System.Windows.Forms.CheckBox();
            this.checkBoxAllExtensions = new System.Windows.Forms.CheckBox();
            this.btnSearchDirectory = new System.Windows.Forms.Button();
            this.txtBoxOutputDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxSyncDirFields = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSearchForOutput = new System.Windows.Forms.Button();
            this.checkBoxDeleteOutputFile = new System.Windows.Forms.CheckBox();
            this.txtBoxOutputFile = new System.Windows.Forms.TextBox();
            this.checkBoxFolderForMerged = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxUniqueFilePerExt
            // 
            this.checkBoxUniqueFilePerExt.AutoSize = true;
            this.checkBoxUniqueFilePerExt.Checked = true;
            this.checkBoxUniqueFilePerExt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUniqueFilePerExt.Location = new System.Drawing.Point(163, 165);
            this.checkBoxUniqueFilePerExt.Name = "checkBoxUniqueFilePerExt";
            this.checkBoxUniqueFilePerExt.Size = new System.Drawing.Size(161, 17);
            this.checkBoxUniqueFilePerExt.TabIndex = 1;
            this.checkBoxUniqueFilePerExt.Text = "One output file per extension";
            this.checkBoxUniqueFilePerExt.UseVisualStyleBackColor = true;
            // 
            // labelExtensionsToMerge
            // 
            this.labelExtensionsToMerge.AutoSize = true;
            this.labelExtensionsToMerge.Location = new System.Drawing.Point(11, 122);
            this.labelExtensionsToMerge.Name = "labelExtensionsToMerge";
            this.labelExtensionsToMerge.Size = new System.Drawing.Size(275, 13);
            this.labelExtensionsToMerge.TabIndex = 2;
            this.labelExtensionsToMerge.Text = "Extensions to merge (split by semicolon if more than one):";
            // 
            // txtBoxExtensions
            // 
            this.txtBoxExtensions.Location = new System.Drawing.Point(14, 139);
            this.txtBoxExtensions.Name = "txtBoxExtensions";
            this.txtBoxExtensions.Size = new System.Drawing.Size(315, 20);
            this.txtBoxExtensions.TabIndex = 3;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(254, 207);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 4;
            this.btnMerge.Text = "Merge!";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxDirectorySearch
            // 
            this.txtBoxDirectorySearch.Location = new System.Drawing.Point(14, 25);
            this.txtBoxDirectorySearch.Name = "txtBoxDirectorySearch";
            this.txtBoxDirectorySearch.Size = new System.Drawing.Size(294, 20);
            this.txtBoxDirectorySearch.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Directory to search in:";
            // 
            // checkBoxIncludeSubDirs
            // 
            this.checkBoxIncludeSubDirs.AutoSize = true;
            this.checkBoxIncludeSubDirs.Location = new System.Drawing.Point(18, 53);
            this.checkBoxIncludeSubDirs.Name = "checkBoxIncludeSubDirs";
            this.checkBoxIncludeSubDirs.Size = new System.Drawing.Size(129, 17);
            this.checkBoxIncludeSubDirs.TabIndex = 7;
            this.checkBoxIncludeSubDirs.Text = "Include subdirectories";
            this.checkBoxIncludeSubDirs.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllExtensions
            // 
            this.checkBoxAllExtensions.AutoSize = true;
            this.checkBoxAllExtensions.Location = new System.Drawing.Point(18, 165);
            this.checkBoxAllExtensions.Name = "checkBoxAllExtensions";
            this.checkBoxAllExtensions.Size = new System.Drawing.Size(90, 17);
            this.checkBoxAllExtensions.TabIndex = 8;
            this.checkBoxAllExtensions.Text = "All extensions";
            this.checkBoxAllExtensions.UseVisualStyleBackColor = true;
            this.checkBoxAllExtensions.CheckedChanged += new System.EventHandler(this.checkBoxAllExtensions_CheckedChanged);
            // 
            // btnSearchDirectory
            // 
            this.btnSearchDirectory.Location = new System.Drawing.Point(305, 25);
            this.btnSearchDirectory.Name = "btnSearchDirectory";
            this.btnSearchDirectory.Size = new System.Drawing.Size(24, 20);
            this.btnSearchDirectory.TabIndex = 9;
            this.btnSearchDirectory.Text = "...";
            this.btnSearchDirectory.UseVisualStyleBackColor = true;
            this.btnSearchDirectory.Click += new System.EventHandler(this.btnSearchDirectory_Click);
            // 
            // txtBoxOutputDir
            // 
            this.txtBoxOutputDir.Location = new System.Drawing.Point(14, 95);
            this.txtBoxOutputDir.Name = "txtBoxOutputDir";
            this.txtBoxOutputDir.Size = new System.Drawing.Size(220, 20);
            this.txtBoxOutputDir.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Directory to output in (may contain output file):";
            // 
            // checkBoxSyncDirFields
            // 
            this.checkBoxSyncDirFields.AutoSize = true;
            this.checkBoxSyncDirFields.Checked = true;
            this.checkBoxSyncDirFields.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSyncDirFields.Location = new System.Drawing.Point(163, 53);
            this.checkBoxSyncDirFields.Name = "checkBoxSyncDirFields";
            this.checkBoxSyncDirFields.Size = new System.Drawing.Size(154, 17);
            this.checkBoxSyncDirFields.TabIndex = 10;
            this.checkBoxSyncDirFields.Text = "Synchronize directory fields";
            this.checkBoxSyncDirFields.UseVisualStyleBackColor = true;
            this.checkBoxSyncDirFields.CheckedChanged += new System.EventHandler(this.checkBoxSyncDirFields_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "searchForFileDialog";
            // 
            // btnSearchForOutput
            // 
            this.btnSearchForOutput.Location = new System.Drawing.Point(305, 94);
            this.btnSearchForOutput.Name = "btnSearchForOutput";
            this.btnSearchForOutput.Size = new System.Drawing.Size(24, 20);
            this.btnSearchForOutput.TabIndex = 11;
            this.btnSearchForOutput.Text = "...";
            this.btnSearchForOutput.UseVisualStyleBackColor = true;
            this.btnSearchForOutput.Click += new System.EventHandler(this.btnSearchForOutput_Click);
            // 
            // checkBoxDeleteOutputFile
            // 
            this.checkBoxDeleteOutputFile.AutoSize = true;
            this.checkBoxDeleteOutputFile.Checked = true;
            this.checkBoxDeleteOutputFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteOutputFile.Enabled = false;
            this.checkBoxDeleteOutputFile.Location = new System.Drawing.Point(18, 211);
            this.checkBoxDeleteOutputFile.Name = "checkBoxDeleteOutputFile";
            this.checkBoxDeleteOutputFile.Size = new System.Drawing.Size(106, 17);
            this.checkBoxDeleteOutputFile.TabIndex = 12;
            this.checkBoxDeleteOutputFile.Text = "Delete output file";
            this.checkBoxDeleteOutputFile.UseVisualStyleBackColor = true;
            // 
            // txtBoxOutputFile
            // 
            this.txtBoxOutputFile.Location = new System.Drawing.Point(235, 95);
            this.txtBoxOutputFile.Name = "txtBoxOutputFile";
            this.txtBoxOutputFile.Size = new System.Drawing.Size(73, 20);
            this.txtBoxOutputFile.TabIndex = 13;
            this.txtBoxOutputFile.TextChanged += new System.EventHandler(this.txtBoxOutputFile_TextChanged);
            // 
            // checkBoxFolderForMerged
            // 
            this.checkBoxFolderForMerged.AutoSize = true;
            this.checkBoxFolderForMerged.Location = new System.Drawing.Point(17, 188);
            this.checkBoxFolderForMerged.Name = "checkBoxFolderForMerged";
            this.checkBoxFolderForMerged.Size = new System.Drawing.Size(137, 17);
            this.checkBoxFolderForMerged.TabIndex = 15;
            this.checkBoxFolderForMerged.Text = "Place files in new folder";
            this.checkBoxFolderForMerged.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 238);
            this.Controls.Add(this.checkBoxFolderForMerged);
            this.Controls.Add(this.btnSearchForOutput);
            this.Controls.Add(this.txtBoxOutputFile);
            this.Controls.Add(this.checkBoxDeleteOutputFile);
            this.Controls.Add(this.checkBoxSyncDirFields);
            this.Controls.Add(this.btnSearchDirectory);
            this.Controls.Add(this.checkBoxAllExtensions);
            this.Controls.Add(this.checkBoxIncludeSubDirs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxOutputDir);
            this.Controls.Add(this.txtBoxDirectorySearch);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.txtBoxExtensions);
            this.Controls.Add(this.labelExtensionsToMerge);
            this.Controls.Add(this.checkBoxUniqueFilePerExt);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "File-Merger";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxUniqueFilePerExt;
        private System.Windows.Forms.Label labelExtensionsToMerge;
        private System.Windows.Forms.TextBox txtBoxExtensions;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.TextBox txtBoxDirectorySearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxIncludeSubDirs;
        private System.Windows.Forms.CheckBox checkBoxAllExtensions;
        private System.Windows.Forms.Button btnSearchDirectory;
        private System.Windows.Forms.TextBox txtBoxOutputDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSyncDirFields;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnSearchForOutput;
        private System.Windows.Forms.CheckBox checkBoxDeleteOutputFile;
        private System.Windows.Forms.TextBox txtBoxOutputFile;
        private System.Windows.Forms.CheckBox checkBoxFolderForMerged;
    }
}
