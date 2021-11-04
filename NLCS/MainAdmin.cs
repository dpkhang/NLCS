using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NLCS
{
    public partial class MainAdmin : Form
    {
        public MainAdmin()
        {
            InitializeComponent();
        }

        public MainAdmin(string code, string phone, string name)
        {
            InitializeComponent();
            this.lblCode.Text = code;
            this.lblPhone.Text = phone;
            this.lblName.Text = name;
        }

        private void MainAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainAdmin_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWard_Click(object sender, EventArgs e)
        {
            Ward w = new Ward();
            w.Show();
        }

        private void btnStr_Click(object sender, EventArgs e)
        {
            Street s = new Street();
            s.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Address a = new Address();
            a.Show();
        }

        private void btnDis_Click(object sender, EventArgs e)
        {
            Distance d = new Distance();
            d.Show();
        }
    }
}
