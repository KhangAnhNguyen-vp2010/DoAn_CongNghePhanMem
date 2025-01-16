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

namespace TourDuLich
{
    public partial class ThanhToanHoaDon : Form
    {
        SqlConnection cn;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        SqlCommand cmd;
        GiaoDienChoKhachHang f;
        public ThanhToanHoaDon(GiaoDienChoKhachHang a)
        {
            InitializeComponent();
            cn = kn.conn;
            f = a;
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
                    guna2HtmlLabel4.Width += 1;
                }
            }
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (radioButton_momo.Checked == true)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thanh toán bằng Momo không?",  // Nội dung thông báo
                    "Xác nhận",                                             // Tiêu đề hộp thoại
                    MessageBoxButtons.YesNo,                                // Loại nút: Yes/No
                    MessageBoxIcon.Question                                 // Icon: Dấu hỏi
                );
                if (result == DialogResult.Yes)
                {
                    cn.Open();
                    string sql = string.Format("EXEC THANHTOAN_HOADON '{0}'", guna2HtmlLabel_mahd.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thanh toán thành công với số tiền: " + guna2HtmlLabel_sotien.Text + " thông qua ví Momo!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                    f.load_HoaDon();
                    this.Close();
                    
                }
                else return;
            }
            else if (radioButton_mbbank.Checked == true)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thanh toán bằng MB Bank không?",  // Nội dung thông báo
                    "Xác nhận",                                             // Tiêu đề hộp thoại
                    MessageBoxButtons.YesNo,                                // Loại nút: Yes/No
                    MessageBoxIcon.Question                                 // Icon: Dấu hỏi
                );
                if (result == DialogResult.Yes)
                {
                    cn.Open();
                    string sql = string.Format("EXEC THANHTOAN_HOADON '{0}'", guna2HtmlLabel_mahd.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thanh toán thành công với số tiền: " + guna2HtmlLabel_sotien.Text + " thông qua MB Bank!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                    f.load_HoaDon();
                    this.Close();
                }
                else return;
            }
        }

        private void ThanhToanHoaDon_Load(object sender, EventArgs e)
        {
            // Không cho phép kéo giãn kích thước Form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Vô hiệu hoá nút phóng to
            this.MaximizeBox = false;

            // (Tuỳ chọn) Vô hiệu hoá nút thu nhỏ
            this.MinimizeBox = false;
            guna2HtmlLabel_mahd.Text = StaticThanhToan.maHD;

            decimal tien = Convert.ToDecimal(StaticThanhToan.tongTien);
            guna2HtmlLabel_sotien.Text = ((int)tien).ToString() + "VNĐ";
        }
    }
}
