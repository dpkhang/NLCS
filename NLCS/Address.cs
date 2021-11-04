using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NLCS
{
    public partial class Address : Form
    {
        private EditProcessAdress add;
        private bool selectedit;
        private int code;
        private bool addaddress;
        public Address()
        {
            InitializeComponent();
        }

        private void cboStreet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        void resetFieldAdd(bool t)
        {
            add = new EditProcessAdress();
            add.resetField(ref btnAdd, ref btnEdit, ref btnDel, ref btnSave, ref txtAdd, ref txtCode, ref cboWard, ref cboStreet, ref selectedit, t);
        }

        private void Address_Load(object sender, EventArgs e)
        {
            add = new EditProcessAdress();
            add.addCboWard(ref cboWard);
            add.addCboStreet(ref cboStreet);
            add.grdAdd(ref grdAdd);
            resetFieldAdd(true);
        }

        private void grdAdd_SelectionChanged(object sender, EventArgs e)
        {
            int row = grdAdd.CurrentCell.RowIndex;
            if (selectedit)
            {
                this.txtCode.Text = grdAdd.Rows[row].Cells[0].Value.ToString();
                this.txtAdd.Text = grdAdd.Rows[row].Cells[1].Value.ToString();
                this.cboStreet.Text = grdAdd.Rows[row].Cells[2].Value.ToString();
                this.cboWard.Text = grdAdd.Rows[row].Cells[3].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            add = new EditProcessAdress();
            resetFieldAdd(false);
            code = add.updateCodeAddress();
            code++;
            txtCode.Text = code.ToString();
            addaddress = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            add = new EditProcessAdress();
            resetFieldAdd(false);
            selectedit = true;
            addaddress = false;
            txtCode.Text = code.ToString();
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            resetFieldAdd(true);
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            add = new EditProcessAdress();
            if (addaddress)
            {
                add.addAddress(ref txtCode,ref txtAdd,ref cboWard,ref cboStreet);
            }else
            {
                add.editAdress(ref txtCode, ref txtAdd, ref cboStreet, ref cboWard);
            }
            resetFieldAdd(true);
            add.grdAdd(ref grdAdd);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            add = new EditProcessAdress();
            add.delDiaChi(ref grdAdd);
            add.grdAdd(ref grdAdd);
        }
    }
}
