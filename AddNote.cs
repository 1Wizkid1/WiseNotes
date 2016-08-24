using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CSWiki_DB
{
    public partial class AddNote : Form
    {
        static string dbLoc;
        static string notesLoc;
        public static string connString;
        public static SqlConnection connect;
        public static SqlCommand myCommand;
        public static SqlDataReader reader;
        public static string fileName;
        
        public AddNote()
        {
            InitializeComponent();
        }

        private void AddNote_Load(object sender, EventArgs e)
        {
            dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"] + @"\wisenotes.mdf";
            notesLoc = System.Configuration.ConfigurationManager.AppSettings["notesFolder"];
            connString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=" + dbLoc;
            connect = new SqlConnection(connString);
            connect.Open();
            myCommand = connect.CreateCommand();
            loadcategories();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = textBox2.Text;
            string fileloc = notesLoc;
            string filepath = fileloc + filename + '"';
            string category = catlist.SelectedItem.ToString();
            string title = notetitle.Text;


            myCommand.Parameters.Clear();
            myCommand.CommandText = @"INSERT INTO dbo.[notes] (title, category, filepath) VALUES ('" + title + "', '" + category + "', '" + filepath + "') ;";
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    
                }
                reader.Close();
                MessageBox.Show("Your new note has been created");
            }
            
        }

        private void loadcategories()
        {
            myCommand.CommandText = @"SELECT * FROM catfolder ORDER by Convert(varchar(max),categories)";
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    catlist.Items.Add(reader.GetString(reader.GetOrdinal("categories")));
                }
            }
        }

        private void AddNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            connect.Close();
            Form1 frm1 = new Form1();
            
            frm1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            

            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = notesLoc;
            // Initialize the SaveFileDialog to specify the RTF extention for the file.
            openFileDialog1.DefaultExt = "*.rtf";
            openFileDialog1.Filter = "RTF Files|*.rtf";

            openFileDialog1.ShowDialog();

           
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            fileName = openFileDialog1.FileName;
            fileName = fileName.Replace(notesLoc, "");
            textBox2.Text = fileName;
        }
    }
}
