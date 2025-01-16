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
using QuanLyTourDuLich;

namespace WindowsFormsApp1
{
    public partial class DoiMK : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        public DoiMK()
        {
            InitializeComponent();
            conn = kn.conn;
        }

        private void btn_doimk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_tk.Text) ||
                string.IsNullOrEmpty(txt_mk.Text) ||
                string.IsNullOrEmpty(txt_mkmoi.Text) ||
                string.IsNullOrEmpty(txt_nhaplai.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mật khẩu mới và nhập lại mật khẩu khớp
            if (txt_mkmoi.Text != txt_nhaplai.Text)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu không trùng khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Chuẩn bị câu lệnh SQL gọi stored procedure
            string sql = "EXEC DOIMATKHAU @MA_TK, @MATKHAU_CU, @MATKHAU_MOI, @NHAP_LAI";
            cmd = new SqlCommand(sql, conn);

            // Thêm các tham số
            cmd.Parameters.AddWithValue("@MA_TK", txt_tk.Text);
            cmd.Parameters.AddWithValue("@MATKHAU_CU", txt_mk.Text);
            cmd.Parameters.AddWithValue("@MATKHAU_MOI", txt_mkmoi.Text);
            cmd.Parameters.AddWithValue("@NHAP_LAI", txt_nhaplai.Text);

            try
            {
                conn.Open();

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_mk.Clear();
                    txt_mkmoi.Clear();
                    txt_nhaplai.Clear();
                    txt_tk.Clear();
                }
                else
                {
                    MessageBox.Show($"Không tồn tại tên tài khoản là: {txt_tk.Text} hoặc mật khẩu cũ không đúng. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            DangNhap dn = new DangNhap();
            dn.Show();
            this.Hide();
        }

        private void DoiMK_Load(object sender, EventArgs e)
        {
         
        }
    }
}
