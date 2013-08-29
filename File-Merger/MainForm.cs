using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Timers;

namespace File_Merger
{
    public partial class MainForm : Form
    {
        private bool syncrhonizeDirFields = true;
        private int promptAdminOutcome = 0;
        private Thread mergeThread = null;
        private System.Windows.Forms.Timer timerCollapseProgress = null;
        private int originalHeight;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //promptAdminOutcome = Prompt.ShowDialog("Did you run me as an administrator (nothing bad will happen if you didn't)?", "Administrator mode", "Yes", "No");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = true;

            addTooltip(txtBoxExtensions, "The extensions written here will be checked unless the 'All Extensions' checkbox is checked.");
            addTooltip(txtBoxDirectorySearch, "Directory in which I will search for files to merge.");
            addTooltip(txtBoxOutputDir, "Directory the output file will be created in.");
            addTooltip(txtBoxOutputFile, "Filename the output file will be named.");
            addTooltip(btnSearchDirectory, "Search for a directroy to fill in the 'search directory' field.");
            addTooltip(btnSearchForOutput, "Search for a file to output the result of the merge in.");
            addTooltip(checkBoxIncludeSubDirs, "Checking this will include subdirectories of the directory we search in.");
            addTooltip(checkBoxSyncDirFields, "Checking this will synchronize the directory search and directory output fields.");
            addTooltip(checkBoxAllExtensions, "Checking this will use all the extensions it can find in the given directory.");
            addTooltip(checkBoxUniqueFilePerExt, "Checking this will mean if there are more extensions found to be merged, it will create one respective file for each such as 'merged_html.html', 'merged_sql.sql', etc.");
            addTooltip(checkBoxDeleteOutputFile, "Checking this will delete any output file if any exist before writing a new one. If not checked and the file already exists, we return an error.");
            addTooltip(btnMerge, "Merge the files!");
            addTooltip(btnStopMerging, "Stop merging the last instance. Since you can have more directories being merged individually at the same time, this button will only stop the last executed one.");

            this.txtBoxDirectorySearch.TextChanged += txtBoxDirectorySearch_TextChanged;
            this.txtBoxOutputDir.TextChanged += txtBoxOutputDir_TextChanged;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            timerCollapseProgress = new System.Windows.Forms.Timer();
            timerCollapseProgress.Enabled = false;
            timerCollapseProgress.Interval = 16;
            timerCollapseProgress.Tick += timerCollapseProgress_Tick;

            Height -= 50; //! We set the size of the form bigger than it actually is so we can put stuff in the expanded spot
            originalHeight = Height;

            progressBarProcess.Minimum = 0;
            progressBarProcess.Value = 0;
            progressBarProcess.Maximum = 100; //! Just holder
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mergeThread = new Thread(new ThreadStart(StartMerging));
            mergeThread.Start();
        }

