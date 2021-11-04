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

namespace NLCS
{
    public partial class frmLogin : Form
    {
        ClsConnection conn;
        public frmLogin()
        {
            InitializeComponent();
        }

        public void checkAccount()
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "SELECT COUNT(*) FROM USERS WHERE ID = " + txtName.Text + "AND PWD = " + txtPwd.Text;
                SqlCommand com = new SqlCommand(sql, conn.Conn);

                int count = Convert.ToInt32(com.ExecuteScalar());
                if (count == 1)
                {
                    string sql2 = "SELECT TYPE FROM USERS WHERE ID = " + txtName.Text + "AND PWD = " + txtPwd.Text;
                    SqlCommand com2 = new SqlCommand(sql2, conn.Conn);
                    string type = com2.ExecuteScalar().ToString();
                    string sql3 = "SELECT PHONE, NAME FROM USERS WHERE ID = " + txtName.Text + "AND PWD = " + txtPwd.Text;
                    SqlCommand com3 = new SqlCommand(sql3, conn.Conn);
                    SqlDataReader rd = com3.ExecuteReader();
                    string phone = "", name = "";
                    while (rd.Read())
                    {
                        phone = rd[0].ToString();
                        name = rd[1].ToString();
                    }
                    rd.Close();
                    lblFalse.Text = "";
                    if (type == "admin")
                    {
                        MainAdmin main = new MainAdmin(txtName.Text, phone, name);
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        frmUsers main = new frmUsers(txtName.Text, phone, name);
                        main.Show();
                        this.Hide();
                    }
                }
                else
                {
                    lblFalse.Text = "Mã đăng nhập hoặc mật khẩu sai!!";
                }
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.checkAccount();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            

        }
    }
}
