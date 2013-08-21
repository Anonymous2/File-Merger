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

namespace File_Merger
{
    public partial class MainForm : Form
    {
        private bool syncrhonizeDirFields = true;
        private int promptAdmOutcome = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //promptAdmOutcome = Prompt.ShowDialog("Did you run the application as an administrator (nothing bad will happen if you didn't)?", "Administrator mode", "Yes", "No");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            addTooltip(txtBoxExtensions, "The extensions written here will either be used or ignored, depending on if the checkbox to reverse this field is checked.");
            addTooltip(txtBoxDirectory, "Directory in which the application will search for files to merge.");
            addTooltip(txtBoxOutputDir, "Directory the output file will be created in.");
            addTooltip(txtBoxOutputFile, "Filename the output file will be named.");
            addTooltip(btnSearchDirectory, "Search for a directroy to fill in the 'search directory' field.");
            addTooltip(btnSearchForOutput, "Search for a file to output the result of the merge in.");
            addTooltip(checkBoxIncludeSubDirs, "Checking this will make the application include subdirectories of the directory we search in.");
            addTooltip(checkBoxReverseExtensions, "Checking this will reverse the extensions field, meaning we ignore any given extensions written there.");
            addTooltip(checkBoxSyncDirFields, "Checking this will synchronize the directory search and directory output fields.");
            addTooltip(checkBoxAllExtensions, "Checking this will make the application use all the extensions it can find in the given directory.");
            addTooltip(checkBoxUniqueFilePerExt, "Checking this will mean if there are more extensions found to be merged, it will create one respective file for each such as 'merged_html.html', 'merged_sql.sql', etc.");
            addTooltip(checkBoxReverseExtensions, "Checking this will reverse the 'extensions' field (unless 'All Extensions' is checked). From that point on, the given extensions will be ignored.");
            addTooltip(checkBoxDeleteOutputFile, "Checking this will delete any output file if any exist before writing a new one. If not checked and the file already exists, we return an error.");
            addTooltip(btnMerge, "Merge the files!");

            this.txtBoxDirectory.TextChanged += txtBoxDirectory_TextChanged;
            this.txtBoxOutputDir.TextChanged += txtBoxOutputDir_TextChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string directorySearch = txtBoxDirectory.Text;
            string directoryOutput = txtBoxOutputDir.Text + txtBoxOutputFile.Text;

