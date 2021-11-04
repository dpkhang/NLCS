using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NLCS
{
    class ClsConnection
    {
        private SqlConnection conn;

        public SqlConnection Conn
        {
            set { this.conn = value; }
            get { return conn; }
        }

        public  bool OpenDB()
        {
            try
            { 
                conn = new SqlConnection("Data Source=DESKTOP-R35MBIO\\SQLEXPRESS; Initial Catalog=DIALY; Integrated Security=True");
                conn.Open();
                return true;
            }
            catch(Exception)
            {
                _ = MessageBox.Show("Không thể kết nối Database", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public  bool CloseDB() {
            try
            {
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
