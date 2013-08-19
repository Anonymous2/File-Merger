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
            this.checkBoxExtensions = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxExtensions = new System.Windows.Forms.TextBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtBoxDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxIncludeSubDirs = new System.Windows.Forms.CheckBox();
            this.checkBoxAllExtensions = new System.Windows.Forms.CheckBox();
            this.btnSearchDirectory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxExtensions
            // 
            this.checkBoxExtensions.AutoSize = true;
            this.checkBoxExtensions.Checked = true;
            this.checkBoxExtensions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExtensions.Location = new System.Drawing.Point(18, 140);
            this.checkBoxExtensions.Name = "checkBoxExtensions";
            this.checkBoxExtensions.Size = new System.Drawing.Size(161, 17);
            this.checkBoxExtensions.TabIndex = 1;
            this.checkBoxExtensions.Text = "One output file per extension";
            this.checkBoxExtensions.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Extensions to merge (split by semicolon if more than one):";
            // 
            // txtBoxExtensions
            // 
            this.txtBoxExtensions.Location = new System.Drawing.Point(14, 91);
            this.txtBoxExtensions.Name = "txtBoxExtensions";
            this.txtBoxExtensions.Size = new System.Drawing.Size(272, 20);
            this.txtBoxExtensions.TabIndex = 3;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(210, 134);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 4;
            this.btnMerge.Text = "Merge!";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxDirectory
            // 
            this.txtBoxDirectory.Location = new System.Drawing.Point(14, 25);
            this.txtBoxDirectory.Name = "txtBoxDirectory";
            this.txtBoxDirectory.Size = new System.Drawing.Size(271, 20);
            this.txtBoxDirectory.TabIndex = 5;
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
            this.checkBoxIncludeSubDirs.Location = new System.Drawing.Point(18, 50);
            this.checkBoxIncludeSubDirs.Name = "checkBoxIncludeSubDirs";
            this.checkBoxIncludeSubDirs.Size = new System.Drawing.Size(129, 17);
            this.checkBoxIncludeSubDirs.TabIndex = 7;
            this.checkBoxIncludeSubDirs.Text = "Include subdirectories";
            this.checkBoxIncludeSubDirs.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllExtensions
            // 
            this.checkBoxAllExtensions.AutoSize = true;
            this.checkBoxAllExtensions.Location = new System.Drawing.Point(18, 117);
            this.checkBoxAllExtensions.Name = "checkBoxAllExtensions";
            this.checkBoxAllExtensions.Size = new System.Drawing.Size(90, 17);
            this.checkBoxAllExtensions.TabIndex = 8;
            this.checkBoxAllExtensions.Text = "All extensions";
            this.checkBoxAllExtensions.UseVisualStyleBackColor = true;
            this.checkBoxAllExtensions.CheckedChanged += new System.EventHandler(this.checkBoxAllExtensions_CheckedChanged);
            // 
            // btnSearchDirectory
            // 
            this.btnSearchDirectory.Location = new System.Drawing.Point(261, 25);
            this.btnSearchDirectory.Name = "btnSearchDirectory";
            this.btnSearchDirectory.Size = new System.Drawing.Size(24, 20);
            this.btnSearchDirectory.TabIndex = 9;
            this.btnSearchDirectory.Text = "...";
            this.btnSearchDirectory.UseVisualStyleBackColor = true;
            this.btnSearchDirectory.Click += new System.EventHandler(this.btnSearchDirectory_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 165);
            this.Controls.Add(this.btnSearchDirectory);
            this.Controls.Add(this.checkBoxAllExtensions);
            this.Controls.Add(this.checkBoxIncludeSubDirs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxDirectory);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.txtBoxExtensions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxExtensions);
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

        private System.Windows.Forms.CheckBox checkBoxExtensions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoxExtensions;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.TextBox txtBoxDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxIncludeSubDirs;
        private System.Windows.Forms.CheckBox checkBoxAllExtensions;
        private System.Windows.Forms.Button btnSearchDirectory;

    }
}

