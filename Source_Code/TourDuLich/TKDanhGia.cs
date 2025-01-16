using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
namespace DanhGia_DiemDen
{
    public partial class TKDanhGia : Form
    {
        DataTable dt;
        SqlConnection conn;
        SqlDataAdapter adp;
        KetNoi kn = new KetNoi();
        SqlCommand cmd;
        public TKDanhGia()
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
                    guna2HtmlLabel5.Width += 1;
                    guna2HtmlLabel6.Width += 1;
                    guna2HtmlLabel7.Width += 1;

                }
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có muốn thoát", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes) { this.Close(); }
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            
        }
        void load()
        {
            string sql = "select * from DANHGIA";
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new  SqlDataAdapter(sql, conn);
            adp.Fill(dt);
            DTGV.DataSource = dt;
            DTGV.RowTemplate.Height = 50;
        }
        private void TKDanhGia_Load(object sender, EventArgs e)
        {
            load();
        }

        private void DTGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            txt_matour.Text = DTGV.Rows[index].Cells[1].Value.ToString();
            DT_ngg.Text = DTGV.Rows[index].Cells[2].Value.ToString();
            txt_makh.Text = DTGV.Rows[index].Cells[3].Value.ToString();
            Rate.Value = int.Parse(DTGV.Rows[index].Cells[4].Value.ToString());
            txt_madg.Text = DTGV.Rows[index].Cells[0].Value.ToString();
            guna2TextBox_maLT.Text = DTGV.Rows[index].Cells[5].Value.ToString();
            guna2TextBox_MAHD.Text = DTGV.Rows[index].Cells[6].Value.ToString();
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                DateTime ngay = DateTime.Parse(DT_ngg.Text);
                string sql = "EXEC SUA_DANHGIA '" + txt_madg.Text + "','" + txt_matour.Text + "','" + ngay.ToString("yyyy-MM-dd") + "','" + txt_makh.Text + "'," + Rate.Value + ", '" + guna2TextBox_maLT.Text + "', '"+guna2TextBox_MAHD.Text+"'";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }

        }

        private void btn_xóa_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                DateTime ngay = DateTime.Parse(DT_ngg.Text);
                string sql = "EXEC XOA_DANHGIA '" + txt_madg.Text + "'";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
