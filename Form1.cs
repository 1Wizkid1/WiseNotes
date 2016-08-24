using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Configuration;
using System.Collections.Specialized;
using System.Configuration.Assemblies;

namespace CSWiki_DB
{
    public partial class Form1 : Form
    {
        static string dbLoc;
        static string notesLoc;
        public static string connString;
        public static SqlConnection connect;
        public static SqlCommand myCommand;
        public static SqlDataReader reader;
        public static string selectedcategory;
        public static string selectednote;
        public string filepath;
        public int fontsz = 12;
        public string filename;
        public int boldornot = 0;
        public int underornot = 0;
        string folderChoice;
        static string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string configFile = System.IO.Path.Combine(appPath, "App.config");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkConfig();
            dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"] + @"\wisenotes.mdf";
            notesLoc = System.Configuration.ConfigurationManager.AppSettings["notesFolder"];
            connString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=" + dbLoc;
            connect = new SqlConnection(connString);
            connect.Close();
            connect.Open();
            myCommand = connect.CreateCommand();
            
            loadcategories();
        }

        private void checkConfig()
        {
            dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"];
            notesLoc = System.Configuration.ConfigurationManager.AppSettings["notesFolder"];

            if (dbLoc == "0" || (dbLoc == ""))
            {
                MessageBox.Show("Please set the folder where the database will be stored.");
                folderBrowserDialog1.ShowDialog();
                folderChoice = folderBrowserDialog1.SelectedPath;
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = configFile;
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["dbFolder"].Value = folderChoice.ToString();
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"];
                
            }
            else
            {
                
            }
            
            if (notesLoc == "0" || (notesLoc == ""))
            {
                MessageBox.Show("Please set the folder where the note files will be stored.");
                folderBrowserDialog1.ShowDialog();
                folderChoice = folderBrowserDialog1.SelectedPath;
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = configFile;
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["notesFolder"].Value = folderChoice.ToString();
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                notesLoc = System.Configuration.ConfigurationManager.AppSettings["notesFolder"];
            }
            else
            {
               
            }
            copyFiles();
        }

        private void copyFiles()
        {
            string dbFile1 = "wisenotes.mdf";
            string dbFile2 = "wisenotes_log.ldf";
            string dbFolder = @"c:\program files (x86)\WiseTech\WiseNotes\dbFiles";
            string sourceFile1 = System.IO.Path.Combine(dbFolder, dbFile1);
            string sourceFile2 = System.IO.Path.Combine(dbFolder, dbFile2);
            string destFile1 = System.IO.Path.Combine(dbLoc, dbFile1);
            string destFile2 = System.IO.Path.Combine(dbLoc, dbFile2);
            if (!System.IO.File.Exists(destFile1) || (!System.IO.File.Exists(destFile2)))
                    {
                System.IO.File.Copy(sourceFile1, destFile1, true);
                System.IO.File.Copy(sourceFile2, destFile2, true);
                    }
            else
                {

                }
        }

        

        private void addnewnote_Click(object sender, EventArgs e)
        {
            AddNote frm2 = new AddNote();
            this.Hide();
            frm2.ShowDialog();
        }

