using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAT_TOUR;
using Guna.UI2.WinForms;
using HOADON;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using WindowsFormsApp1;

namespace TourDuLich
{
    public partial class GiaoDien_For_HDV : Form
    {
        SqlConnection cn;
        KetNoi kn = new KetNoi();
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        public GiaoDien_For_HDV()
        {
            InitializeComponent();
            cn = kn.conn;
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
        string mahdv;
        void loadThongTin()
        {
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                guna2TextBox_mahdv.Text = row[0].ToString();
                mahdv = row[0].ToString();
                guna2TextBox_ten.Text = row[1].ToString();
                guna2TextBox_mail.Text = row[2].ToString();
                guna2TextBox_sdt.Text = row[3].ToString();
            }
        }

        private void GiaoDien_For_HDV_Load(object sender, EventArgs e)
        {
            loadThongTin();
            loadBangPC();
            guna2TextBox_ten.Enabled = false;
            guna2TextBox_mail.Enabled = false;
            guna2TextBox_sdt.Enabled = false;
            guna2Button_save.Hide();
            guna2Button_cancel.Hide();
        }

        private void guna2Button_edit_Click(object sender, EventArgs e)
        {
            guna2Button_cancel.Show();
            guna2Button_save.Show();
            guna2TextBox_ten.Enabled = true;
            guna2TextBox_mail.Enabled = true;
            guna2TextBox_sdt.Enabled = true;
        }

        private void guna2Button_cancel_Click(object sender, EventArgs e)
        {
            guna2TextBox_ten.Enabled = false;
            guna2TextBox_mail.Enabled = false;
            guna2TextBox_sdt.Enabled = false;
            guna2Button_save.Hide();
            guna2Button_cancel.Hide();
            loadThongTin();
        }

        private void guna2Button_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_ten.Text.Trim()) || string.IsNullOrEmpty(guna2TextBox_mail.Text.Trim()) || string.IsNullOrEmpty(guna2TextBox_sdt.Text.Trim()))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cn.Open();
                    string sql = string.Format("update hdv set ten_hdv = N'{0}', email = N'{1}', sdt = '{2}' where ma_hdv = '{3}'", guna2TextBox_ten.Text, guna2TextBox_mail.Text, guna2TextBox_sdt.Text, guna2TextBox_mahdv.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("HOÀN TẤT!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadThongTin();
                    guna2Button_cancel.Hide();
                    guna2Button_save.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { cn.Close(); }
            }
        }

        void loadBangPC()
        {
            string sql = string.Format("SELECT * FROM PC_HDV WHERE MA_HDV = '{0}'", mahdv);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView_pc.DataSource = dt;
        }

        private void GiaoDien_For_HDV_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Nếu chọn "No", hủy sự kiện đóng Form
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy sự kiện đóng Form
            }
            else
            {
                DangNhap f = new DangNhap();
                f.Show();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            QLHOADON f = new QLHOADON();
            f.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            QLDatTour f = new QLDatTour();
            f.Show();
        }
    }
}
