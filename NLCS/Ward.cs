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
    public partial class Ward : Form
    {
        private int code;
        private bool add;
        private bool selectedit;
        EditProcess wards;

        public Ward()
        {
            InitializeComponent();
        }

        void resetFieldWard(bool t)
        {
            wards = new EditProcess();
            wards.resetField(ref btnAdd, ref btnEdit, ref btnDel, ref btnSave, ref txtName, ref txtCode, ref selectedit, t);
        }

        private void Ward_Load(object sender, EventArgs e)
        {
            try
            {
                wards = new EditProcess();
                resetFieldWard(true);
                wards.grdWard(ref grdWard);
                wards.updateCodeWard();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            wards = new EditProcess();
            resetFieldWard(false);
            code = wards.updateCodeWard();
            code++;
            txtCode.Text = code.ToString();
            add = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            wards = new EditProcess();
            resetFieldWard(false);
            selectedit = true;
            txtCode.Text = code.ToString();
            add = false;
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void grdWard_SelectionChanged(object sender, EventArgs e)
        {
            int row = grdWard.CurrentCell.RowIndex;
            if(selectedit)
            {
                this.txtCode.Text = grdWard.Rows[row].Cells[0].Value.ToString();
                this.txtName.Text = grdWard.Rows[row].Cells[1].Value.ToString();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            wards = new EditProcess();
            resetFieldWard(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                wards = new EditProcess();
                if (add)
                {
                    wards.addWard(txtName.Text, ref txtCode);
                }else
                {
                    wards.editWard(txtName.Text, ref txtCode);
                    
                }
                resetFieldWard(true);
                wards.grdWard(ref grdWard);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            wards = new EditProcess();
            wards.delWard(ref grdWard);
            wards.grdWard(ref grdWard);
        }

        private void Ward_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainAdmin m = new MainAdmin();
            m.Enabled = true;
        }
    }
}
