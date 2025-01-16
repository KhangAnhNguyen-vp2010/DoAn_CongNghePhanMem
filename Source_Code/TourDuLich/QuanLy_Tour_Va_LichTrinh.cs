using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Data.SqlClient;
using Guna.UI2.WinForms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace QuanLyTourDuLich
{
    public partial class QuanLy_Tour_Va_LichTrinh : Form
    {
        SqlConnection cn;
        KetNoi kn = new KetNoi();
        SqlDataAdapter adapter;
        DataTable dt;
        SqlCommand cmd;
        OpenFileDialog fileDlog;
        public QuanLy_Tour_Va_LichTrinh()
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
                    this.Width += 50;
                    guna2HtmlLabel1.Width += 1;
                    guna2HtmlLabel2.Width += 1;
                    guna2HtmlLabel3.Width += 1;
                    guna2HtmlLabel4.Width += 1;
                    guna2HtmlLabel5.Width += 1;
                    guna2HtmlLabel6.Width += 1;
                    guna2HtmlLabel7.Width += 1;
                    guna2HtmlLabel8.Width += 1;
                    guna2HtmlLabel9.Width += 1;
                    guna2HtmlLabel10.Width += 1;
                    label1.Width += 1;
                    label2.Width += 1;
                    label3.Width += 1;
                    label4.Width += 1;
                    label5.Width += 1;
                    label6.Width += 1;
                    label7.Width += 1;
                    label8.Width += 1;
                    label9.Width += 1;
                    label_sl.Width += 1;
                    label_tongtienTour.Width += 1;
                    label_tt_tour.Width += 1;
                    guna2ComboBox_thutuLT.Width += 10;
                    
                    guna2TabControl1.Width += 50;

                }
            }
        }

        private void QuanLy_Tour_Va_LichTrinh_Load(object sender, EventArgs e)
        {
            //// load tour
            loadCBB_MaDD();
            loadCBB_LocMaDD();
            loadCBB_MaTour();
            loadData_Tour();
            ///// load lich trinh
            loadData_LichTrinh();
            loadCBB_TourForLT();
            loadCBB_HDVForLT();
            loadCBB_Loc_hdv();
            loadCBB_Loc_matour();
        }

        private void guna2Button_file_Click(object sender, EventArgs e)
        {
            fileDlog = new OpenFileDialog();
            fileDlog.Filter = fileDlog.Filter = "JPG (*.jpg)|*.jpg|All files (*.*)|*.*";
            fileDlog.FilterIndex = 1;
            fileDlog.RestoreDirectory = true;
            if (fileDlog.ShowDialog() == DialogResult.OK)
            {
                guna2PictureBox1.ImageLocation = fileDlog.FileName;
                guna2TextBox_linkanh.Text = Path.GetFileName(fileDlog.FileName);
            }

        }

        void loadCBB_MaDD()
        {
            string sql = "select * from diemden";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_madd.DataSource = dt;
            guna2ComboBox_madd.DisplayMember = "ten_diem_den";
            guna2ComboBox_madd.ValueMember = "ma_dia_diem";
        }

        void loadCBB_LocMaDD()
        {
            string sql = "select * from diemden";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_locmadd.DataSource = dt;
            guna2ComboBox_locmadd.DisplayMember = "ten_diem_den";
            guna2ComboBox_locmadd.ValueMember = "ma_dia_diem";
        }

        void loadCBB_MaTour()
        {
            string sql = "select * from diemden";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_tongMatour.DataSource = dt;
            guna2ComboBox_tongMatour.DisplayMember = "ten_diem_den";
            guna2ComboBox_tongMatour.ValueMember = "ma_dia_diem";
        }

        void loadData_Tour()
        {
            string sql = "select * from tour";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView2.DataSource = dt;
            label_sl.Text = "Số lượng: ";
            label_tongtienTour.Text = "Tổng tiền: ";
            label_sl.Text += guna2DataGridView2.RowCount.ToString();
            label_tongtienTour.Text += Convert.ToInt32(dt.Compute("SUM(GIA_TOUR)", string.Empty)).ToString();
        }

        void them()
        {
            try
            {
                cn.Open();
                string sql = string.Format("insert into tour values('{0}', N'{1}', getdate(), '{2}', '{3}', N'{4}', '{5}')", guna2TextBox_matour.Text, guna2TextBox_tentour.Text, guna2TextBox_gia.Text, guna2ComboBox_madd.SelectedValue, guna2TextBox_linkanh.Text, guna2TextBox_slVe.Text);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Insert Success", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData_Tour();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.Close(); }
        }

        void sua()
        {
            try
            {
                cn.Open();
                DateTime date = DateTime.Parse(guna2DateTimePicker1.Text);
                string sql = string.Format("update tour set ten_tour = N'{0}', tg_tour = '{1}', gia_tour='{2}', ma_dia_diem = '{3}', hinhanh = '{4}', sl = '{5}' where ma_tour ='{6}'", guna2TextBox_tentour.Text, date.ToString("yyyy-MM-dd"), guna2TextBox_gia.Text, guna2ComboBox_madd.SelectedValue, guna2TextBox_linkanh.Text, guna2TextBox_slVe.Text,guna2TextBox_matour.Text);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Edit Success", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData_Tour();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.Close(); }
        }

        void xoa()
        {
            try
            {
                cn.Open();

                string sql = string.Format("delete from tour where ma_tour = '{0}'", guna2TextBox_matour.Text);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Delete Success", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData_Tour();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { cn.Close(); }
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    guna2TextBox_matour.Text = guna2DataGridView2.Rows[index].Cells[0].Value.ToString().Trim();
                    guna2TextBox_tentour.Text = guna2DataGridView2.Rows[index].Cells[1].Value.ToString();
                    DateTime datevalue = (DateTime)guna2DataGridView2.Rows[index].Cells[2].Value;
                    guna2DateTimePicker1.Text = datevalue.ToString("yyyy-MM-dd");
                    guna2TextBox_gia.Text = guna2DataGridView2.Rows[index].Cells[3].Value.ToString();
                    guna2ComboBox_madd.SelectedValue = guna2DataGridView2.Rows[index].Cells[4].Value.ToString();
                    guna2TextBox_linkanh.Text = guna2DataGridView2.Rows[index].Cells[5].Value.ToString();
                    guna2TextBox_slVe.Text = guna2DataGridView2.Rows[index].Cells[6].Value.ToString();

                    string relativeImagePath = guna2TextBox_linkanh.Text; // Đường dẫn tương đối

                    string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
                    string imageDirectory = Path.Combine(Application.StartupPath, "image", "tour"); // Thư mục chứa hình ảnh

                    // Kết hợp thư mục gốc với đường dẫn tương đối để có đường dẫn tuyệt đối
                    string imagePath = Path.Combine(imageDirectory, relativeImagePath);

                    if (File.Exists(imagePath)) // Kiểm tra nếu file tồn tại
                    {
                        guna2PictureBox1.Image = Image.FromFile(imagePath); // Chuyển đổi đường dẫn thành hình ảnh
                    }
                    else
                    {
                        guna2PictureBox1.Image = null; // Nếu không tồn tại, đặt giá trị là null
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void guna2Button_them_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_matour.Text) || string.IsNullOrEmpty(guna2TextBox_tentour.Text) || string.IsNullOrEmpty(guna2TextBox_gia.Text) || string.IsNullOrEmpty(guna2TextBox_matour.Text) || guna2PictureBox1 == null)
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else them();
        }

        private void guna2Button_xoa_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(guna2TextBox_matour.Text))
            {
                MessageBox.Show("Nhập mã tour cần xoá !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else xoa();
        }

        private void guna2Button_sua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_matour.Text) || string.IsNullOrEmpty(guna2TextBox_tentour.Text) || string.IsNullOrEmpty(guna2TextBox_gia.Text) || string.IsNullOrEmpty(guna2TextBox_matour.Text) || guna2PictureBox1 == null)
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else sua();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            loadData_Tour();
            label_tt_tour.Text = (0).ToString();
        }

        void SapXep()
        {
            string pt = null;
            int values;
            if (guna2ComboBox_ss.SelectedIndex == 0)
            {
                pt = "ma_tour";
            }
            else if (guna2ComboBox_ss.SelectedIndex == 1)
            {
                pt = "ten_tour";
            }
            else if (guna2ComboBox_ss.SelectedIndex == 2)
            {
                pt = "tg_tour";
            }
            else if (guna2ComboBox_ss.SelectedIndex == 3)
            {
                pt = "gia_tour";
            }
            else if (guna2ComboBox_ss.SelectedIndex == 4)
            {
                pt = "ma_dia_diem";
            }

            if (guna2ComboBox_values.SelectedIndex == 0)
            {
                values = 0;
            }
            else values = 1;

            string sql = string.Format("EXEC SAPXEP_TOUR'{0}', '{1}'", pt, values);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView2.DataSource = dt;
        }

        private void guna2Button_sort_Click(object sender, EventArgs e)
        {
            SapXep();
        }

        void loc_Tour_MaDD()
        {
            string sql = string.Format("EXEC LOC_TOUR_THEOMADD '{0}'", guna2ComboBox_locmadd.SelectedValue);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView2.DataSource = dt;
            label_sl.Text = "Số lượng: ";
            label_tongtienTour.Text = "Tổng tiền: ";
            label_sl.Text += guna2DataGridView2.RowCount.ToString();
            try
            {
                label_tongtienTour.Text += Convert.ToInt32(dt.Compute("SUM(GIA_TOUR)", string.Empty)).ToString();
            }
            catch (Exception)
            {
                label_tongtienTour.Text += (0).ToString();
            }

        }


        private void guna2Button_loc_Click(object sender, EventArgs e)
        {
            loc_Tour_MaDD();
        }

        void search()
        {
            string sql = string.Format("EXEC TIMKIEM_TOUR N'{0}'", guna2TextBox_search.Text);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView2.DataSource = dt;
            label_sl.Text = "Số lượng: ";
            label_tongtienTour.Text = "Tổng tiền: ";
            try
            {
                label_sl.Text += guna2DataGridView2.RowCount.ToString();
                label_tongtienTour.Text += Convert.ToInt32(dt.Compute("SUM(GIA_TOUR)", string.Empty)).ToString();
            }
            catch (Exception)
            {
                label_tongtienTour.Text += "0";
            }


        }

        private void guna2Button_search_Click(object sender, EventArgs e)
        {
            search();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        void sum_tien_tour()
        {
            cn.Open();
            DateTime start = DateTime.Parse(guna2DateTimePicker_start.Text);
            DateTime end = DateTime.Parse(guna2DateTimePicker_End.Text);
            string sql = string.Format("DECLARE @Square INT; set @Square = dbo.TONG_GIA_TOUR ('{0}', '{1}', '{2}'); SELECT @Square", guna2ComboBox_tongMatour.SelectedValue, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
            cmd = new SqlCommand(sql, cn);
            try
            {
                int money = (int)cmd.ExecuteScalar();
                label_tt_tour.Text = money.ToString();
            }
            catch (Exception)
            {
                label_tt_tour.Text = (0).ToString();
            }

            cn.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            sum_tien_tour();
        }

        //// lich trinh
        void loadData_LichTrinh()
        {
            string sql = "select * from lichtrinh";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
        }

        void loadCBB_TourForLT()
        {
            string sql = "select * from tour";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_tour_LT.DataSource = dt;
            guna2ComboBox_tour_LT.DisplayMember = "ten_tour";
            guna2ComboBox_tour_LT.ValueMember = "ma_tour";
        }

        void loadCBB_HDVForLT()
        {
            string sql = "select * from hdv";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_hdv_LT.DataSource = dt;
            guna2ComboBox_hdv_LT.DisplayMember = "ma_hdv";
            guna2ComboBox_hdv_LT.ValueMember = "ma_hdv";
        }

        void loadCBB_Loc_matour()
        {
            string sql = "select * from tour";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_loc_theo_tour.DataSource = dt;
            guna2ComboBox_loc_theo_tour.DisplayMember = "ten_tour";
            guna2ComboBox_loc_theo_tour.ValueMember = "ma_tour";
        }

        void loadCBB_Loc_hdv()
        {
            string sql = "select * from hdv";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_loc_theo_hdv.DataSource = dt;
            guna2ComboBox_loc_theo_hdv.DisplayMember = "ten_hdv";
            guna2ComboBox_loc_theo_hdv.ValueMember = "ma_hdv";
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    string malt = guna2DataGridView1.Rows[index].Cells[1].Value.ToString().Trim();
                    string sql = string.Format("EXEC XEM_CHITIET_LICHTRINH '{0}'", malt);
                    adapter = new SqlDataAdapter(sql, cn);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    guna2ComboBox_tour_LT.SelectedValue = dt.Rows[0][0].ToString();
                    guna2TextBox_malt.Text = dt.Rows[0][1].ToString();
                    DateTime start = DateTime.Parse(dt.Rows[0][2].ToString());
                    guna2DateTimePicker_ngayBD.Text = start.ToString("yyyy-MM-dd");
                    DateTime end = DateTime.Parse(dt.Rows[0][3].ToString());
                    guna2DateTimePicker_KT.Text = end.ToString("yyyy-MM-dd");
                    guna2ComboBox_hdv_LT.SelectedValue = dt.Rows[0][4].ToString();
                }
                catch (Exception)
                {

                }
            }
        }

        private void guna2Button_tk_LT_Click(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC TIMKIEM_LICHTRINH N'{0}'", guna2TextBox_tk_lt.Text);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2Button_ss_LT_Click(object sender, EventArgs e)
        {
            string pt = null;
            int values;
            if (guna2ComboBox_sortLT.SelectedIndex == 0)
            {
                pt = "ma_tour";
            }
            else if (guna2ComboBox_sortLT.SelectedIndex == 1)
            {
                pt = "ma_lt";
            }
            else if (guna2ComboBox_sortLT.SelectedIndex == 2)
            {
                pt = "ngay_bd";
            }
            else if (guna2ComboBox_sortLT.SelectedIndex == 3)
            {
                pt = "ngay_kt";
            }
            else if (guna2ComboBox_sortLT.SelectedIndex == 4)
            {
                pt = "ma_hdv";
            }

            if (guna2ComboBox_thutuLT.SelectedIndex == 0)
            {
                values = 0;
            }
            else values = 1;
            string sql = string.Format("EXEC SAPXEP_LICHTRINH '{0}', '{1}'", pt, values);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2Button_loc_theo_tour_Click(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC LOC_LICHTRINH_THEO_MATOUR '{0}'", guna2ComboBox_loc_theo_tour.SelectedValue);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2Button_loc_theo_hdv_Click(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC LOC_LICHTRINH_THEO_MAHDV '{0}'", guna2ComboBox_loc_theo_hdv.SelectedValue);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2Button_addLT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_malt.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Biến handler để quản lý sự kiện
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
                    // Đăng ký sự kiện InfoMessage trước khi mở kết nối
                    cn.InfoMessage += infoMessageHandler;
                    cn.Open();
                    DateTime start = DateTime.Parse(guna2DateTimePicker_ngayBD.Text);
                    DateTime end = DateTime.Parse(guna2DateTimePicker_KT.Text);
                    string sql = string.Format("EXEC THEM_LICHTRINH '{0}', '{1}', '{2}', '{3}', '{4}'", guna2ComboBox_tour_LT.SelectedValue, guna2TextBox_malt.Text, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"), guna2ComboBox_hdv_LT.SelectedValue);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    loadData_LichTrinh();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally {
                    // Hủy đăng ký sự kiện InfoMessage và đóng kết nối
                    cn.InfoMessage -= infoMessageHandler;
                    cn.Close();
                }
            }
        }

        private void guna2Button_editLT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_malt.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Biến handler để quản lý sự kiện
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
                    // Đăng ký sự kiện InfoMessage trước khi mở kết nối
                    cn.InfoMessage += infoMessageHandler;
                    cn.Open();
                    DateTime start = DateTime.Parse(guna2DateTimePicker_ngayBD.Text);
                    DateTime end = DateTime.Parse(guna2DateTimePicker_KT.Text);
                    string sql = string.Format("EXEC SUA_LICHTRINH '{0}', '{1}', '{2}', '{3}', '{4}'", guna2ComboBox_tour_LT.SelectedValue, guna2TextBox_malt.Text, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"), guna2ComboBox_hdv_LT.SelectedValue);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    loadData_LichTrinh();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally {
                    // Hủy đăng ký sự kiện InfoMessage và đóng kết nối
                    cn.InfoMessage -= infoMessageHandler;
                    cn.Close();
                }
            }
        }

        private void guna2Button_xoaLT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_malt.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ liệu!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cn.Open();
                    string sql = string.Format("delete from lichtrinh where ma_lt = '{0}'", guna2TextBox_malt.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xoá Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData_LichTrinh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { cn.Close(); }
            }
        }

        private void guna2Button_reloadLT_Click(object sender, EventArgs e)
        {
            loadData_LichTrinh();
        }
    }
}
