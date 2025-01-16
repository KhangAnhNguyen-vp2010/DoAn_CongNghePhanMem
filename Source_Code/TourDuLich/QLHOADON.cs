using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using System.Threading;
using TourDuLich;
using Guna.UI2.WinForms;
namespace HOADON
{
    public partial class QLHOADON : Form
    {
        SqlConnection cn;
        KetNoi kn = new KetNoi();
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataTable dt;
        public QLHOADON()
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

                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text) || string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtMaDat.Text) || string.IsNullOrEmpty(txtTongTien.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                try
                {
                    cn.Open();
                    DateTime date = DateTime.Parse(dtpNgayLap.Text);
                    string sql = string.Format("exec add_hoadon '{0}', '{1}', '{2}', '{3}', '{4}', N'{5}', '{6}'", txtMaDat.Text, txtMaHD.Text, date.ToString("yyyy/MM/dd"), txtMaKH.Text, txtTongTien.Text, guna2ComboBox_tt.SelectedValue);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    loadData();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { cn.Close(); }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {



        }

        void loadData()
        {
            string sql = "select * from hoadon";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            dgvHoaDon.DataSource = dt;

        }


        private void dtpNgayLap_ValueChanged(object sender, EventArgs e)
        {

        }


        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTimkiem_Click_1(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC TIMKIEM_HOADON N'{0}'", txtTimKiem.Text);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            dgvHoaDon.DataSource = dt;
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text) || string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtMaDat.Text) || string.IsNullOrEmpty(txtTongTien.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                // Biến handler để quản lý sự kiện
                SqlInfoMessageEventHandler infoMessageHandler = (s, eArgs) =>
                {
                    // Hiển thị từng thông báo từ SQL Server
                    foreach (SqlError error in eArgs.Errors)
                    {
                        MessageBox.Show("Thông báo từ SQL: " + error.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                try
                {
                    cn.InfoMessage += infoMessageHandler;
                    cn.Open();
                    DateTime date = DateTime.Parse(dtpNgayLap.Text);
                    string sql = string.Format("exec add_hoadon '{0}', '{1}', '{2}', '{3}', '{4}', N'{5}'", txtMaDat.Text, txtMaHD.Text, date.ToString("yyyy/MM/dd"), txtMaKH.Text, txtTongTien.Text, guna2ComboBox_tt.SelectedItem);
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
            loadData();
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                cn.Open();
                string sql = string.Format("exec delete_hoadon '{0}'", txtMaHD.Text);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.Close(); }
            loadData();
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text) || string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtMaDat.Text) || string.IsNullOrEmpty(txtTongTien.Text))
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
                    cn.InfoMessage += infoMessageHandler;
                    cn.Open();
                    DateTime date = DateTime.Parse(dtpNgayLap.Text);
                    string sql = string.Format("exec edit_hoadon '{0}', '{1}', '{2}', '{3}', '{4}',  N'{5}'", txtMaDat.Text, txtMaHD.Text, date.ToString("yyyy/MM/dd"), txtMaKH.Text, txtTongTien.Text, guna2ComboBox_tt.SelectedItem);
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
            loadData();
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvHoaDon_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    txtMaHD.Text = dgvHoaDon.Rows[index].Cells[1].Value.ToString();
                    txtMaDat.Text = dgvHoaDon.Rows[index].Cells[0].Value.ToString();
                    DateTime date = (DateTime)dgvHoaDon.Rows[index].Cells[2].Value;
                    dtpNgayLap.Text = date.ToString("yyyy/MM/dd");
                    txtMaKH.Text = dgvHoaDon.Rows[index].Cells[3].Value.ToString();
                    txtTongTien.Text = dgvHoaDon.Rows[index].Cells[4].Value.ToString();
                    // Lấy giá trị từ DataGridView
                    string comboBoxValue = dgvHoaDon.Rows[index].Cells[5].Value.ToString();

                    // Kiểm tra giá trị có tồn tại trong danh sách ComboBox
                    if (!string.IsNullOrEmpty(comboBoxValue) && guna2ComboBox_tt.Items.Contains(comboBoxValue))
                    {
                        guna2ComboBox_tt.SelectedItem = comboBoxValue;
                    }
                    else
                    {
                        guna2ComboBox_tt.SelectedIndex = -1; // Không chọn mục nào nếu giá trị không khớp
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void guna2Button_updateTT_Click(object sender, EventArgs e)
        {
            cn.Open();
            string sql = "EXEC CapNhatTrangThaiHoaDon";
            cmd = new SqlCommand(sql,cn);
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Đang Tiến Hành Cập Nhật!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Thread.Sleep(3000);
            MessageBox.Show("Đã Hoàn Tất!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadData();
        }
    }
}
