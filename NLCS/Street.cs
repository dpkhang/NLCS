using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NLCS
{
    public partial class Street : Form
    {
        private int code;
        private bool add;
        private bool selectedit;
        EditProcess street;

        public Street()
        {
            InitializeComponent();
        }

        void resetFieldStreet(bool t)
        {
            street = new EditProcess();
            street.resetField(ref btnAdd, ref btnEdit, ref btnDel, ref btnSave, ref txtName, ref txtCode, ref selectedit, t);
        }

        private void Street_Load(object sender, EventArgs e)
        {
            try
            {
                street = new EditProcess();
                resetFieldStreet(true);
                street.grdStreet(ref grdStreet);
                street.updateCodeStreet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            street = new EditProcess();
            resetFieldStreet(false);
            code = street.updateCodeStreet();
            code++;
            txtCode.Text = code.ToString();
            add = true;
        }

        private void grdStreet_SelectionChanged(object sender, EventArgs e)
        {
            int row = grdStreet.CurrentCell.RowIndex;
            if (selectedit)
            {
                this.txtCode.Text = grdStreet.Rows[row].Cells[0].Value.ToString();
                this.txtName.Text = grdStreet.Rows[row].Cells[1].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            street = new EditProcess();
            resetFieldStreet(false);
            selectedit = true;
            txtCode.Text = code.ToString();
            add = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                street = new EditProcess();
                if (add)
                {
                    street.addStreet(txtName.Text, ref txtCode);
                }
                else
                {
                    street.editStreet(txtName.Text, ref txtCode);

                }
                resetFieldStreet(true);
                street.grdStreet(ref grdStreet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            street = new EditProcess();
            street.delStreet(ref grdStreet);
            street.grdStreet(ref grdStreet);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            street = new EditProcess();
            resetFieldStreet(true);
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
