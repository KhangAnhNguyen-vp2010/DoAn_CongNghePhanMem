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

    public partial class HDV : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        public HDV()
        {
            InitializeComponent();
            conn = kn.conn;
        }
        public void loadGrid()
        {

            string sql = "Select * from HDV";
            adapter = new SqlDataAdapter(sql, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dgv_hdv.DataSource = dt;

            dgv_hdv.Columns["MA_HDV"].HeaderText = "Mã HDV";
            dgv_hdv.Columns["TEN_HDV"].HeaderText = "Tên HDV";
            dgv_hdv.Columns["EMAIL"].HeaderText = "Email";
            dgv_hdv.Columns["SDT"].HeaderText = "Số điện thoại";
            tong_hdv.Text = (dgv_hdv.Rows.Count).ToString();

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void HDV_Load(object sender, EventArgs e)
        {
            loadGrid();
        }

        private void btn_tim_Click(object sender, EventArgs e)
        {

            string mahdv = txt_manv.Text;
            if (!string.IsNullOrEmpty(mahdv))
            {
                string sql = "Select * from HDV where MA_HDV=@HDV";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HDV", mahdv);

                adapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);

                dgv_hdv.DataSource = dt;
                tong_hdv.Text = (dgv_hdv.Rows.Count).ToString();
            }
            else
            {
                loadGrid();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ThemSuaHDV them = new ThemSuaHDV();
            them.Show();
            them.DisableSua();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dgv_hdv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dòng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị xác nhận xóa
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa các dòng đã chọn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
                return;

            try
            {
                conn.Open();

                foreach (DataGridViewRow row in dgv_hdv.SelectedRows)
                {
                    string maHDV = row.Cells["MA_HDV"].Value.ToString();

                    string sql = "exec XOA_HDV @MA_HDV";

                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {

                        command.Parameters.AddWithValue("@MA_HDV", maHDV);
                        int result = command.ExecuteNonQuery();

                        if (result <= 0)
                        {
                            //MessageBox.Show($"Không tìm thấy hướng dẫn viên với mã: {maHDV}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MessageBox.Show($"Không Thể Xoá Hướng Dẫn Viên: {maHDV}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadGrid();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }



        private void làmMớiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadGrid();
        }
        private void dgv_hdv_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            if (dgv_hdv.SelectedRows.Count > 0)
            {
                DataGridViewRow row1 = dgv_hdv.SelectedRows[0];
                string manv = row1.Cells["MA_HDV"].Value.ToString();

                ThemSuaHDV z = new ThemSuaHDV();


                z.SetMaHDV(manv);

                z.Show();
                z.DisableThem();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgv_hdv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy Mã HDV từ cột trong DataGridView (cột Mã HDV)
                DataGridViewRow row1 = dgv_hdv.Rows[e.RowIndex];
                txt_manv.Text = row1.Cells["MA_HDV"].Value.ToString();
            }

        }

        private void btn_xemls_Click(object sender, EventArgs e)
        {
            string manv = txt_manv.Text;
            LichSuDanTour ls = new LichSuDanTour();
            ls.LoadLS(manv);
            ls.Show();
        }

        private void guna2Button_rl_Click(object sender, EventArgs e)
        {
            loadGrid();
        }
    }
}