        private void loadcategories()
        {
            //myCommand.CommandText = @"SELECT * FROM catfolder ORDER BY categories";
            myCommand.CommandText = @"SELECT * FROM catfolder";
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    categorylist.Items.Add(reader.GetString(reader.GetOrdinal("categories")));
                }
            }
        }

        private void categorylist_Click(object sender, EventArgs e)
        {
            connect.Close();
            connect.Open();
            noteslist.Items.Clear();
            richTextBox1.Text = "";
            if (categorylist.SelectedItem == null)
            {
                return;
            }
            else
            {
                selectedcategory = categorylist.SelectedItem.ToString();
                notelistupdate();
            }
        }

        private void notelistupdate()
        {
            myCommand.Parameters.Clear();
            myCommand.CommandText = @"SELECT * FROM notes WHERE category = @input ORDER by Convert(varchar(max),title)"; 
 
             myCommand.Parameters.AddWithValue("@input", selectedcategory);
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    noteslist.Items.Add(reader.GetString(reader.GetOrdinal("title")));
                }
            }
        }

        private void noteslist_Click(object sender, EventArgs e)
        {
            connect.Close();
            connect.Open();
            if (noteslist.SelectedItem == null)
            {
                return;
            }

            else
            {
                selectednote = noteslist.SelectedItem.ToString();
                richTextBox1.Text = "";
                myCommand.Parameters.Clear();
                myCommand.CommandText = @"SELECT * FROM notes WHERE title = @input";
                myCommand.Parameters.AddWithValue("@input", selectednote);
                reader = myCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        filepath = reader.GetString(reader.GetOrdinal("filepath"));
                        filepath = filepath.Replace(@"\", @"\\\\");
                        filepath = filepath.Replace("\"", "");
                        richTextBox1.LoadFile(filepath);
                        filename = filepath.Replace(@"c:\\\\\\\\\\\\\\\\program files (x86)\\\\\\\\\\\\\\\\wisetech\\\\\\\\\\\\\\\\wisenotes\\\\\\\\\\\\\\\\notefiles\\\\\\\\\\\\\\\\", "");
                    }
                }
            }
        }

      

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Show the form when the user double clicks on the notify icon. 

            // Set the WindowState to normal if the form is minimized. 
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Minimized;
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            // Activate the form. 
            this.Activate();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void aDMINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SaveRTF();
        }

        public void SaveRTF()
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();
            
            if (noteslist.SelectedItem == null)
            {
                
            }
            else
            {
                
                saveFile1.FileName = filename;
            }
            
            saveFile1.RestoreDirectory = true;
            saveFile1.InitialDirectory = notesLoc;
            // Initialize the SaveFileDialog to specify the RTF extention for the file.
            saveFile1.DefaultExt = "*.rtf";
            saveFile1.Filter = "RTF Files|*.rtf";
            
            // Determine whether the user selected a file name from the saveFileDialog. 
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                richTextBox1.SaveFile(saveFile1.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            boldornot++;
            
            if (boldornot == 1)
            {
                richTextBox1.SelectionFont = new Font("Arial", fontsz, FontStyle.Bold);
            }
            if (boldornot == 2)
            {
                boldornot = 0;
                richTextBox1.SelectionFont = new Font("Arial", fontsz, FontStyle.Regular);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        
        private void button8_Click(object sender, EventArgs e)
        {
            fontsz++;
            richTextBox1.SelectionFont = new Font("Arial", fontsz);
            
            //richTextBox1.SelectionLength = 0;//Unselect the selection
          //  fontsz++;
            //richTextBox1.Font = new Font("Arial", fontsz);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            fontsz--;
            richTextBox1.SelectionFont = new Font("Arial", fontsz);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            categorylist.ClearSelected();
            noteslist.ClearSelected();
            richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void U_Click(object sender, EventArgs e)
        {
            underornot++;

            if (underornot == 1)
            {
                richTextBox1.SelectionFont = new Font("Arial", fontsz, FontStyle.Underline);
            }
            if (underornot == 2)
            {
                underornot = 0;
                richTextBox1.SelectionFont = new Font("Arial", fontsz, FontStyle.Regular);
            }
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            this.Hide();
            frm4.Show();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PrintDialog printDialog1 = new PrintDialog())
            {
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    var pi = new ProcessStartInfo(filepath);
                    pi.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                    pi.CreateNoWindow = true;
                    pi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    pi.UseShellExecute = true;
                    pi.Verb = "PrintTo";
                    System.Diagnostics.Process.Start(pi);
                }
            }
            
           
        }

        private void licensingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("WiseNotes is free to use" +Environment.NewLine + "It may not be sold or marketed without the permission of WiseTech." +Environment.NewLine + "Please Contact Kyle at WiseTech at kdwisdom@gmail.com for more questions");
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            noteslist.Items.Clear();
            string searchParameters = "%" + textSearch.Text + "%";
            connect.Close();
            connect.Open();

            richTextBox1.Text = "";
            myCommand.Parameters.Clear();
            myCommand.CommandText = @"SELECT * FROM notes WHERE title like @search ORDER by title";
            myCommand.Parameters.AddWithValue("@search", searchParameters);
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    noteslist.Items.Add(reader.GetString(reader.GetOrdinal("title")));

                }
            }
        }

        private void clearSearch_Click(object sender, EventArgs e)
        {
            textSearch.Text = "";
            noteslist.Items.Clear();
        }

        private void textSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton.PerformClick();
            }
        }

        private void deleteNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteNotes frmDel = new DeleteNotes();
            this.Hide();
            frmDel.Show();
        }

        private void setDatabaseFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            folderBrowserDialog1.ShowDialog();
            folderChoice = folderBrowserDialog1.SelectedPath;
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["dbFolder"].Value = folderChoice.ToString();
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"];
        }
    }
}
