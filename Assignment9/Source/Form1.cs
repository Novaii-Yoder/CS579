using System.Diagnostics;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NjRAT_Remover
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool IsProcessRunning(string processName)
        {
            // Get all processes with the specified name
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;  // Return true if any processes with that name are found
        }

        private bool removeFile(string filepath)
        {
            try
            {
                // Check if file exists with its full path
                if (File.Exists(filepath))
                {
                    // If file found, delete it
                    File.Delete(filepath);
                    //textBox1.AppendText($"File \"{filepath}\" deleted.\r\n");
                    return true;
                }
                else
                {
                    //textBox1.AppendText($"File \"{filepath}\" not found.\r\n");
                    return false;
                }
            }
            catch (IOException ioExp)
            {
                textBox1.AppendText($"ERROR: {ioExp.Message}\r\n");
            }
            return false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool setRemoveButton = false;

            bool isRunning = IsProcessRunning("njrat") || IsProcessRunning("windows");


            textBox1.Text = ""; // clear textbox


            if (isRunning)
            {
                textBox1.AppendText("NjRat is currently running!\r\n");
                setRemoveButton = true;
            }
            else
            {
                textBox1.AppendText("NjRat is not running.\r\n");
            }

            try
            {
                // files to look for
                string[] filePatterns = new string[]
                {
                    @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\ecc7c8c51c0850c1ec247c7fd3602f20.exe",
                    @"C:\njRAT.exe",
                    @"C:\njq8.exe",
                    @"C:\Users\{0}\AppData\Local\Temp\windows.exe"
                };
                string[] userProfiles = Directory.GetDirectories(@"C:\Users");
                int counter = 0;

                //check root c:/ folder
                foreach (string pattern in filePatterns)
                {
                    if (!pattern.Contains("{0}")) // Check non-user specific files only once
                    {
                        if (File.Exists(pattern))
                        {
                            textBox1.AppendText("Found file: " + pattern + "\r\n");
                            counter++;
                        }
                    }
                }

                // check user folders
                foreach (string userProfile in userProfiles)
                {
                    string userName = Path.GetFileName(userProfile);
                    foreach (string pattern in filePatterns)
                    {
                        if (pattern.Contains("{0}")) // For paths that do depend on the user profile
                        {
                            string filePath = string.Format(pattern, userName); // Replace {0} with the username

                            if (File.Exists(filePath))
                            {
                                textBox1.AppendText("Found file: " + filePath + "\r\n");
                                counter++;
                            }
                        }
                    }
                }


                textBox1.AppendText($"Suspicious files found: {counter}\r\n");
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as directory not found or access denied
                // MessageBox.Show($"An error occurred: {ex.Message}");
                textBox1.AppendText($"ERROR: {ex.Message}\r\n");
            }

            button2.Visible = setRemoveButton;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool processStopped = false;
            Process[] njrats = Process.GetProcessesByName("windows");
            njrats = njrats.Concat(Process.GetProcessesByName("njrat")).ToArray();
            int counterf = 0;
            int counterp = 0;

            foreach (Process rat in njrats)
            {
                textBox1.AppendText($"Stopping process {rat}.\r\n");
                rat.Kill();
                rat.WaitForExit();
                rat.Dispose();
                //textBox1.AppendText($"Stopped process {rat}.\r\n");
                counterp++;
                processStopped = true;
            }

            if (!processStopped)
            {
                textBox1.AppendText($"ERROR: No running process found.\r\n");
            }


            string[] filePatterns = new string[]
            {
                @"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\ecc7c8c51c0850c1ec247c7fd3602f20.exe",
                @"C:\njRAT.exe",
                @"C:\njq8.exe",
                @"C:\Users\{0}\AppData\Local\Temp\windows.exe"
            };

            string[] userProfiles = Directory.GetDirectories(@"C:\Users");


            foreach (string pattern in filePatterns)
            {
                foreach (string userProfile in userProfiles)
                {
                    //textBox1.AppendText($"Users: {userProfile}");
                    string userName = Path.GetFileName(userProfile);

                    string filePath = string.Format(pattern, userName); // Replace {0} with the username
                    if (!pattern.Contains("{0}")) // For paths that do not depend on the user profile
                    {
                        filePath = pattern;
                    }

                    // Call the method to remove the file
                    if (removeFile(filePath))
                    {
                        textBox1.AppendText($"Removed file: {filePath}\r\n");
                        counterf++;
                    }
                }
            }
            textBox1.AppendText($"Successfully removed {counterf} suspicious files and stopped {counterp} njRAT process(es)\r\n");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
