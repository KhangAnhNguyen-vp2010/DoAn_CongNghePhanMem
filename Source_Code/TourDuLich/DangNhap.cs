using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using TourDuLich;

namespace WindowsFormsApp1
{
    public partial class DangNhap : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        public DangNhap()
        {
            InitializeComponent();
            conn = kn.conn;
        }

        bool Login(string username, string password)
        {
            bool result = false;
            string cmd = "SELECT COUNT(1) FROM TAI_KHOAN WHERE TENTAIKHOAN=@username AND MATKHAU=@password";

            SqlCommand command = new SqlCommand(cmd, conn);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            try
            {
                conn.Open();
                int count = (int)command.ExecuteScalar();

                // Kiểm tra nếu tài khoản tồn tại
                result = (count == 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        string roleConform(string username, string password)
        {
            string role = string.Empty;
            string cmd = "SELECT LOAITK FROM TAI_KHOAN WHERE TENTAIKHOAN = @tk AND MATKHAU = @mk";

            SqlCommand command = new SqlCommand(cmd, conn);
            command.Parameters.AddWithValue("@tk", username);
            command.Parameters.AddWithValue("@mk", password);

            conn.Open();
            object result = command.ExecuteScalar();

            // Kiểm tra nếu tài khoản tồn tại và lấy vai trò
            if (result != null)
            {
                role = result.ToString();
            }
            conn.Close();
            return role;
        }
        private void btn_dangky_Click(object sender, EventArgs e)
        {
            DangKy dk = new DangKy();
            dk.Show();
            this.Hide();
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            string tk = txt_tk.Text;
            string mk = txt_mk.Text;
            if (string.IsNullOrEmpty(tk) || string.IsNullOrEmpty(mk))
            {
                MessageBox.Show("Nhập đầy đủ tài khoản mật khẩu", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (rdo_ad.Checked == false && rdo_kh.Checked == false && rdo_nv.Checked == false)
            {
                MessageBox.Show("Bạn Phải Chọn Role Của Mình", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string role = roleConform(tk, mk).Trim();
            if (rdo_kh.Checked == true && role == "KH")
            {
                if (Login(tk, mk))
                {
                    MessageBox.Show("Khách hàng đăng nhập thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form chính hoặc thực hiện các hành động khác
                    StaticUser.tenTK = tk.Trim();
                    StaticUser.vaiTro = role.Trim();
                    GiaoDienChoKhachHang gd_for_kh = new GiaoDienChoKhachHang();
                    gd_for_kh.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rdo_ad.Checked == true && role == "AD")
            {
                if (Login(tk, mk))
                {
                    MessageBox.Show("Admin đăng nhập thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form chính hoặc thực hiện các hành động khác
                    GiaoDien_For_Admin dg_for_admin = new GiaoDien_For_Admin();
                    StaticUser.tenTK = tk.Trim();
                    StaticUser.vaiTro = role.Trim();
                    dg_for_admin.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rdo_nv.Checked == true && role == "HDV")
            {
                if (Login(tk, mk))
                {
                    MessageBox.Show("Nhân viên đăng nhập thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    StaticUser.tenTK = tk.Trim();
                    StaticUser.vaiTro = role.Trim();

                    // Mở form chính hoặc thực hiện các hành động khác
                    GiaoDien_For_HDV gdHDV = new GiaoDien_For_HDV();
                    gdHDV.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_doimk_Click(object sender, EventArgs e)
        {
            DoiMK dmk = new DoiMK();
            dmk.Show();
            this.Hide();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi; // Hoặc Font
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn Có Muốn Thoát???", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.No)
            {
                
            }
            else if (r == DialogResult.Yes)
            {
                Close();
            }
        }

        private void DangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void DangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
