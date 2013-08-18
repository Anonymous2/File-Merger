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
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxExtensions = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxExtensions = new System.Windows.Forms.TextBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtBoxDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxIncludeSubDirs = new System.Windows.Forms.CheckBox();
            this.checkBoxAllExtensions = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // checkBoxExtensions
            // 
            this.checkBoxExtensions.AutoSize = true;
            this.checkBoxExtensions.Checked = true;
            this.checkBoxExtensions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExtensions.Location = new System.Drawing.Point(17, 163);
            this.checkBoxExtensions.Name = "checkBoxExtensions";
            this.checkBoxExtensions.Size = new System.Drawing.Size(161, 17);
            this.checkBoxExtensions.TabIndex = 1;
            this.checkBoxExtensions.Text = "One output file per extension";
            this.checkBoxExtensions.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Extensions to merge (split by semicolon if more than one):";
            // 
            // txtBoxExtensions
            // 
            this.txtBoxExtensions.Location = new System.Drawing.Point(13, 114);
            this.txtBoxExtensions.Name = "txtBoxExtensions";
            this.txtBoxExtensions.Size = new System.Drawing.Size(272, 20);
            this.txtBoxExtensions.TabIndex = 3;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(209, 157);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 4;
            this.btnMerge.Text = "Merge!";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxDirectory
            // 
            this.txtBoxDirectory.Location = new System.Drawing.Point(13, 40);
            this.txtBoxDirectory.Name = "txtBoxDirectory";
            this.txtBoxDirectory.Size = new System.Drawing.Size(271, 20);
            this.txtBoxDirectory.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Directory to search in:";
            // 
            // checkBoxIncludeSubDirs
            // 
            this.checkBoxIncludeSubDirs.AutoSize = true;
            this.checkBoxIncludeSubDirs.Location = new System.Drawing.Point(17, 70);
            this.checkBoxIncludeSubDirs.Name = "checkBoxIncludeSubDirs";
            this.checkBoxIncludeSubDirs.Size = new System.Drawing.Size(129, 17);
            this.checkBoxIncludeSubDirs.TabIndex = 7;
            this.checkBoxIncludeSubDirs.Text = "Include subdirectories";
            this.checkBoxIncludeSubDirs.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllExtensions
            // 
            this.checkBoxAllExtensions.AutoSize = true;
            this.checkBoxAllExtensions.Location = new System.Drawing.Point(17, 140);
            this.checkBoxAllExtensions.Name = "checkBoxAllExtensions";
            this.checkBoxAllExtensions.Size = new System.Drawing.Size(90, 17);
            this.checkBoxAllExtensions.TabIndex = 8;
            this.checkBoxAllExtensions.Text = "All extensions";
            this.checkBoxAllExtensions.UseVisualStyleBackColor = true;
            this.checkBoxAllExtensions.CheckedChanged += new System.EventHandler(this.checkBoxAllExtensions_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 188);
            this.Controls.Add(this.checkBoxAllExtensions);
            this.Controls.Add(this.checkBoxIncludeSubDirs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxDirectory);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.txtBoxExtensions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxExtensions);
            this.Controls.Add(this.label1);
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

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxExtensions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoxExtensions;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.TextBox txtBoxDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxIncludeSubDirs;
        private System.Windows.Forms.CheckBox checkBoxAllExtensions;

    }
}

