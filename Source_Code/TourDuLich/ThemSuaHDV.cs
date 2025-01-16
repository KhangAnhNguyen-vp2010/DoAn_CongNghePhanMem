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
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
namespace WindowsFormsApp1
{
    public partial class ThemSuaHDV : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;

        public ThemSuaHDV()
        {
            InitializeComponent();
            conn = kn.conn;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            string maHDV = txt_manv.Text.Trim();
            string tenHDV = txt_ten.Text.Trim();
            string email = txt_email.Text.Trim();
            string sdt = txt_sdt.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(maHDV) || string.IsNullOrEmpty(tenHDV) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^\d+$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra trùng mã hướng dẫn viên
            string checkSql = "SELECT COUNT(*) FROM HDV WHERE MA_HDV = @MA_HDV";
            cmd = new SqlCommand(checkSql, conn);
            cmd.Parameters.AddWithValue("@MA_HDV", maHDV);

            try
            {
                conn.Open();
                int existingCount = (int)cmd.ExecuteScalar();

                if (existingCount > 0)
                {
                    MessageBox.Show("Mã hướng dẫn viên đã tồn tại, vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = "EXEC THEM_HDV @MA_HDV, @TEN_HDV, @EMAIL, @SDT";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MA_HDV", maHDV);
                cmd.Parameters.AddWithValue("@TEN_HDV", tenHDV);
                cmd.Parameters.AddWithValue("@EMAIL", email);
                cmd.Parameters.AddWithValue("@SDT", sdt);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Thêm hướng dẫn viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HDV hdv = new HDV();
                    hdv.loadGrid();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm hướng dẫn viên không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_sua_Click(object sender, EventArgs e)
        {
            string maHDV = txt_manv.Text.Trim();
            string tenHDV = txt_ten.Text.Trim();
            string email = txt_email.Text.Trim();
            string sdt = txt_sdt.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(maHDV) || string.IsNullOrEmpty(tenHDV) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^\d+$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string sql = "EXEC CAPNHAT_HDV @MA_HDV, @TEN_HDV, @EMAIL, @SDT";
            cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MA_HDV", maHDV);
            cmd.Parameters.AddWithValue("@TEN_HDV", tenHDV);
            cmd.Parameters.AddWithValue("@EMAIL", email);
            cmd.Parameters.AddWithValue("@SDT", sdt);
            try
            {
                conn.Open();
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thông tin hướng dẫn viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HDV hdv = new HDV();
                    hdv.loadGrid();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thông tin hướng dẫn viên không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void SetMaHDV(string maHDV)
        {
            txt_manv.Text = maHDV;
        }
        public void DisableSua()
        {
            btn_sua.Enabled = false;
        }
        public void DisableThem()
        {
            btn_them.Enabled = false;
            txt_manv.Enabled = false;
        }

        private void ThemSuaHDV_Load(object sender, EventArgs e)
        {

        }
    }
}
