using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NLCS
{
    class EditProcess
    {
        private ClsConnection conn;
        private int code;

        public void resetField(ref Button btnAdd, ref Button btnEdit, ref Button btnDel, ref Button btnSave, ref TextBox txtName, ref TextBox txtCode, ref bool selectededit, bool t)
        {
            try
            {
                btnAdd.Enabled = t;
                btnEdit.Enabled = t;
                btnDel.Enabled = t;
                btnSave.Enabled = !t;
                txtCode.Clear();
                txtName.Clear();
                txtCode.Enabled = false;
                txtName.Enabled = !t;
                selectededit = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    int max_u = max; //-7
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
            }
            catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            
        }

        public int updateCodeWard()
        {
            return updateCode("MAX(maphuong)", "PHUONG");
        }

        public int updateCodeStreet()
        {
            return updateCode("MAX(maduong)", "DUONG");
        }

        public int updateCodeListNode()
        {
            return updateCode("MAX(id)", "DANHSACHDIADIEM");
        }

        private void grdView(string table, string columnsort, ref DataGridView grd)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "SELECT * FROM " + table + " order by " + columnsort + " asc";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn.Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "PHUONG");
                grd.DataSource = ds.Tables["PHUONG"];
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void grdWard(ref DataGridView grd)
        {
            grdView("PHUONG", "maphuong", ref grd);
        }

        public void grdStreet(ref DataGridView grd)
        {
            grdView("DUONG", "maduong", ref grd);
        }

        private void add(string table, string name, ref TextBox txt)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "INSERT INTO " + table + " VALUES (@code, @name)";
                SqlParameter p1 = new SqlParameter("@code", SqlDbType.Int);
                p1.Value = txt.Text;
                SqlParameter p2 = new SqlParameter("@name", SqlDbType.NVarChar);
                p2.Value = name;
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.Parameters.Add(p1);
                com.Parameters.Add(p2);
                com.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception)
            {
                MessageBox.Show("Thêm không thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void addWard(string name,ref TextBox txt)
        {
            add("PHUONG", name, ref txt);
        }

        public void addStreet(string name, ref TextBox txt)
        {
            add("DUONG", name, ref txt);
        }

        private void edit(string table, string datasetcolumn,string conditionconlumn,  string name, ref TextBox txt)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "UPDATE " + table + " SET " + datasetcolumn + " = @name WHERE  " + conditionconlumn + " = @code";
                SqlParameter p1 = new SqlParameter("@code", SqlDbType.Int);
                p1.Value = txt.Text;
                SqlParameter p2 = new SqlParameter("@name", SqlDbType.NVarChar);
                p2.Value = name;
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.Parameters.Add(p1);
                com.Parameters.Add(p2);
                com.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception)
            {
                MessageBox.Show("Sửa không thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void editWard(string name, ref TextBox txt) {
            edit("PHUONG", "tenphuong", "maphuong", name, ref txt);
        }

        public void editStreet(string name, ref TextBox txt)
        {
            edit("DUONG", "tenduong", "maduong", name, ref txt);
        }

        private void del(string table, string conditioncolumn1, string conditioncolumn2, ref DataGridView grd)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                List<int> list = new List<int>();
                int row = grd.CurrentCell.RowIndex;
                int codeward = Convert.ToInt32(grd.Rows[row].Cells[0].Value);
                string nameward = grd.Rows[row].Cells[1].Value.ToString();
                string sql1 = "SELECT IDDIACHI FROM DIACHI WHERE " + conditioncolumn1 + " = @id";
                string sql2 = "DELETE FROM DIACHI WHERE " + conditioncolumn1 + " = " + codeward;
                string sql3 = "DELETE FROM " + table + " WHERE " + conditioncolumn2 + " = " + codeward;
                SqlCommand com1 = new SqlCommand(sql1, conn.Conn);
                SqlCommand com2 = new SqlCommand(sql2, conn.Conn);
                SqlCommand com3 = new SqlCommand(sql3, conn.Conn);
                SqlParameter p1 = new SqlParameter("@id", SqlDbType.Int);
                p1.Value = codeward;
                com1.Parameters.Add(p1);
                SqlDataReader rd = com1.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Convert.ToInt32(rd[0]));
                }
                rd.Close();
                foreach (int item in list)
                {
                    string sql4 = "DELETE FROM QUANGDUONG WHERE diachi1 = @idaddress OR diachi2 = @idaddress";
                    SqlCommand com4 = new SqlCommand(sql4, conn.Conn);
                    SqlParameter p2 = new SqlParameter("@idaddress", SqlDbType.Int);
                    p2.Value = item;
                    com4.Parameters.Add(p2);
                    com4.ExecuteNonQuery();
                }
                list.Clear();
                com2.ExecuteNonQuery();
                com3.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }
            catch (Exception)
            {
                MessageBox.Show("Xóa không thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void delWard(ref DataGridView grd)
        {
            del("PHUONG", "phuong", "maphuong", ref grd);
        }

        public void delStreet(ref DataGridView grd)
        {
            del("DUONG", "duong", "maduong", ref grd);
        }
    }
}
