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

        private string allFiles;

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
            allFiles = ""; //! Reset this first so we don't get duplicates after pressing the Merge button more than once.
            GetAllFilesFromDirectory(directory, checkBoxIncludeSubDirs.Checked);

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
                firstLinePrinted = false;

                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(directory + "\\merged_" + extensionWithoutDot + extensionArray[y], true))
                {
                    for (int i = 0; i < arrayFiles.Length; i++)
                    {
                        if (Path.HasExtension(arrayFiles[i]) && extensionArray[y].Contains(Path.GetExtension(arrayFiles[i])))
                        {
                            if (firstLinePrinted) //! First line has to be on-top of the file.
                                outputFile.WriteLine("\t"); //! "\t" is a single linebreak, "\n" breaks two lines.

                            firstLinePrinted = true;
                            outputFile.WriteLine("-- - - - - - - - - - - - - - - - - - - - - - - - - - -");
                            outputFile.WriteLine("-- '" + arrayFiles[i] + "'");
                            outputFile.WriteLine("-- - - - - - - - - - - - - - - - - - - - - - - - - - -");
                            outputFile.WriteLine("\t");

                            string[] linesOfFile = System.IO.File.ReadAllLines(arrayFiles[i]);

                            for (int j = 0; j < linesOfFile.Length; j++)
                                outputFile.WriteLine("\t" + linesOfFile[j]);
                        }
                    }
                }
            }
        }

        private void GetAllFilesFromDirectory(string directory, bool includingSubDirs)
        {
            string[] directories = Directory.GetDirectories(directory);
            string[] files = Directory.GetFiles(directory);

            for (int i = 0; i < files.Length; i++)
                allFiles += files[i] + "\n";

            //! If we include sub directories, recursive call this function up to every single directory.
            if (includingSubDirs)
                for (int i = 0; i < directories.Length; i++)
                    GetAllFilesFromDirectory(directories[i], true);
        }

        private void checkBoxAllExtensions_CheckedChanged(object sender, EventArgs e)
        {
            //! No point in giving extensions if we will look for all extensions.
            txtBoxExtensions.Enabled = !checkBoxAllExtensions.Checked;
        }
    }
}