            if (directorySearch == "")
            {
                MessageBox.Show("The search directory field was left empty.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(directorySearch))
            {
                MessageBox.Show("The given search directory does not exist.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (txtBoxOutputFile.Text != String.Empty && !Path.HasExtension(txtBoxOutputFile.Text))
            {
                MessageBox.Show("There is no extension in the output file field but it's not empty either.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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

            //! Re-cursive call to get all files, then put them back in an array.
            string allFiles = "";
            GetAllFilesFromDirectory(directorySearch, checkBoxIncludeSubDirs.Checked, ref allFiles);

            if (allFiles == string.Empty)
            {
                MessageBox.Show("The searched directory contains no files at all.", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] arrayFiles = allFiles.Split('\n');

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
                string extensionWithoutDot = extensionArray[y].Replace(".", "");
                string commentTypeStart = GetCommentStartTypeForLanguage(extensionWithoutDot);
                string commentTypeEnd = GetCommentEndTypeForLanguage(extensionWithoutDot);
                firstLinePrinted = false;
                string fullOutputFilename = directoryOutput + "\\merged_" + extensionWithoutDot + extensionArray[y];

                if (oneHardcodedOutputFile)
                    fullOutputFilename = directoryOutput;

                if (!oneHardcodedOutputFile && fullOutputFilename == directorySearch + "\\merged_")
                    continue;

                if (Path.HasExtension(fullOutputFilename))
                {
                    if (File.Exists(fullOutputFilename))
                    {
                        if (new FileInfo(fullOutputFilename).Length != 0 && !checkBoxDeleteOutputFile.Checked)
                        {
                            MessageBox.Show("Output file already exists and you did not check the box to delete the file if it would exist!", "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        else //! Delete both if length is 0 OR when we should delete it because of the checkbox
                        {
                            //string _fullOutputFilename = fullOutputFilename.Replace("\\", "'\'\");
                            //File.Delete(@_fullOutputFilename);
                            File.Delete(@fullOutputFilename);
                        }
                    }
                }

                using (StreamWriter outputFile = new StreamWriter(fullOutputFilename, true))
                {
                    for (int i = 0; i < arrayFiles.Length; i++)
                    {
                        if (Path.HasExtension(arrayFiles[i]))
                        {
                            if (Path.HasExtension(arrayFiles[i]) && (oneHardcodedOutputFile || (checkBoxReverseExtensions.Checked != (extensionArray[y] == Path.GetExtension(arrayFiles[i])))))
                            {
                                if (firstLinePrinted) //! First line has to be on-top of the file.
                                    outputFile.WriteLine("\t"); //! "\t" is a single linebreak, "\n" breaks two lines.

                                firstLinePrinted = true;
                                outputFile.WriteLine(commentTypeStart + " - - - - - - - - - - - - - - - - - - - - - - - - - -" + commentTypeEnd);
                                outputFile.WriteLine(commentTypeStart + " '" + arrayFiles[i] + "'" + commentTypeEnd);
                                outputFile.WriteLine(commentTypeStart + " - - - - - - - - - - - - - - - - - - - - - - - - - -" + commentTypeEnd);
                                outputFile.WriteLine("\t");

                                string[] linesOfFile;

                                try
                                {
                                    linesOfFile = File.ReadAllLines(arrayFiles[i]);
                                }
                                catch (IOException)
                                {
                                    string messageToShow = "Output file could not be read (probably because it's being used). The content of the file did, however, most likely get updated properly (this is only a warning).";

                                    if (promptAdmOutcome == 2)
                                        messageToShow += ". Please note you did not run the program in administrator mode, which is most likely the problem. If you did, please make sure the file was not actually updated anyhow";

                                    messageToShow += "!";
                                    MessageBox.Show(messageToShow, "An error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    continue;
                                }

                                for (int j = 0; j < linesOfFile.Length; j++)
                                    outputFile.WriteLine("\t" + linesOfFile[j]);
                            }
                        }
                    }
                }
            }
        }

        private void GetAllFilesFromDirectory(string directorySearch, bool includingSubDirs, ref string allFiles)
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

            if (txtBoxDirectory.Text != "" && Directory.Exists(txtBoxDirectory.Text))
                fbd.SelectedPath = txtBoxDirectory.Text;

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtBoxOutputDir.Text = fbd.SelectedPath;
                txtBoxOutputDir_TextChanged(sender, e);
            }
        }

        void txtBoxDirectory_TextChanged(object sender, System.EventArgs e)
        {
            if (syncrhonizeDirFields)
            {
                if (txtBoxDirectory.Text.Length > 0)// && (txtBoxOutputDir.Text == "" || txtBoxDirectory.Text.Substring(0, txtBoxDirectory.Text.Length - 1) == txtBoxOutputDir.Text ||
                    //txtBoxOutputDir.Text.Substring(0, txtBoxOutputDir.Text.Length - 1) == txtBoxDirectory.Text))
                    txtBoxOutputDir.Text = txtBoxDirectory.Text;
                else if (txtBoxDirectory.Text == "" && txtBoxOutputDir.Text != "")
                    txtBoxOutputDir.Text = "";
            }
        }

        void txtBoxOutputDir_TextChanged(object sender, System.EventArgs e)
        {
            if (syncrhonizeDirFields)
            {
                if (txtBoxOutputDir.Text.Length > 0)// && (txtBoxDirectory.Text == "" || txtBoxOutputDir.Text.Substring(0, txtBoxOutputDir.Text.Length - 1) == txtBoxDirectory.Text ||
                    //txtBoxDirectory.Text.Substring(0, txtBoxDirectory.Text.Length - 1) == txtBoxOutputDir.Text))
                    txtBoxDirectory.Text = txtBoxOutputDir.Text;
                else if (txtBoxOutputDir.Text == "" && txtBoxDirectory.Text != "")
                    txtBoxDirectory.Text = "";
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
                syncrhonizeDirFields = true;

                txtBoxOutputDir_TextChanged(sender, e);
            }
        }

        private void checkBoxReverseExtensions_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxReverseExtensions.Checked)
            {
                if (!checkBoxAllExtensions.Checked)
                    txtBoxExtensions.BackColor = Color.LightYellow;

                labelExtensionsToMerge.Text = "Extensions NOT to merge (split by semicolon if more than one):";
            }
            else
            {
                if (!checkBoxAllExtensions.Checked)
                    txtBoxExtensions.BackColor = txtBoxExtensions.Enabled ? Color.White : SystemColors.Control;

                labelExtensionsToMerge.Text = "Extensions to merge (split by semicolon if more than one):";
            }
        }

        private void checkBoxAllExtensions_CheckedChanged(object sender, EventArgs e)
        {
            txtBoxExtensions.Enabled = !checkBoxAllExtensions.Checked;

            if (checkBoxAllExtensions.Checked)
            {
                txtBoxExtensions.BackColor = SystemColors.Control;
                return;
            }

            if (checkBoxReverseExtensions.Checked)
                txtBoxExtensions.BackColor = Color.LightYellow;
            else
                txtBoxExtensions.BackColor = txtBoxExtensions.Enabled ? Color.White : SystemColors.Control;
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
