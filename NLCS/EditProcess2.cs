using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NLCS
{
    class EditProcessAdress
    {
        private ClsConnection conn;
        private int code;

        public void resetField(ref Button btnAdd, ref Button btnEdit, ref Button btnDel, ref Button btnSave, ref TextBox txtAdd, ref TextBox txtCode, ref ComboBox cboWard, ref ComboBox cboStreet, ref bool selectededit, bool t)
        {
            try
            {
                btnAdd.Enabled = t;
                btnEdit.Enabled = t;
                btnDel.Enabled = t;
                btnSave.Enabled = !t;
                txtCode.Clear();
                txtAdd.Clear();
                cboWard.SelectedIndex = 0;
                cboStreet.SelectedIndex = 0;
                txtCode.Enabled = false;
                txtAdd.Enabled = !t;
                cboStreet.Enabled = !t;
                cboWard.Enabled = !t;
                selectededit = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void resetField_Distance(ref Button btnAdd, ref Button btnEdit, ref Button btnDel, ref Button btnSave,ref TextBox txtCode, ref TextBox txtLengh, ref ComboBox cboBegin, ref ComboBox cboEnd, ref bool selectededit, bool t)
        {
            try
            {
                btnAdd.Enabled = t;
                btnEdit.Enabled = t;
                btnDel.Enabled = t;
                btnSave.Enabled = !t;
                txtCode.Clear();
                txtLengh.Clear();
                cboBegin.SelectedIndex = 0;
                cboEnd.SelectedIndex = 0;
                txtCode.Enabled = false;
                txtLengh.Enabled = !t;
                cboBegin.Enabled = !t;
                cboEnd.Enabled = !t;
                selectededit = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void addComboBox(string select, string name, string columndisplay, string columnvalue , ref ComboBox cbo)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "SELECT " + select +" FROM " + name + ";";
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds, name);
                cbo.DisplayMember = columndisplay;
                cbo.ValueMember = columnvalue;
                cbo.DataSource = ds.Tables[name];
                conn.CloseDB();
            }catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void addCboWard(ref ComboBox cbo) {
            addComboBox("*", "PHUONG", "tenphuong", "maphuong",ref cbo);
        }

        public void addCboStreet(ref ComboBox cbo)
        {
            addComboBox("*", "DUONG", "tenduong", "maduong", ref cbo);
        }

        public void addCboDistance(ref ComboBox cboBegin, ref ComboBox cboEnd)
        {
            addComboBox("idDiachi, CONCAT(sonha,', ', tenduong,', ', tenphuong) as thongtin", "DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong", "thongtin", "idDiachi", ref cboBegin);
            addComboBox("idDiachi, CONCAT(sonha,', ', tenduong,', ', tenphuong) as thongtin", "DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong", "thongtin", "idDiachi", ref cboEnd);
        }

        private void grdView(string sql, string name, ref DataGridView grd)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn.Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, name);
                grd.DataSource = ds.Tables[name];
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void grdDis(ref DataGridView grd)
        {
            grdView("SELECTQUANGDUONG", "QUANGDUONG", ref grd);
        }

        public void grdAdd(ref DataGridView grd)
        {
            string sql = "select idDiachi , sonha , tenduong, tenphuong from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong order by idDiaChi asc;";
            grdView(sql,"PHUONG", ref grd);
        }

        private int updateCode(string selectcolumn, string table)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "SELECT " + selectcolumn + " FROM " + table;
                string sql2 = "SELECT * FROM " + table;
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                SqlCommand com2 = new SqlCommand(sql2, conn.Conn);
                SqlDataReader rd = com2.ExecuteReader();
                int temp = 0;
                while (rd.Read() && rd[0].ToString() != "")
                    temp = 1;
                rd.Close();
                if (temp == 0)
                {
                    code = 0;
                }
                else
                {
                    int max = Convert.ToInt32(com.ExecuteScalar());
                    int max_u = max;
                    SqlDataReader rd2 = com2.ExecuteReader();
                    max_u = max;
                    for (int i = 1; i <= max; i++)
                    {
                        temp = 0;
                        while (rd2.Read())
                        {
                            if (i == Convert.ToInt32(rd2[0]))
                            {
                                temp = 1;
                                break;
                            }
                        }
                        if (temp != 1 && max_u > i)
                        {
                            max_u = i - 1;
                        }
                    }
                    code = max_u;
                    rd2.Close();
                }
                conn.CloseDB();
                return code;
            }catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        public int updateCodeAddress()
        {
            return updateCode("MAX(idDiaChi)", "DIACHI");
        }

        public int updateCodeDis()
        {
            return updateCode("MAX(id)", "QUANGDUONG");
        }

        private void addPAddress(string table, ref TextBox txt, ref TextBox housenumber, ref ComboBox cboWard, ref ComboBox cboStreet)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "INSERT INTO " + table + " VALUES (@code, @sonha, @duong, @phuong)";
                SqlParameter p1 = new SqlParameter("@code", SqlDbType.Int);
                p1.Value = Convert.ToInt32(txt.Text);
                SqlParameter p2 = new SqlParameter("@sonha", SqlDbType.NVarChar);
                p2.Value = housenumber.Text;
                SqlParameter p3 = new SqlParameter("@duong", SqlDbType.Int);
                p3.Value = Convert.ToInt32(cboStreet.SelectedValue);
                SqlParameter p4 = new SqlParameter("@phuong", SqlDbType.Int);
                p4.Value = Convert.ToInt32(cboWard.SelectedValue);
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.Parameters.Add(p1);
                com.Parameters.Add(p2);
                com.Parameters.Add(p3);
                com.Parameters.Add(p4);
                com.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm không thành công!" , "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void addAddress(ref TextBox txt, ref TextBox housenumber, ref ComboBox cboWard, ref ComboBox cboStreet)
        {
            addPAddress("DIACHI", ref txt, ref housenumber, ref cboWard, ref cboStreet);
        }

        private void addPDistance(string table, ref ComboBox cbo1, ref ComboBox cbo2, ref TextBox txtcode, ref TextBox txtdistance)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "INSERT INTO " + table + " VALUES (@code, @diachi1, @diachi2, @khoangcach)";
                SqlParameter p1 = new SqlParameter("@code", SqlDbType.Int);
                p1.Value = Convert.ToInt32(txtcode.Text);
                SqlParameter p2 = new SqlParameter("@diachi1", SqlDbType.Int);
                p2.Value = Convert.ToInt32(cbo1.SelectedValue);
                SqlParameter p3 = new SqlParameter("@diachi2", SqlDbType.Int);
                p3.Value = Convert.ToInt32(cbo2.SelectedValue);
                SqlParameter p4 = new SqlParameter("@khoangcach", SqlDbType.Float);
                p4.Value = Convert.ToDouble(txtdistance.Text);
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.Parameters.Add(p1);
                com.Parameters.Add(p2);
                com.Parameters.Add(p3);
                com.Parameters.Add(p4);
                com.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm không thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void addDistance(ref ComboBox cbo1, ref ComboBox cbo2, ref TextBox txtcode, ref TextBox txtdistance)
        {
            addPDistance("QUANGDUONG", ref cbo1, ref cbo2, ref txtcode, ref txtdistance);
        }

        private void edit(string _object, ref TextBox txt1, ref TextBox txt2, ref ComboBox cbo1, ref ComboBox cbo2)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                SqlParameter p1, p2, p3, p4;
                SqlCommand com;
                string sql;
                if (_object == "DIACHI")
                {
                    sql = "UPDATE DIACHI SET sonha = @sonha, duong = @duong, phuong = @phuong where idDiaChi = @id";
                    com = new SqlCommand(sql, conn.Conn);
                    p1 = new SqlParameter("@sonha", SqlDbType.VarChar);
                    p2 = new SqlParameter("@duong", SqlDbType.Int);
                    p3 = new SqlParameter("@phuong", SqlDbType.Int);
                    p4 = new SqlParameter("@id", SqlDbType.Int);
                    p1.Value = txt2.Text;
                    p2.Value = cbo1.SelectedValue;
                    p3.Value = cbo2.SelectedValue;
                    p4.Value = txt1.Text;
                    com.Parameters.Add(p1);
                    com.Parameters.Add(p2);
                    com.Parameters.Add(p3);
                    com.Parameters.Add(p4);

                }
                else {
                    sql = "UPDATE QUANGDUONG SET diachi1 = @diachi1, diachi2 = @diachi2, khoangcach = @kc where id = @id";
                    com = new SqlCommand(sql, conn.Conn);
                    p1 = new SqlParameter("@id", SqlDbType.Int);
                    p2 = new SqlParameter("@diachi1", SqlDbType.Int);
                    p3 = new SqlParameter("@diachi2", SqlDbType.Int);
                    p4 = new SqlParameter("@kc", SqlDbType.Float);
                    p1.Value = txt1.Text;
                    p2.Value = cbo1.SelectedValue;
                    p3.Value = cbo2.SelectedValue;
                    p4.Value = txt2.Text;
                    com.Parameters.Add(p1);
                    com.Parameters.Add(p2);
                    com.Parameters.Add(p3);
                    com.Parameters.Add(p4);
                }
                com.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa không thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void editAdress(ref TextBox txtCode, ref TextBox txtAdd, ref ComboBox cboStreet, ref ComboBox cboWard)
        {
            edit("DIACHI", ref txtCode, ref txtAdd, ref cboStreet, ref cboWard);
        }

        public void editDistance(ref TextBox txtCode, ref TextBox txtDis, ref ComboBox cboBegin, ref ComboBox cboEnd)
        {
            edit("QUANGDUONG", ref txtCode, ref txtDis, ref cboBegin, ref cboEnd);
        }

        private void del(ref DataGridView grd)
        {
            try
            {
                int row = grd.CurrentCell.RowIndex;
                int codaaddress = Convert.ToInt32(grd.Rows[row].Cells[0].Value);
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "DELETE FROM QUANGDUONG WHERE DIACHI1 = " + codaaddress + " OR DIACHI2 = " + codaaddress;
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                string sql2 = "DELETE FROM DIACHI WHERE idDiaChi = " + codaaddress;
                SqlCommand com2 = new SqlCommand(sql2, conn.Conn);
                com2.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Xóa không thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void delDiaChi(ref DataGridView grd)
        {
            del(ref grd);
        }

        public void delDis(ref DataGridView grd)
        {
            try
            {
                int row = grd.CurrentCell.RowIndex;
                int codedis = Convert.ToInt32(grd.Rows[row].Cells[0].Value);
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "DELETE FROM QUANGDUONG WHERE id = " + codedis;
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
