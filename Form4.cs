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
    public partial class Form4 : Form
    {
        static string dbLoc;
        static string notesLoc;
        public static string connString;
        public static SqlConnection connect;
        public static SqlCommand myCommand;
        public static SqlDataReader reader;

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect.Close();
            connect.Open();
            string catname = newcatname.Text;


            myCommand.Parameters.Clear();
            myCommand.CommandText = @"INSERT INTO dbo.[catfolder] (categories) values ('" + catname + "')";
            myCommand.Parameters.AddWithValue("@input", catname);
            reader = myCommand.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {

                }
                reader.Close();
                MessageBox.Show("Your have created a new category titled: " + catname + "!");
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            dbLoc = System.Configuration.ConfigurationManager.AppSettings["dbFolder"] + @"\wisenotes.mdf";
            notesLoc = System.Configuration.ConfigurationManager.AppSettings["notesFolder"];
            connString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=" + dbLoc;
            connect = new SqlConnection(connString);
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            //connect.Close();
            //connect.Open();
            myCommand = connect.CreateCommand();
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
        }
    }
}
