using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace NLCS
{
    class EditProcess3
    {
        private ClsConnection conn;
        public void findPath(ref DataGridView grdNode, ref RichTextBox rtxtResult)
        {
            try
            {
                List<int> list = new List<int>();
                this.getData(grdNode, ref list);
                string str = "";
                for (int i = 0; i < list.Count; i++)
                    if (i != list.Count - 1)
                    {
                        str += list[i] + ", ";
                    }
                    else
                    {
                        str += list[i];
                    }
                Graph G = new Graph(list.Count);
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "select * from QUANGDUONG where diachi1 in (" + str + ") and diachi2 in (" + str + ")";
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    int vertice1 = Convert.ToInt32(rd[1]);
                    int vertice2 = Convert.ToInt32(rd[2]);
                    int weigh = Convert.ToInt32(rd[3]);
                    G.addEdge(list.IndexOf(vertice1) + 1, list.IndexOf(vertice2) + 1, weigh);
                }
                Graph T = new Graph(G.Vertices);
                int sum_w = 0;
                Prim p = new Prim(G, ref T, ref sum_w, 1);
                this.printInfoGraph(ref rtxtResult, T, list, sum_w);
                conn.CloseDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   

        public void addNode(DataGridView grd)
        {
            try
            {
                int row = grd.CurrentCell.RowIndex;
                int iddiachi = Convert.ToInt32(grd.Rows[row].Cells[0].Value);
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "INSERT INTO DANHSACHDIADIEM VALUES ( " + iddiachi + ");";
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                conn.CloseDB();
            }catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void grdView(ref DataGridView grd1, ref DataGridView grd2)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "select idDiachi , sonha , tenduong, tenphuong from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong order by idDiaChi asc;";
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds, "DIACHI");
                grd1.DataSource = ds.Tables["DIACHI"];
                string sql2 = "select idDiachi, sonha , tenduong, tenphuong from DANHSACHDIADIEM dd join DIACHI dc on dd.iddiadiem = dc.idDiachi join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong;";
                SqlCommand com2 = new SqlCommand(sql2, conn.Conn);
                SqlDataAdapter da2 = new SqlDataAdapter(com2);
                DataSet ds2 = new DataSet();
                da2.Fill(ds2, "DACHSACHDIADIEM");
                grd2.DataSource = ds2.Tables["DACHSACHDIADIEM"];
                conn.CloseDB();
            }catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void getData(DataGridView grd, ref List<int> list) 
        {
            try
            {
                for (int i = 0; i < grd.RowCount - 1; i++)
                {
                    list.Add(Convert.ToInt32(grd.Rows[i].Cells[0].Value));
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void printInfoGraph(ref RichTextBox txt, Graph G, List<int> list, int sum_w)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string str = "";
                int[,] GA = G.getGraph();
                int[] parent = new int[G.Vertices + 1];
                List<int[]> list_edge = new List<int[]>();
                printInfoGraphS(GA, G,ref list_edge, 0, ref parent, 0);
                for (int i = 0; i < list_edge.Count; i++)
                {
                    int start = list[list_edge[i][0]];
                    int end = list[list_edge[i][1]];
                    int sum = list_edge[i][2];
                    string sqli = "select CONCAT('(', idDiachi,') ', sonha ,', ', tenduong, ', ',tenphuong) as DIACHI from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong where idDiaChi = " + start;
                    string sqlj = "select CONCAT('(', idDiachi,') ', sonha ,', ', tenduong, ', ',tenphuong) as DIACHI from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong where idDiaChi = " + end;
                    SqlCommand com1 = new SqlCommand(sqli, conn.Conn);
                    SqlCommand com2 = new SqlCommand(sqlj, conn.Conn);
                    str = str + com1.ExecuteScalar() + "  <->  " + com2.ExecuteScalar() + ": " + sum + "km \n\n";
                }
                if(sum_w > 666666)
                {
                    txt.Text = "Không tìm được đường đi";
                }
                else
                {
                    txt.Text = "Tổng độ dài: " + sum_w + "km\n" + str;
                }
                conn.CloseDB();
            }catch(Exception) {
                MessageBox.Show("Có lỗi xảy ra!!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void printInfoGraphS(int[,] GA, Graph G,ref  List<int[]> list_edge, int begin, ref int[] parent, int p)
        {
            List<int> list_neighbor = new List<int>();
            int root = begin + 1;
            G.findNeighbor(ref list_neighbor, root);
            parent[root] = p;
            for (int j = 0; j < list_neighbor.Count; j++)
            {
                int y = list_neighbor[j];
                if (y != parent[root])
                {
                    int[] arr = new int[3];
                    arr[0] = root - 1;
                    arr[1] = y - 1;
                    arr[2] = GA[root, y];
                    list_edge.Add(arr);
                    printInfoGraphS(GA, G, ref list_edge, y - 1, ref parent, root);
                }
            }
        }
        public void delNode(ref DataGridView grd)
        {
            try{
                int row = grd.CurrentCell.RowIndex;
                int id = Convert.ToInt32(grd.Rows[row].Cells[0].Value);
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "DELETE FROM DANHSACHDIADIEM WHERE iddiadiem = " + id;
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.CloseDB();
            }catch(Exception ex)
            {
                MessageBox.Show("Xóa không thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void delAllNode(ref DataGridView grd)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "DELETE FROM DANHSACHDIADIEM";
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                conn.CloseDB();
            }catch(Exception ex)
            {
                MessageBox.Show("Xóa không thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void searchAddress(TextBox txtSearch, ComboBox cboSelectionSearch, ref DataGridView grd)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "select idDiachi , sonha , tenduong, tenphuong from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong ";
                if(cboSelectionSearch.SelectedIndex == 0)
                {
                    sql = sql + "where iddiachi like '%" + txtSearch.Text + "%';";
                }else if (cboSelectionSearch.SelectedIndex == 1)
                {
                    sql = sql + "where sonha like '%" + txtSearch.Text + "%';";
                }else if (cboSelectionSearch.SelectedIndex == 2)
                {
                    sql = sql + "where tenduong like N'%" + txtSearch.Text + "%';";
                }else
                {
                    sql = sql + "where tenphuong like N'%" + txtSearch.Text + "%';";
                }
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds, "DIACHI2");
                grd.DataSource = ds.Tables["DIACHI2"];
                conn.CloseDB();
            }catch(Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void undoAddress(ref DataGridView grd)
        {
            try
            {
                conn = new ClsConnection();
                conn.OpenDB();
                string sql = "select idDiachi , sonha , tenduong, tenphuong from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong order by idDiaChi asc;";
                SqlCommand com = new SqlCommand(sql, conn.Conn);
                com.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds, "DIACHI");
                grd.DataSource = ds.Tables["DIACHI"];
                conn.CloseDB();
            }catch( Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void findPath2(int start, int end, RichTextBox txtResult)
        {
            List<int> list = new List<int>();          
            conn = new ClsConnection();
            conn.OpenDB();
            string sql_diachi = "SELECT idDiachi from DIACHI";
            SqlCommand com_sql_diachi = new SqlCommand(sql_diachi, conn.Conn);
            SqlDataReader rd_diachi = com_sql_diachi.ExecuteReader();
            while (rd_diachi.Read())
            {
                list.Add(Convert.ToInt32(rd_diachi[0]));
            }
            rd_diachi.Close();
            Graph G = new Graph(Convert.ToInt32(list.Count));
            string sql = "select * from QUANGDUONG";
            SqlCommand com = new SqlCommand(sql, conn.Conn);
            SqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
            {
                int vertice1 = Convert.ToInt32(rd[1]);
                int vertice2 = Convert.ToInt32(rd[2]);
                int weigh = Convert.ToInt32(rd[3]);
         //       MessageBox.Show(vertice1.ToString() + vertice2 + weigh);
                G.addEdge(list.IndexOf(vertice1) + 1, list.IndexOf(vertice2) + 1, weigh);
            }
            rd.Close();
            Dijsktra d = new Dijsktra(G, start);
            List<int[]> list2 = new List<int[]>();
            d.Print(end, ref list2);
            string str__ = "";
            int sum_w = d.getPiEnd(end);
            for (int i = list2.Count - 1; i >= 0; i--)
            {
                if(list2[i][0] != -1)
                {
                    int start_ = list[list2[i][0] - 1];
                    int end_ = list[list2[i][1] - 1];
                    string sqli = "select CONCAT('(', idDiachi,') ', sonha ,', ', tenduong, ', ',tenphuong) as DIACHI from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong where idDiaChi = " + start_;
                    string sqlj = "select CONCAT('(', idDiachi,') ', sonha ,', ', tenduong, ', ',tenphuong) as DIACHI from DIACHI dc join PHUONG p on p.maphuong = dc.phuong join DUONG d on d.maduong = dc.duong where idDiaChi = " + end_;
                    SqlCommand com1 = new SqlCommand(sqli, conn.Conn);
                    SqlCommand com2 = new SqlCommand(sqlj, conn.Conn);
                    str__ = str__ + com1.ExecuteScalar() + "  <->  " + com2.ExecuteScalar() + ": " + "km \n\n";
                }
            }
            if (sum_w > 666666)
            {
                txtResult.Text = "Không tìm được đường đi";
            }
            else
            {
                txtResult.Text = "Tổng độ dài: " + sum_w + "km\n" + str__;
            }


        }
    }
}
