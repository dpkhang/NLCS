using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NLCS
{
    public partial class Distance : Form
    {
        List<int> list;
        private EditProcessAdress dis;
        private bool selectedit;
        private int code;
        private bool adddis;
        public Distance()
        {
            InitializeComponent();
        }

        private void grdAdd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        void resetFieldDis(bool t)
        {
            dis = new EditProcessAdress();
            dis.resetField_Distance(ref btnAdd, ref btnEdit, ref btnDel,ref btnSave, ref txtCode, ref txtLengh, ref cboBegin, ref cboEnd, ref selectedit, t);
        }

        private void Distance_Load(object sender, EventArgs e)
        {
            dis = new EditProcessAdress();
            dis.addCboDistance(ref cboBegin, ref cboEnd);
            resetFieldDis(true);
            dis.grdDis(ref grdDistance);
        }

        private void txtAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dis = new EditProcessAdress();
            resetFieldDis(false);
            code = dis.updateCodeDis();
            code++;
            txtCode.Text = code.ToString();
            adddis = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dis = new EditProcessAdress();
            if (adddis)
            {
                dis.addDistance(ref cboBegin,ref cboEnd,ref txtCode,ref txtLengh);
            }
            else
            {
                dis.editDistance(ref txtCode, ref txtLengh, ref cboBegin, ref cboEnd);
            }
            resetFieldDis(true);
            dis.grdDis(ref grdDistance);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            dis = new EditProcessAdress();
            dis.delDis(ref grdDistance);
            dis.grdDis(ref grdDistance);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            resetFieldDis(true);
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dis = new EditProcessAdress();
            resetFieldDis(false);
            selectedit = true;
            adddis = false;
            txtCode.Text = code.ToString();
        }

        private void grdDistance_SelectionChanged(object sender, EventArgs e)
        {
            int row = grdDistance.CurrentCell.RowIndex;
            if (selectedit)
            {
                this.txtCode.Text = grdDistance.Rows[row].Cells[0].Value.ToString();
                this.cboBegin.Text = grdDistance.Rows[row].Cells[1].Value.ToString();
                this.cboEnd.Text = grdDistance.Rows[row].Cells[2].Value.ToString();
                this.txtLengh.Text = grdDistance.Rows[row].Cells[3].Value.ToString();
            }
        }
    }
}
