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
    public partial class Login : Form
    {
        OleDbConnection con = null;
        public Login()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            string str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=ShoreSweep.mdb";
            // Paste your connection string that you copy from your database Properties.
            con = new OleDbConnection(str);
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM shore WHERE user = '" + txt_user.Text + "' AND pass = '" + txt_pass.Text + "'", con);
            con.Open();
            try
            {
                int i;
                i = Convert.ToInt32(cmd.ExecuteScalar());
                if (i == 1)
                {
                    this.Hide();
                    Form1 objFrm = new Form1();
                    objFrm.Show();
                   
                }
                else
                {
                    MessageBox.Show("Invalid User Name or Password");
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
