using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TheArtOfDevHtmlRenderer.Adapters;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using Guna.UI2.WinForms;

namespace DanhGia_DiemDen
{
    public partial class DiemDen : Form
    {
        DataTable dt;
        SqlConnection conn;
        SqlDataAdapter adp;
        KetNoi kn=new KetNoi();
        SqlCommand cmd;
        public DiemDen()
        {
            InitializeComponent();
            conn = kn.conn;
            AdjustFormSize();
        }

        float currentDpi;
        private void AdjustFormSize()
        {
            using (Graphics g = this.CreateGraphics())
            {
                currentDpi = g.DpiX; // DPI hiện tại
                if (currentDpi == 96)
                {
             
                    guna2HtmlLabel1.Width += 1;
                    guna2HtmlLabel2.Width += 1;
                    guna2HtmlLabel3.Width += 1;
                    guna2HtmlLabel4.Width += 1;
                    

                }
            }
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {
            load();
            
        }
        void load()
        {
            string sql = "select * from diemden";
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(sql, conn);
            adp.Fill(dt);
            DTGV.DataSource = dt;
            DTGV.RowTemplate.Height = 50;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load();

            sl();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có muốn thoát","Thoát",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
            if(r == DialogResult.Yes) { this.Close(); }
        }

        private void DTGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            txt_mdd.Text = DTGV.Rows[index].Cells[0].Value.ToString();
            txt_tendd.Text = DTGV.Rows[index].Cells[1].Value.ToString();
            txt_dc.Text = DTGV.Rows[index].Cells[2].Value.ToString();
        }

        private void DTGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        void them()
        {
            // Biến handler để quản lý sự kiện
            SqlInfoMessageEventHandler infoMessageHandler = (s, eArgs) =>
            {
                // Hiển thị từng thông báo từ SQL Server
                foreach (SqlError error in eArgs.Errors)
                {
                    MessageBox.Show("Thông báo từ SQL: " + error.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            try
            {
                conn.InfoMessage += infoMessageHandler;
                conn.Open();
                string sql = "Exec THEMDIEMDEN '" + txt_mdd.Text + "',N'" + txt_tendd.Text + "',N'" + txt_dc.Text + "'";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                load();

            }
            catch (SqlException ex) { MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.InfoMessage -= infoMessageHandler; conn.Close(); }
            sl();
        }
        void xoa()
        {
            try
            {
                conn.Open();
                string sql = "Exec XOADIEMDEN '" + txt_mdd.Text + "'";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load();

            }
            catch (Exception ex) { MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }
            sl();
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            them();
            
        }
        void sua()
        {
            try
            {
                conn.Open();
                string sql = "Exec CAPNHATDIEMDEN '" + txt_mdd.Text + "',N'" + txt_tendd.Text + "',N'" + txt_dc.Text + "'";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load();

            }
            catch (Exception ex) { MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }
        }
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            xoa();
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            sua();
        }
        void sl()
        {
            conn.Open();
            string sql = "select count(*) from DIEMDEN";
            cmd = new SqlCommand(sql, conn);
            int a = (int)cmd.ExecuteScalar();
            txt_sl.Text = a.ToString();
            conn.Close();
        }
        private void txt_sl_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
