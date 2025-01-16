using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using QuanLyTourDuLich;
using TheArtOfDevHtmlRenderer.Adapters;
using WindowsFormsApp1;

namespace TourDuLich
{
    public partial class NhapThongTinKH_KhiDangKy : Form
    {
        SqlConnection cn;
        KetNoi kn = new KetNoi();
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        string tentk, pass;
        public NhapThongTinKH_KhiDangKy(string tk, string mk)
        {
            InitializeComponent();
            cn = kn.conn;
            tentk = tk;
            pass = mk;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false; // Email rỗng hoặc chỉ chứa khoảng trắng
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Biểu thức chính quy kiểm tra email
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        bool IsValid_phone(string text)
        {
            // Kiểm tra độ dài chuỗi có đúng 10 ký tự và toàn bộ là số
            return text.Length == 10 && text.All(char.IsDigit);
        }

        bool IsValid_cccd(string text)
        {
            // Kiểm tra độ dài chuỗi có đúng 12 ký tự và toàn bộ là số
            return text.Length == 12 && text.All(char.IsDigit);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void NhapThongTinKH_KhiDangKy_Load(object sender, EventArgs e)
        {

        }
        string maKH()
        {
            string sql = "select top 1 ma_kh from khachhang order by ma_kh desc";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string old = "KH001";
            string newStr;
            if (dt.Rows.Count > 0)
            {
                old = dt.Rows[0][0].ToString();
                int stt = int.Parse(old.Substring(2));
                stt++;
                newStr = "KH" + stt.ToString("D3");
                return newStr;
            }
            return old;
        }
        private void btn_dangky_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_tenkh.Text.Trim()))
            {
                MessageBox.Show("Vui Lòng Nhập Tên!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txt__mail.Text.Trim()))
            {
                MessageBox.Show("Vui Lòng Nhập Email!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (IsValidEmail(txt__mail.Text) == false)
                {
                    MessageBox.Show("Định dạng email không hợp lệ!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (string.IsNullOrEmpty(txt_sdt.Text.Trim()))
            {
                MessageBox.Show("Vui Lòng Nhập SĐT!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (IsValid_phone(txt_sdt.Text) == false)
                {
                    MessageBox.Show("SĐT phải đủ 10 kí tự số!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (string.IsNullOrEmpty(guna2TextBox_diachi.Text.Trim()))
            {
                MessageBox.Show("Vui Lòng Nhập Địa Chỉ!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(guna2TextBox_cccd.Text.Trim()))
            {
                MessageBox.Show("Vui Lòng Nhập CCCD!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (IsValid_cccd(guna2TextBox_cccd.Text) == false)
                {
                    MessageBox.Show("CCCD phải đủ 12 kí tự số!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            cn.Open();
            string makh = maKH();
            string sql = string.Format("EXEC TAOTAIKHOAN '{0}', '{1}', 'KH', 'HOAT DONG', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'", tentk, pass, makh, txt_tenkh.Text, txt__mail.Text, txt_sdt.Text, guna2TextBox_diachi.Text, guna2TextBox_cccd.Text);
            cmd = new SqlCommand(sql, cn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Đăng Ký Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cn.Close();
            DangNhap f = new DangNhap();
            f.Show();
            this.Close();
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void btn__back_Click(object sender, EventArgs e)
        {
            DangKy f = new DangKy();
            f.Show();
            this.Close();
        }
    }
}
