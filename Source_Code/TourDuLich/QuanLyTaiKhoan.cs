using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport.DevComponents.DotNetBar.Controls;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;

namespace TourDuLich
{
    public partial class QuanLyTaiKhoan : Form
    {
        SqlConnection cn;
        SqlDataAdapter adapter;
        DataTable dt;
        KetNoi kn = new KetNoi();
        SqlCommand cmd;
        public QuanLyTaiKhoan()
        {
            InitializeComponent();
            cn = kn.conn;
        }

        public void load_data()
        {
            string sql = string.Format("select * from tai_khoan");
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
            button_sl.Text = guna2DataGridView1.Rows.Count.ToString();

            // Kiểm tra xem DataGridView có dữ liệu hay không
            if (guna2DataGridView1.Rows.Count > 0)
            {
                // Chọn dòng đầu tiên
                guna2DataGridView1.ClearSelection();
                guna2DataGridView1.Rows[0].Selected = true;
                guna2DataGridView1.CurrentCell = guna2DataGridView1.Rows[0].Cells[0];

                // Gọi sự kiện CellClick cho dòng đầu tiên
                guna2DataGridView1_CellClick(
                    guna2DataGridView1,
                    new DataGridViewCellEventArgs(0, 0) // Cột 0, dòng 0
                );

                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    row.Cells["MATKHAU"].Value = new string('•', row.Cells["MATKHAU"].Value.ToString().Length);
                }

            }

        }

        void loadCBB()
        {
            string sql = "SELECT TENTAIKHOAN from TAI_KHOAN where LOAITK = 'HDV' OR LOAITK = 'KH'";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_cbbtk.DataSource = dt;
            guna2ComboBox_cbbtk.DisplayMember = "tentaikhoan";
            guna2ComboBox_cbbtk.ValueMember = "tentaikhoan";
            guna2ComboBox_cbbtk.SelectedIndex = 0;
        }
        void Reset_Pass()
        {
            try
            {
                cn.Open();
                string sql = string.Format("update tai_khoan set matkhau = '123' where tentaikhoan = N'{0}'", guna2ComboBox_cbbtk.SelectedValue);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Reset Mật Khẩu Thành Công!!! - Mật Khẩu Mặc Định Là '123'", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.Close(); }
        }

        private void QuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            load_data();
            guna2ComboBox_cbbtk.Enabled = false;
            guna2Button_sb.Enabled = false;
            loadCBB();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    label_tentk.Text = guna2DataGridView1.Rows[index].Cells[0].Value.ToString();
                    string password = guna2DataGridView1.Rows[index].Cells[1].Value.ToString();
                    label_mk.Text = new string('*', password.Length);
                    label_loaitk.Text = guna2DataGridView1.Rows[index].Cells[2].Value.ToString();
                    label_tt.Text = guna2DataGridView1.Rows[index].Cells[3].Value.ToString();
                    if (label_loaitk.Text == "AD")
                    {
                        guna2Button_xoa.Hide();
                    }
                    else guna2Button_xoa.Show();
                }
                catch (Exception)
                {

                }
            }
        }

        private void guna2Button_sua_Click(object sender, EventArgs e)
        {
            if (guna2Button_sua.Text == "Reset Password")
            {
                guna2ComboBox_cbbtk.Enabled = true;
                guna2Button_sb.Enabled = true;
                guna2Button_sua.Text = "Cancel";
            }
            else
            {
                guna2ComboBox_cbbtk.Enabled = false;
                guna2Button_sb.Enabled = false;
                guna2Button_sua.Text = "Reset Password";
            }
        }

        private void guna2Button_sb_Click(object sender, EventArgs e)
        {
            Reset_Pass();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            load_data();
        }

        private void guna2Button_them_HDV_Click(object sender, EventArgs e)
        {
            ThemTK_HDV f = new ThemTK_HDV(this);
            f.Show();
        }

        void xoa_TK()
        {
            try
            {
                cn.Open();
                string sql = string.Format("delete from tai_khoan where tentaikhoan = N'{0}'", label_tentk.Text);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xoá Tài Khoản Thành Công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.Close(); }
        }
        private void guna2Button_xoa_Click(object sender, EventArgs e)
        {
            xoa_TK();
        }

        private void guna2Button_searchTK_Click(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC TIMKIEM_TAIKHOAN N'{0}'", guna2TextBox_searchTK.Text);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
            button_sl.Text = guna2DataGridView1.Rows.Count.ToString();
        }
    }
}
