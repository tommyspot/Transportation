using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ShoreSweep
{
    public partial class frmAddVolunteer : Form
    {
        public frmAddVolunteer()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            string str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=ShoreSweep.mdb";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
            conn.ConnectionString = str;

            try
            {

                conn.Open();
                String FullName = txt_user.Text.ToString();
             
                bool exists = false;
                OleDbCommand cmd = new OleDbCommand();

                String queryUser = "select count(*) from [assignee] where FullName = @FullName";
                cmd.Connection = conn;
                cmd.CommandText = queryUser;
                cmd.Parameters.AddWithValue("FullName", txt_user.Text);
                exists = (int)cmd.ExecuteScalar() > 0;
                // if exists, show a message error
                if (exists)
                {
                    MessageBox.Show(txt_user.Text + "  " + "This username has been using by another user.");
                    return;
                }
                else
                {
                    String my_querry = "INSERT INTO assignee ([FullName])" + " VALUES (@FullName)";


                    cmd.Connection = conn;
                    cmd.CommandText = my_querry;
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                   
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Assignee created successfuly...!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed due to" + ex.Message);
            }
            finally
            {
                conn.Close();
                txt_user.Text = "";
               
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
