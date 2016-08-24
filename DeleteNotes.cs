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
using System.IO;
using System.Security.AccessControl;

namespace CSWiki_DB
{
    public partial class DeleteNotes : Form
    {
        static string dbLoc;
        public static string connString;
        public static SqlConnection connect;
        public static SqlCommand myCommand;
        public static SqlDataReader reader;
        public static string selectedcategory;
        public static string selectednote;

        public DeleteNotes()
        {
            InitializeComponent();
        }

        private void DeleteNotes_Load(object sender, EventArgs e)
        {
            dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"] + @"\wisenotes.mdf";
            connString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=" + dbLoc;
            connect = new SqlConnection(connString);
            connect.Close();
            connect.Open();
            myCommand = connect.CreateCommand();
            loadNotes();
            loadCats();
        }

        private void loadCats()
        {
            myCommand.Parameters.Clear();
            myCommand.CommandText = @"SELECT * FROM catfolder ORDER by Convert(varchar(max),categories)";


            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    catList.Items.Add(reader.GetString(reader.GetOrdinal("categories")));
                }
            }
        }

        private void loadNotes()
        {
            myCommand.Parameters.Clear();
            myCommand.CommandText = @"SELECT * FROM notes ORDER by Convert(varchar(max),title)";

            
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    noteslist.Items.Add(reader.GetString(reader.GetOrdinal("title")));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myCommand.Parameters.Clear();
            myCommand.CommandText = @"DELETE FROM notes WHERE title=@deltitle";
            myCommand.Parameters.AddWithValue("@deltitle", selectednote);


            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {

                }
                MessageBox.Show("Note has been deleted");
            }
            noteslist.Items.Clear();
            catList.Items.Clear();
            loadNotes();
            loadCats();
        }

        private void DeleteNotes_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 frm1 = new Form1();
            this.Hide();
            frm1.Show();
        }

        private void noteslist_Click(object sender, EventArgs e)
        {
            if (noteslist.SelectedItem == null)
            {
                return;
            }

            else
            {
                selectednote = noteslist.SelectedItem.ToString();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myCommand.Parameters.Clear();
            myCommand.CommandText = @"DELETE FROM catfolder WHERE categories=@delcat";
            myCommand.Parameters.AddWithValue("@delcat", selectedcategory);


            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {

                }
                MessageBox.Show("Category has been deleted");
            }
            noteslist.Items.Clear();
            catList.Items.Clear();
            loadNotes();
            loadCats();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (catList.SelectedItem == null)
            {
                return;
            }

            else
            {
                selectedcategory = catList.SelectedItem.ToString();
            }

        }
    }
}
