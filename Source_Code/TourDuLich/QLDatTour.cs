using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using TourDuLich;
using Guna.UI2.WinForms;
namespace DAT_TOUR
{
    public partial class QLDatTour : Form
    {
        SqlConnection cn;
        KetNoi kn = new KetNoi();
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        public QLDatTour()
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
                    guna2HtmlLabel8.Width += 1;

                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void QLDatTour_Load(object sender, EventArgs e)
        {
            loadData();
            if (StaticUser.vaiTro == "HDV")
            {
                btnXoa.Hide();
            }
            else
            {
                btnXoa.Show();
            }
        }
        void loadData()
        {
            string sql = "select * from DAT_TOUR";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            dgvDatTour.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTour.Text) || string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtMaDat.Text) || string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtTongGia.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
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
                    cn.InfoMessage += infoMessageHandler;
                    cn.Open();
                    DateTime date = DateTime.Parse(dtpNgayDat.Text);
                    string sql = string.Format("exec add_dattour '{0}', '{1}', '{2}', '{3}', '{4}','{5}', N'{6}', '{7}'", txtMaDat.Text, txtMaTour.Text, txtMaKH.Text, txtSoLuong.Text, date.ToString("yyyy/MM/dd"), txtTongGia.Text, cboTrangThai.SelectedItem, guna2TextBox_malt.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    loadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { cn.InfoMessage -= infoMessageHandler; cn.Close(); }
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaDat.Text))
            {
                MessageBox.Show("Vui lòng nhập mã đặt tour cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
                cn.Open();
                cn.InfoMessage += infoMessageHandler;
                string sql = string.Format("exec delete_dattour '{0}'", txtMaDat.Text);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.InfoMessage -= infoMessageHandler; cn.Close(); }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTour.Text) || string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtMaDat.Text) || string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtTongGia.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
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
                    cn.Open();
                    cn.InfoMessage += infoMessageHandler;
                    DateTime date = DateTime.Parse(dtpNgayDat.Text);
                    string sql = string.Format("exec edit_dattour '{0}', '{1}', '{2}', '{3}', '{4}','{5}', N'{6}', '{7}'", txtMaDat.Text, txtMaTour.Text, txtMaKH.Text, txtSoLuong.Text, date.ToString("yyyy/MM/dd"), txtTongGia.Text, cboTrangThai.SelectedItem, guna2TextBox_malt.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    loadData();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { cn.InfoMessage -= infoMessageHandler; cn.Close(); }
            }

        }

        private void btnDong_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvDatTour_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    txtMaDat.Text = dgvDatTour.Rows[index].Cells[0].Value.ToString();
                    txtMaTour.Text = dgvDatTour.Rows[index].Cells[1].Value.ToString();
                    txtMaKH.Text = dgvDatTour.Rows[index].Cells[2].Value.ToString();
                    txtSoLuong.Text = dgvDatTour.Rows[index].Cells[3].Value.ToString();
                    DateTime date = (DateTime)dgvDatTour.Rows[index].Cells[4].Value;
                    dtpNgayDat.Text = date.ToString("yyyy/MM/dd");
                    txtTongGia.Text = dgvDatTour.Rows[index].Cells[5].Value.ToString();
                    string comboBoxValue = dgvDatTour.Rows[index].Cells[6].Value.ToString();

                    // Kiểm tra giá trị có tồn tại trong danh sách ComboBox
                    if (!string.IsNullOrEmpty(comboBoxValue) && cboTrangThai.Items.Contains(comboBoxValue))
                    {
                        cboTrangThai.SelectedItem = comboBoxValue;
                    }
                    else
                    {
                        cboTrangThai.SelectedIndex = -1; // Không chọn mục nào nếu giá trị không khớp
                    }
                    guna2TextBox_malt.Text = dgvDatTour.Rows[index].Cells[7].Value.ToString();
                }
                catch (Exception)
                {

                }
            }
        }

        private void dgvDatTour_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string sql = "select * from DAT_TOUR";
        }

        private void txtMaTour_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMaKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC TIMKIEM_DATTOUR N'{0}'", guna2TextBox_tk.Text);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            dgvDatTour.DataSource = dt;
        }
    }
}
