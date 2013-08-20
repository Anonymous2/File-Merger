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

            this.txtBoxDirectory.TextChanged += txtBoxDirectory_TextChanged;
            this.txtBoxDirectoryOutput.TextChanged += txtBoxDirectoryOutput_TextChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string directorySearch = txtBoxDirectory.Text;
            string directoryOutput = txtBoxDirectoryOutput.Text;

            if (directorySearch == "")
            {
                MessageBox.Show("The directory field was left empty.", "An error has occurred!");
                return;
            }

            if (!Directory.Exists(directorySearch))
            {
                MessageBox.Show("The given directory does not exist.", "An error has occurred!");
                return;
            }

            if (Path.HasExtension(directorySearch))
            {
                MessageBox.Show("There is an extension in the directory.", "An error has occurred!");
                return;
            }

            string extensionString = "";

            if (!checkBoxAllExtensions.Checked)
            {
                extensionString = txtBoxExtensions.Text;

                if (extensionString == "")
                {
                    MessageBox.Show("The extensions field was left empty.", "An error has occurred!");
                    return;
                }
            }

            //! Re-cursive call to get all files, then put them back in an array.
            string allFiles = "";
            GetAllFilesFromDirectory(directorySearch, checkBoxIncludeSubDirs.Checked, ref allFiles);

            if (allFiles == string.Empty)
            {
                MessageBox.Show("The searched directory contains no files at all.", "An error has occurred!");
                return;
            }

            string[] arrayFilesOld = allFiles.Split('\n');
            string[] arrayFiles;

            List<string> list = new List<string>(arrayFilesOld);
            list.Remove("");
            arrayFiles = list.ToArray();

            if (checkBoxAllExtensions.Checked)
                for (int i = 0; i < arrayFiles.Length; i++)
                    if (arrayFiles[i] != string.Empty && arrayFiles[i] != "" && Path.HasExtension(arrayFiles[i]))
                        extensionString += Path.GetExtension(arrayFiles[i]) + ";";

            string[] extensionArray = extensionString.Split(';');
            int z = checkBoxExtensions.Checked ? extensionArray.Length : 1;
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
                            MessageBox.Show("Output file already exists and you did not check the box to delete the file if it would exist!", "An error has occurred!");
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
                        if (Path.HasExtension(arrayFiles[i]) && (oneHardcodedOutputFile || extensionArray[y] == Path.GetExtension(arrayFiles[i])))
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
                                string messageToShow = "Output file could not be read (probably because it's being used)";

                                if (promptAdmOutcome == 2)
                                    messageToShow += ". Please note you did not run the program in administrator mode, which is most likely the problem. If you did, please make sure the file was not actually updated anyhow";

                                messageToShow += "!";
                                MessageBox.Show(messageToShow, "An error has occurred!");
                                continue;
                            }

                            for (int j = 0; j < linesOfFile.Length; j++)
                                outputFile.WriteLine("\t" + linesOfFile[j]);
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

        private void checkBoxAllExtensions_CheckedChanged(object sender, EventArgs e)
        {
            //! No point in giving extensions if we will look for all extensions.
            txtBoxExtensions.Enabled = !checkBoxAllExtensions.Checked;
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
                txtBoxDirectoryOutput.Text = fbd.SelectedPath;
                txtBoxDirectoryOutput_TextChanged(sender, e);
            }
        }

        void txtBoxDirectory_TextChanged(object sender, System.EventArgs e)
        {
            if (syncrhonizeDirFields)
            {
                if (txtBoxDirectory.Text.Length > 0)// && (txtBoxDirectoryOutput.Text == "" || txtBoxDirectory.Text.Substring(0, txtBoxDirectory.Text.Length - 1) == txtBoxDirectoryOutput.Text ||
                    //txtBoxDirectoryOutput.Text.Substring(0, txtBoxDirectoryOutput.Text.Length - 1) == txtBoxDirectory.Text))
                    txtBoxDirectoryOutput.Text = txtBoxDirectory.Text;
                else if (txtBoxDirectory.Text == "" && txtBoxDirectoryOutput.Text != "")
                    txtBoxDirectoryOutput.Text = "";
            }
        }

        void txtBoxDirectoryOutput_TextChanged(object sender, System.EventArgs e)
        {
            if (syncrhonizeDirFields)
            {
                if (txtBoxDirectoryOutput.Text.Length > 0)// && (txtBoxDirectory.Text == "" || txtBoxDirectoryOutput.Text.Substring(0, txtBoxDirectoryOutput.Text.Length - 1) == txtBoxDirectory.Text ||
                    //txtBoxDirectory.Text.Substring(0, txtBoxDirectory.Text.Length - 1) == txtBoxDirectoryOutput.Text))
                    txtBoxDirectory.Text = txtBoxDirectoryOutput.Text;
                else if (txtBoxDirectoryOutput.Text == "" && txtBoxDirectory.Text != "")
                    txtBoxDirectory.Text = "";
            }

            checkBoxDeleteOutputFile.Enabled = Path.HasExtension(txtBoxDirectoryOutput.Text);
        }

        private void checkBoxSyncDirFields_CheckedChanged(object sender, EventArgs e)
        {
            syncrhonizeDirFields = checkBoxSyncDirFields.Checked;
        }

        private void btnSearchForOutput_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Textfiles (*.txt)*.txt";
            openFileDialog1.Filter = "All files (*.*)|*.*";

            if (txtBoxDirectoryOutput.Text != "" && Directory.Exists(txtBoxDirectoryOutput.Text))
                openFileDialog1.InitialDirectory = txtBoxDirectoryOutput.Text;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syncrhonizeDirFields = false;
                txtBoxDirectoryOutput.Text = openFileDialog1.FileName;
                syncrhonizeDirFields = true;

                txtBoxDirectory_TextChanged(sender, e);
            }
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
