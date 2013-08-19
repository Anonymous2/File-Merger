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
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string directory = txtBoxDirectory.Text;

            if (directory == "")
            {
                MessageBox.Show("The directory field was left empty.", "An error has occurred!");
                return;
            }

            if (!Directory.Exists(directory))// || !((File.GetAttributes(directory) & FileAttributes.Directory) == FileAttributes.Directory))
            {
                MessageBox.Show("The given directory does not exist.", "An error has occurred!");
                return;
            }

            if (Path.HasExtension(directory))
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
            string allFiles = ""; //! Reset this first so we don't get duplicates after pressing the Merge button more than once.
            GetAllFilesFromDirectory(directory, checkBoxIncludeSubDirs.Checked, ref allFiles);

            if (allFiles == string.Empty)
            {
                MessageBox.Show("The searched directory contains no files at all.", "An error has occurred!");
                return;
            }

            string[] arrayFiles = allFiles.Split('\n');

            if (checkBoxAllExtensions.Checked)
                for (int i = 0; i < arrayFiles.Length; i++)
                    if (Path.HasExtension(arrayFiles[i]))
                        extensionString += Path.GetExtension(arrayFiles[i]) + ";";

            string[] extensionArray = extensionString.Split(';');
            int z = checkBoxExtensions.Checked ? extensionArray.Length : 1;
            bool firstLinePrinted = true;

            for (int y = 0; y < z; ++y)
            {
                string extensionWithoutDot = extensionArray[y].Replace(".", "");
                string commentTypeStart = GetCommentStartTypeForLanguage(extensionWithoutDot);
                string commentTypeEnd = GetCommentEndTypeForLanguage(extensionWithoutDot);
                firstLinePrinted = false;

                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(directory + "\\merged_" + extensionWithoutDot + extensionArray[y], true))
                {
                    for (int i = 0; i < arrayFiles.Length; i++)
                    {
                        if (Path.HasExtension(arrayFiles[i]) && extensionArray[y] == Path.GetExtension(arrayFiles[i]))
                        {
                            if (firstLinePrinted) //! First line has to be on-top of the file.
                                outputFile.WriteLine("\t"); //! "\t" is a single linebreak, "\n" breaks two lines.

                            firstLinePrinted = true;
                            outputFile.WriteLine(commentTypeStart + " - - - - - - - - - - - - - - - - - - - - - - - - - -" + commentTypeEnd);
                            outputFile.WriteLine(commentTypeStart + " '" + arrayFiles[i] + "'" + commentTypeEnd);
                            outputFile.WriteLine(commentTypeStart + " - - - - - - - - - - - - - - - - - - - - - - - - - -" + commentTypeEnd);
                            outputFile.WriteLine("\t");

                            string[] linesOfFile = System.IO.File.ReadAllLines(arrayFiles[i]);

                            for (int j = 0; j < linesOfFile.Length; j++)
                                outputFile.WriteLine("\t" + linesOfFile[j]);
                        }
                    }
                }
            }
        }

        private void GetAllFilesFromDirectory(string directory, bool includingSubDirs, ref string allFiles)
        {
            string[] directories = Directory.GetDirectories(directory);
            string[] files = Directory.GetFiles(directory);

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
    }
}
