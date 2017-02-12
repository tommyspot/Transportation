using System;
using System.Windows.Forms;
using System.Data.OleDb;


namespace ShoreSweep
{
    public partial class User : Form
    {
        OleDbConnection con = null;
        public User()
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
                String user = txt_user.Text.ToString();
                String pass = txt_pass.Text.ToString();
                bool exists = false;
                OleDbCommand cmd = new OleDbCommand();

                String queryUser = "select count(*) from [shore] where user = @user";
                cmd.Connection = conn;
                cmd.CommandText = queryUser;
                cmd.Parameters.AddWithValue("user", txt_user.Text);
                exists = (int)cmd.ExecuteScalar() > 0;
                // if exists, show a message error
                if (exists)
                {
                    MessageBox.Show(txt_user.Text + "  " + "This username has been using by another user.");
                    return;
                }
                else
                {
                    String my_querry = "INSERT INTO shore ([user],[pass])" + " VALUES (@user,@pass)";


                    cmd.Connection = conn;
                    cmd.CommandText = my_querry;
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("User created successfuly...!");
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
                txt_pass.Text = "";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           // pictureBox1.Image = ImageUtil.ResizeImage(ImageUtil.LoadPicture("https://farm1.staticflickr.com/2/1418878_1e92283336_m.jpg"), pictureBox1, true);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
