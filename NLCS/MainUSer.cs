using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace NLCS
{
    public partial class frmUsers : Form
    {
        EditProcess3 edit3;
        public frmUsers()
        {
            InitializeComponent();
        }

        public frmUsers(string code, string phone, string name)
        {
            InitializeComponent();
            this.lblCode.Text = code;
            this.lblPhone.Text = phone;
            this.lblName.Text = name;
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.delAllNode(ref grdNode);
            edit3.grdView(ref grdSearch, ref grdNode);
            cboSelectionSearch.SelectedIndex = 0;
        }

        private void frmUsers_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            edit3 = new EditProcess3();
            edit3.delAllNode(ref grdNode);
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.findPath(ref grdNode, ref rtxtResult);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.delNode(ref grdNode);
            edit3.grdView(ref grdSearch, ref grdNode);
        }

        private void grdSearch_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.addNode(grdSearch);
            edit3.grdView(ref grdSearch, ref grdNode);
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.delAllNode(ref grdNode);
            edit3.grdView(ref grdSearch, ref grdNode);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.searchAddress(txtSearch, cboSelectionSearch, ref grdSearch);
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.undoAddress(ref grdSearch);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            edit3 = new EditProcess3();
            edit3.findPath2(Convert.ToInt32(txtnode1.Text), Convert.ToInt32(txtnode2.Text), rtxtResult);
        }
    }
}