        private void StartMerging()
        {
            string directorySearch = txtBoxDirectorySearch.Text;
            string directoryOutput = txtBoxOutputDir.Text + txtBoxOutputFile.Text;

            if (directorySearch == "")
            {
                MessageBox.Show("The search directory field was left empty.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (directoryOutput == "" || directoryOutput == String.Empty)
                directoryOutput = directorySearch;

            if (!Directory.Exists(directorySearch))
            {
                MessageBox.Show("The given search directory does not exist.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!txtBoxDirectorySearch.Text.Contains("\\"))
            {
                MessageBox.Show("The directory search field must contain backslashes at the end (\\). For now I've added them for you.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateTextControl(txtBoxDirectorySearch, txtBoxDirectorySearch.Text + "\\");
                directorySearch += "\\";
            }

            if (txtBoxOutputDir.Text != "" && !txtBoxOutputDir.Text.Contains("\\"))
            {
                MessageBox.Show("The directory output field must contain backslashes at the end (\\). For now I've added them for you.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateTextControl(txtBoxDirectorySearch, txtBoxDirectorySearch.Text + "\\");
                directoryOutput += "\\";
            }

            if (txtBoxOutputFile.Text != "" && Path.GetDirectoryName(txtBoxOutputFile.Text) != "" && Path.GetDirectoryName(txtBoxOutputFile.Text) != "\\")
            {
                MessageBox.Show("It is not allowed to give a directory in the output FILE field.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Path.HasExtension(directorySearch))
            {
                MessageBox.Show("There is an extension in the directory field we search in.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Path.HasExtension(txtBoxOutputDir.Text))
            {
                MessageBox.Show("There is an extension in the output directory field.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtBoxOutputFile.Text != "" && txtBoxOutputDir.Text == "")
            {
                MessageBox.Show("The output directory field must be filled if the output file field is filled.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtBoxOutputFile.Text != String.Empty)
            {
                if (!Path.HasExtension(txtBoxOutputFile.Text))
                {
                    MessageBox.Show("There is no extension in the output file field but it's not empty either.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (File.Exists(directoryOutput) && checkBoxDeleteOutputFile.Enabled && !checkBoxDeleteOutputFile.Checked)
                {
                    MessageBox.Show("The given output file already exists and the checkbox to delete the output file is not checked.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string extensionString = "";

            //! Do not pick ALL extensions
            if (!checkBoxAllExtensions.Checked)
            {
                extensionString = txtBoxExtensions.Text;

                if (extensionString == "")
                {
                    MessageBox.Show("The extensions field was left empty.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (txtBoxOutputFile.Text != String.Empty)
            {
                if (txtBoxOutputFile.Text.Substring(0, 1) != "\\")
                {
                    MessageBox.Show("There are no backslashes on the start of the output file, I've added them manually.", "A warning has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateTextControl(txtBoxOutputFile, "\\" + txtBoxOutputFile.Text);
                }
            }

            if (checkBoxIncludeSubDirs.Checked)
            {
                int amountOfDirectories = Directory.GetDirectories(directorySearch).Length;

                if (amountOfDirectories > 20)
                    MessageBox.Show("I've has found more than 20 (" + amountOfDirectories + " to be exact) directories. The process might take a while. You can see the process finished by checking when the 'Merge!' button becomes click-able again.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int amountOfFiles = Directory.GetFiles(directorySearch).Length;

                if (amountOfFiles > 20)
                    MessageBox.Show("I've has found more than 20 (" + amountOfFiles + " to be exact) files. The process might take a while. You can see the process finished by checking when the 'Merge!' button becomes click-able again.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //! Re-cursive call to get all files, then put them back in an array.
            string allFiles = "";
            GetAllFilesFromDirectory(directorySearch, checkBoxIncludeSubDirs.Checked, ref allFiles);

            if (allFiles == string.Empty)
            {
                MessageBox.Show("The searched directory contains no files at all.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetEnabledOfControl(btnMerge, false);
            SetEnabledOfControl(btnStopMerging, true);

            string[] arrayFiles = allFiles.Split('\n');
            SetProgressBarMaxValue(progressBarProcess, arrayFiles.Length);
            SetLabelText(labelProgressCounter, "0 / " + arrayFiles.Length);

            if (checkBoxAllExtensions.Checked)
                for (int i = 0; i < arrayFiles.Length; i++)
                    if (arrayFiles[i] != string.Empty && arrayFiles[i] != "" && Path.HasExtension(arrayFiles[i]))
                        extensionString += Path.GetExtension(arrayFiles[i]) + ";";

            string[] extensionArray = extensionString.Split(';');
            int z = checkBoxUniqueFilePerExt.Checked ? extensionArray.Length : 1;
            bool firstLinePrinted = true, oneHardcodedOutputFile = false;

            if (!Directory.Exists(directoryOutput))
            {
                //! If the directory does not exist, we create a new one. If the directory output field
                //! contains an extension (thus is a filename with an output FILE to create), we first
                //! remove the filename from the directory and then check if the directory does not
                //! yet exist. If it doesn't, create it.
                string _directoryOutput = directoryOutput;

                //! Minus hardcoded 1 because that's the '/' line.
                if (Path.HasExtension(directoryOutput))
                    _directoryOutput = _directoryOutput.Remove(_directoryOutput.Length - 1 - Path.GetFileName(directoryOutput).Length);

                if (!Directory.Exists(_directoryOutput))
                    Directory.CreateDirectory(_directoryOutput);
            }

            if (Path.HasExtension(directoryOutput))
            {
                oneHardcodedOutputFile = true;
                z = 1; //! Only create one file
                extensionArray[0] = Path.GetExtension(directoryOutput); //! That one file we create must contain the output's file extension
                //filenameExludingExtension = Path.GetFileNameWithoutExtension(directoryOutput); //! Extension is added later on
            }

            for (int y = 0; y < z; ++y)
            {
                try
                {
                    string extensionWithoutDot = extensionArray[y].Replace(".", "");
                    string commentTypeStart = GetCommentStartTypeForLanguage(extensionWithoutDot);
                    string commentTypeEnd = GetCommentEndTypeForLanguage(extensionWithoutDot);
                    firstLinePrinted = false;
                    string fullOutputFilename = directoryOutput + "\\merged_" + extensionWithoutDot + extensionArray[y];

                    if (oneHardcodedOutputFile)
                        fullOutputFilename = directoryOutput;

                    if (!oneHardcodedOutputFile && fullOutputFilename == directorySearch + "\\merged_")
                        continue;

                    if (!checkBoxUniqueFilePerExt.Checked && z > 1)
                        fullOutputFilename = directoryOutput + "\\merged_files.txt";

                    if (Path.HasExtension(fullOutputFilename))
                    {
                        if (File.Exists(fullOutputFilename))
                        {
                            if (new FileInfo(fullOutputFilename).Length != 0 && !checkBoxDeleteOutputFile.Checked)
                            {
                                MessageBox.Show("Output file already exists and you did not check the box to delete the file if it would exist!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                            else //! Delete both if length is 0 OR if we should delete it because of the checkbox
                                File.Delete(fullOutputFilename);
                        }
                    }

                    using (StreamWriter outputFile = new StreamWriter(fullOutputFilename, true))
                    {
                        SetProgressBarValue(progressBarProcess, progressBarProcess.Value + 1);

                        for (int i = 0; i < arrayFiles.Length; i++)
                        {
                            if (Path.HasExtension(arrayFiles[i]))
                            {
                                if (Path.HasExtension(arrayFiles[i]) && (oneHardcodedOutputFile || extensionArray[y] == Path.GetExtension(arrayFiles[i])))
                                {
                                    //! We run the try-catch before writing anything to save memory. If we get
                                    //! an error, there's no reason to continue anyway.
                                    string[] linesOfFile;

                                    try
                                    {
                                        linesOfFile = File.ReadAllLines(arrayFiles[i]);
                                    }
                                    catch (IOException)
                                    {
                                        string messageToShow = "Output file could not be read (probably because it's being used). The content of the file did, however, most likely get updated properly (this is only a warning)";

                                        if (promptAdminOutcome == 2)
                                            messageToShow += ". Please note you did not run the program in administrator mode, which is most likely the problem. If you did, please make sure the file was not actually updated anyhow";

                                        messageToShow += "!";
                                        
                                        MsgBoxCheck.MessageBox dlg = new MsgBoxCheck.MessageBox();
                                        DialogResult dr =
                                                dlg.Show(@"Software\PricklySoft\TestMsgBoxCheck",
                                                "DontShowAgain", DialogResult.OK,
                                                "Don't ask me this again",
                                                messageToShow,
                                                "An error has occurred!",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        continue;
                                    }

                                    SetLabelText(labelProgressCounter, progressBarProcess.Value + " / " + progressBarProcess.Maximum);
                                    SetLabelText(labelProgressFilename, Path.GetFileName(arrayFiles[i]));

                                    if (firstLinePrinted) //! First line has to be on-top of the file.
                                        outputFile.WriteLine("\t"); //! "\t" is a single linebreak, "\n" breaks two lines.

                                    firstLinePrinted = true;
                                    outputFile.WriteLine(commentTypeStart + " - - - - - - - - - - - - - - - - - - - - - - - - - -" + commentTypeEnd);
                                    outputFile.WriteLine(commentTypeStart + " '" + arrayFiles[i] + "'" + commentTypeEnd);
                                    outputFile.WriteLine(commentTypeStart + " - - - - - - - - - - - - - - - - - - - - - - - - - -" + commentTypeEnd);
                                    outputFile.WriteLine("\t");

                                    for (int j = 0; j < linesOfFile.Length; j++)
                                        outputFile.WriteLine("\t" + linesOfFile[j]);
                                }
                            }
                        }
                    }
                }
                //! Only try, no need to catch anything. We need to set back the counter of the progress box, though.
                catch (Exception)
                {
                    SetProgressBarValue(progressBarProcess, progressBarProcess.Value - 1);
                }; 
            }

            SetEnabledOfControl(btnMerge, true);
            SetEnabledOfControl(btnStopMerging, false);

            SetProgressBarMaxValue(progressBarProcess, 100);
            SetProgressBarValue(progressBarProcess, 0);
            SetLabelText(labelProgressCounter, "");
            SetLabelText(labelProgressFilename, "");
        }

        private void GetAllFilesFromDirectory(string directorySearch, bool includingSubDirs, ref string allFiles)
        {
            try
            {
                string[] directories = Directory.GetDirectories(directorySearch);
                string[] files = Directory.GetFiles(directorySearch);

                for (int i = 0; i < files.Length; i++)
                    if (!files[i].Contains("merged_") && files[i] != "")
                        if ((File.GetAttributes(files[i]) & FileAttributes.Hidden) != FileAttributes.Hidden)
                            allFiles += files[i] + "\n";

                //! If we include sub directories, recursive call this function up to every single directory.
                if (includingSubDirs)
                    for (int i = 0; i < directories.Length; i++)
                        GetAllFilesFromDirectory(directories[i], true, ref allFiles);
            }
            catch (Exception) { }; //! Just don't do anything
        }

        private string GetCommentStartTypeForLanguage(string languageExtension)
        {
            if (languageExtension == "sql" || languageExtension == "lua")
                return "--";
            else if (languageExtension == "html" || languageExtension == "xml")
                return "<!--";
            else if (languageExtension == "php" || languageExtension == "pl" || languageExtension == "pm" ||
                languageExtension == "t" || languageExtension == "pod" || languageExtension == "rb" ||
                languageExtension == "rbw" || languageExtension == "py" || languageExtension == "pyw" ||
                languageExtension == "pyc" || languageExtension == "pyo" || languageExtension == "pyd")
                return "#";
            else if (languageExtension == "cpp" || languageExtension == "cs" || languageExtension == "d" ||
                languageExtension == "js" || languageExtension == "java" || languageExtension == "javac" ||
                languageExtension == "p" || languageExtension == "pp" || languageExtension == "pas" ||
                languageExtension == "c")
                return "//";

            return "--"; //! Default for unknown languages
        }

        private string GetCommentEndTypeForLanguage(string languageExtension)
        {
            if (languageExtension == "html" || languageExtension == "xml")
                return " -->";

            return ""; //! Default for unknown languages
        }

        private void btnSearchDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a directory to merge files from.";

            if (txtBoxDirectorySearch.Text != "" && Directory.Exists(txtBoxDirectorySearch.Text))
                fbd.SelectedPath = txtBoxDirectorySearch.Text;

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UpdateTextControl(txtBoxDirectorySearch, fbd.SelectedPath);
                txtBoxDirectorySearch_TextChanged(sender, e);
            }
        }

        void txtBoxDirectorySearch_TextChanged(object sender, System.EventArgs e)
        {
            if (syncrhonizeDirFields)
            {
                if (txtBoxDirectorySearch.Text.Length > 0)// && (txtBoxOutputDir.Text == "" || txtBoxDirectory.Text.Substring(0, txtBoxDirectory.Text.Length - 1) == txtBoxOutputDir.Text ||
                    //txtBoxOutputDir.Text.Substring(0, txtBoxOutputDir.Text.Length - 1) == txtBoxDirectory.Text))
                    UpdateTextControl(txtBoxOutputDir, txtBoxDirectorySearch.Text);
                else if (txtBoxDirectorySearch.Text == "" && txtBoxOutputDir.Text != "")
                    UpdateTextControl(txtBoxOutputDir, "");
            }
        }

        void txtBoxOutputDir_TextChanged(object sender, System.EventArgs e)
        {
            if (syncrhonizeDirFields)
            {
                if (txtBoxOutputDir.Text.Length > 0)// && (txtBoxDirectory.Text == "" || txtBoxOutputDir.Text.Substring(0, txtBoxOutputDir.Text.Length - 1) == txtBoxDirectory.Text ||
                    //txtBoxDirectory.Text.Substring(0, txtBoxDirectory.Text.Length - 1) == txtBoxOutputDir.Text))
                    UpdateTextControl(txtBoxDirectorySearch, txtBoxOutputDir.Text);
                else if (txtBoxOutputDir.Text == "" && txtBoxDirectorySearch.Text != "")
                    UpdateTextControl(txtBoxDirectorySearch, "");
            }
        }

        private void checkBoxSyncDirFields_CheckedChanged(object sender, EventArgs e)
        {
            syncrhonizeDirFields = checkBoxSyncDirFields.Checked;
        }

        private void btnSearchForOutput_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Textfiles (*.txt)*.txt";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FileName = "";

            if (txtBoxOutputDir.Text != "" && Directory.Exists(txtBoxOutputDir.Text))
                openFileDialog1.InitialDirectory = txtBoxOutputDir.Text;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syncrhonizeDirFields = false;
                string fileNameWithDir = openFileDialog1.FileName;
                string fileNameWithoutDir = Path.GetFileName(fileNameWithDir);

                if (Path.HasExtension(fileNameWithDir))
                    fileNameWithDir = fileNameWithDir.Substring(0, fileNameWithDir.Length - Path.GetFileName(fileNameWithDir).Length);

                txtBoxOutputDir.Text = fileNameWithDir;
                txtBoxOutputFile.Text = "\\" + fileNameWithoutDir;
                syncrhonizeDirFields = checkBoxSyncDirFields.Checked;
                txtBoxOutputDir_TextChanged(sender, e);
            }
        }

        private void checkBoxAllExtensions_CheckedChanged(object sender, EventArgs e)
        {
            txtBoxExtensions.Enabled = !checkBoxAllExtensions.Checked;
        }

        private void addTooltip(Control control, string tooltipMsg)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(control, tooltipMsg);
            toolTip.ShowAlways = true;
        }

        private void txtBoxOutputFile_TextChanged(object sender, EventArgs e)
        {
            checkBoxDeleteOutputFile.Enabled = Path.HasExtension(txtBoxOutputFile.Text);
            checkBoxUniqueFilePerExt.Enabled = !Path.HasExtension(txtBoxOutputFile.Text);
        }

        private void buttonStopMerging_Click(object sender, EventArgs e)
        {
            if (mergeThread != null && mergeThread.IsAlive)
            {
                mergeThread.Abort();
                mergeThread = null;
            }

            SetEnabledOfControl(btnMerge, true);
            SetEnabledOfControl(btnStopMerging, false);

            //? Why doesn't this work for all four of the lines below? Only two of them have effect (one label and one progressbar related).
            progressBarProcess.Value = 0;
            progressBarProcess.Maximum = 100;
            labelProgressCounter.Text = "";
            labelProgressFilename.Text = "";
        }

        private delegate void UpdateTextControlDelegate(Control control, string text);

        private void UpdateTextControl(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                Invoke(new UpdateTextControlDelegate(UpdateTextControl), new object[] { control, text });
                return;
            }

            control.Text = text;
        }

        private delegate void SetEnabledOfControlDelegate(Control control, bool enable);

        private void SetEnabledOfControl(Control control, bool enable)
        {
            if (control.InvokeRequired)
            {
                Invoke(new SetEnabledOfControlDelegate(SetEnabledOfControl), new object[] { control, enable });
                return;
            }

            control.Enabled = enable;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    button1_Click(sender, e);
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Are you sure you want to quit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        Close();

                    break;
            }
        }

        private void checkBoxShowProgress_CheckedChanged(object sender, EventArgs e)
        {
            timerCollapseProgress.Enabled = true;
        }

        private void timerCollapseProgress_Tick(object sender, EventArgs e)
        {
            if (checkBoxShowProgress.Checked)
            {
                if (Height >= originalHeight + 50)
                {
                    Height = originalHeight + 50;
                    timerCollapseProgress.Enabled = false;
                }
                else
                    Height += 5;
            }
            else
            {
                if (Height > originalHeight)
                    Height -= 5;
                else
                {
                    Height = originalHeight;
                    timerCollapseProgress.Enabled = false;
                }
            }
        }

        private delegate void SetProgressBarMaxValueDelegate(ProgressBar progressBar, int value);

        private void SetProgressBarMaxValue(ProgressBar progressBar, int value)
        {
            if (progressBar.InvokeRequired)
            {
                Invoke(new SetProgressBarMaxValueDelegate(SetProgressBarMaxValue), new object[] { progressBar, value });
                return;
            }

            progressBar.Maximum = value;
        }

        private delegate void SetProgressBarValueDelegate(ProgressBar progressBar, int value);

        private void SetProgressBarValue(ProgressBar progressBar, int value)
        {
            if (progressBar.InvokeRequired)
            {
                Invoke(new SetProgressBarValueDelegate(SetProgressBarValue), new object[] { progressBar, value });
                return;
            }

            if (progressBar.Value >= progressBar.Maximum)
            {
                progressBar.Value = progressBar.Maximum;
                return;
            }

            progressBar.Value = value;
        }

        private delegate void SetLabelTextDelegate(Label label, string text);

        private void SetLabelText(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                Invoke(new SetLabelTextDelegate(SetLabelText), new object[] { label, text });
                return;
            }

            label.Text = text;
        }

        private void progressBarProcess_Click(object sender, EventArgs e)
        {

        }
    }

    public static class Prompt
    {
        public static int ShowDialog(string text, string caption, string btnOneText, string btnTwoText)
        {
            Form prompt = new Form();
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            prompt.ShowIcon = false;
            prompt.Width = 300;
            prompt.Height = 125;
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 10, Top = 15, Text = text };
            Button firstButton = new Button() { Text = btnOneText, Left = 30, Width = 90, Top = 50 };
            Button secondButton = new Button() { Text = btnTwoText, Left = 160, Width = 90, Top = 50 };
            int clickedFirstButton = 0; //! 0 = uninitialized (red 'X' for example), 1 = button one, 2 = button two
            firstButton.Click  += (sender, e) => { prompt.Close(); clickedFirstButton = 1; };
            secondButton.Click += (sender, e) => { prompt.Close(); clickedFirstButton = 2; };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(firstButton);
            prompt.Controls.Add(secondButton);
            prompt.ShowDialog();

            //! Keep opening new prompts until the user pressed either of the buttons.
            return clickedFirstButton > 0 ? clickedFirstButton : Prompt.ShowDialog(text, caption, btnOneText, btnTwoText);
        }
    }
}
