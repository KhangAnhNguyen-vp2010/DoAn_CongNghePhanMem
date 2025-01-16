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

namespace KHACHHANG__PC_TOUR
{
    public partial class PhanCongHDV : Form
    {
        KetNoi kn = new KetNoi();
        SqlConnection conn;
        public PhanCongHDV()
        {
            conn = kn.conn;
            InitializeComponent();
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
                    btn_xoapc.Width += 10;

                }
            }
        }

        void load_dstour()
        {
            string sql = "SELECT * FROM TOUR";
            DataTable dataTable1 = new DataTable();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    conn.Open();
                    adapter.Fill(dataTable1);


                    if (dataTable1.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        DGV_HDV.DataSource = dataTable1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        void load_dshdv()
        {
            string sql = "SELECT * FROM HDV";
            DataTable dataTable1 = new DataTable();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    conn.Open();
                    adapter.Fill(dataTable1);


                    if (dataTable1.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        DGV_HDV.DataSource = dataTable1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void guna2TextBox2_Click(object sender, EventArgs e)
        {
            DGV_HDV.DataSource = null;
            DGV_HDV.Rows.Clear();
            label_title.Text = "Danh sách Tour";
            load_dstour();
        }

        private void guna2TextBox1_Click(object sender, EventArgs e)
        {
            DGV_HDV.DataSource = null;
            DGV_HDV.Rows.Clear();
            label_title.Text = "Danh sách hướng dẫn viên";
            load_dshdv();
        }

        private void btn_phancong_Click(object sender, EventArgs e)
        {
            string mahdv = txt_mahdv.Text.Trim();
            string matour = txt_matour.Text.Trim();
            DateTime ngaypc = DTP_ngaypc.Value.Date;

            //Kiem tra neu khong co ma tour trong du lieu
            string kiemtra_matour = "SELECT COUNT(*) FROM TOUR WHERE MA_TOUR = @MATOUR";
            using (SqlCommand checkCommand = new SqlCommand(kiemtra_matour, conn))
            {
                checkCommand.Parameters.AddWithValue("@MATOUR", matour);
                conn.Open();
                int count = (int)checkCommand.ExecuteScalar();
                conn.Close();
                if (count == 0)
                {
                    // Nếu MaNV đã tồn tại, thông báo lỗi
                    MessageBox.Show("Mã Tour không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            //Kiem tra neu khong co ma hướng dẫn viên trong du lieu
            string kiemtra_mahdv = "SELECT COUNT(*) FROM HDV WHERE MA_HDV = @MAHDV";
            using (SqlCommand checkCommand = new SqlCommand(kiemtra_mahdv, conn))
            {
                checkCommand.Parameters.AddWithValue("@MAHDV", mahdv);
                conn.Open();
                int count = (int)checkCommand.ExecuteScalar();
                conn.Close();
                if (count == 0)
                {
                    // Nếu MaNV đã tồn tại, thông báo lỗi
                    MessageBox.Show("Mã hướng dẫn viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            try
            {
                string sql = "EXEC PROC_THEM_CAPNHAT_PHANCONG @MAHDV ,@MATOUR ,@NGAYPC ";
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@MAHDV", mahdv);
                    command.Parameters.AddWithValue("@MATOUR", matour);
                    command.Parameters.AddWithValue("@NGAYPC", ngaypc);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Phân công thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Lỗi phân công : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DGV_HDV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && label_title.Text == "Danh sách Tour")
            {

                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DGV_HDV.Rows[e.RowIndex];

                string matour = row.Cells["MA_TOUR"].Value.ToString();
                DateTime tgtour = row.Cells["TG_TOUR"].Value != DBNull.Value && row.Cells["TG_TOUR"].Value != null ? Convert.ToDateTime(row.Cells["TG_TOUR"].Value) : DateTime.Now;
                // Đổ dữ liệu vào các TextBox
                txt_matour.Text = matour;
                DTP_ngaypc.Value = tgtour;
            }
            else if (e.RowIndex >= 0 && label_title.Text == "Danh sách hướng dẫn viên")
            {

                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DGV_HDV.Rows[e.RowIndex];

                string mahdv = row.Cells["MA_HDV"].Value.ToString();
                // Đổ dữ liệu vào các TextBox
                txt_mahdv.Text = mahdv;
            }
            else if (e.RowIndex >= 0 && label_title.Text == "Danh sách phân công HDV")
            {

                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DGV_HDV.Rows[e.RowIndex];

                string mahdv = row.Cells["MA_HDV"].Value.ToString();
                string matour = row.Cells["MA_TOUR"].Value.ToString();
                DateTime ngaypc = row.Cells["NGAY_PC"].Value != DBNull.Value && row.Cells["NGAY_PC"].Value != null ? Convert.ToDateTime(row.Cells["NGAY_PC"].Value) : DateTime.Now;
                // Đổ dữ liệu vào các TextBox
                txt_mahdv.Text = mahdv;
                txt_matour.Text = matour;
                DTP_ngaypc.Value = ngaypc;
            }
        }
        void load_dspc()
        {
            string sql = "SELECT * FROM PC_HDV";
            DataTable dataTable1 = new DataTable();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    conn.Open();
                    adapter.Fill(dataTable1);


                    if (dataTable1.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        DGV_HDV.DataSource = dataTable1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void btn_dspc_Click(object sender, EventArgs e)
        {
            DGV_HDV.DataSource = null;
            DGV_HDV.Rows.Clear();
            label_title.Text = "Danh sách phân công HDV";
            load_dspc();
        }


        void xoaPC()
        {
            string mahdv = txt_mahdv.Text;
            string matour = txt_matour.Text;

            // Xác nhận xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa phân công của hướng dẫn viên có mã " + mahdv + " trong tour " + matour + " ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra nếu makh đã tồn tại trong cơ sở dữ liệu
            string kiemtra_makh = "SELECT COUNT(*) FROM PC_HDV WHERE MA_HDV = @mahdv AND MA_TOUR = @matour";
            try
            {
                using (SqlCommand checkCommand = new SqlCommand(kiemtra_makh, conn))
                {
                    checkCommand.Parameters.AddWithValue("@mahdv", txt_mahdv.Text);
                    checkCommand.Parameters.AddWithValue("@matour", txt_matour.Text);
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    conn.Close();

                    if (count == 0)
                    {
                        // Nếu makh không tồn tại, thông báo lỗi
                        MessageBox.Show("Phân công không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Lỗi :" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (result == DialogResult.Yes)
            {

                try
                {

                    string sql = "EXEC PROC_XOA_PHANCONG_HDV @MA_HDV , @MA_TOUR ;";
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@MA_HDV", mahdv);
                        command.Parameters.AddWithValue("@MA_TOUR", matour);
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }

                    load_dspc(); // Hàm để load lại dữ liệu vào DataGridView

                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void btn_xoapc_Click(object sender, EventArgs e)
        {
            xoaPC();
        }



        void LoadMaTourComboBox()
        {
            try
            {
                conn.Open();

                // Truy vấn để lấy mã tour từ bảng TOUR
                string sqlTour = "SELECT MA_TOUR FROM TOUR";  // Giả sử bảng TOUR có cột MA_TOUR
                SqlCommand cmdTour = new SqlCommand(sqlTour, conn);
                SqlDataReader readerTour = cmdTour.ExecuteReader();

                // Thêm mã tour vào ComboBox
                cbB_tim_matour.Items.Clear();  // Xóa các mục cũ trong ComboBox (nếu có)
                while (readerTour.Read())
                {
                    cbB_tim_matour.Items.Add(readerTour["MA_TOUR"].ToString());
                }

                readerTour.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu mã tour: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadMaHDVComboBox()
        {
            try
            {
                // Tạo kết nối với cơ sở dữ liệu

                conn.Open();

                // Truy vấn để lấy mã hướng dẫn viên từ bảng HDV
                string sqlHDV = "SELECT MA_HDV FROM HDV";  // Giả sử bảng HDV có cột MA_HDV
                SqlCommand cmdHDV = new SqlCommand(sqlHDV, conn);
                SqlDataReader readerHDV = cmdHDV.ExecuteReader();

                // Thêm mã hướng dẫn viên vào ComboBox
                cbB_tim_mahdv.Items.Clear();  // Xóa các mục cũ trong ComboBox (nếu có)
                while (readerHDV.Read())
                {
                    cbB_tim_mahdv.Items.Add(readerHDV["MA_HDV"].ToString());
                }

                readerHDV.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu mã hướng dẫn viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PhanCongHDV_Load(object sender, EventArgs e)
        {
            LoadMaHDVComboBox();
            LoadMaTourComboBox();

        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các textbox
            string maHDV = cbB_tim_mahdv.Text.Trim();
            string maTour = cbB_tim_matour.Text.Trim();
            string ngayPC = DTP_tim.Value.ToString("yyyy-MM-dd");


            // Kiểm tra các checkbox để xác định xem có áp dụng điều kiện lọc nào
            if (!chkB_mahdv.Checked)
                maHDV = null;

            if (!chkB_matour.Checked)
                maTour = null;

            if (!chkB_ngay.Checked)
                ngayPC = null;

            // Kiểm tra nếu các textbox trống, có thể để NULL cho parameter
            if (string.IsNullOrEmpty(maHDV))
                maHDV = null;
            if (string.IsNullOrEmpty(maTour))
                maTour = null;
            if (string.IsNullOrEmpty(ngayPC))
                ngayPC = null;

            // Kết nối và gọi stored procedure
            try
            {
                // Cấu hình kết nối cơ sở dữ liệu

                conn.Open();

                // Tạo command để gọi stored procedure
                using (SqlCommand cmd = new SqlCommand("PROC_TIM_PC_HDV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số vào stored procedure
                    cmd.Parameters.AddWithValue("@MA_HDV", (object)maHDV ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MA_TOUR", (object)maTour ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NGAY_PC", (object)ngayPC ?? DBNull.Value);

                    // Thực thi và nhận kết quả
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Hiển thị kết quả vào DataGridView
                    DGV_search.DataSource = dt;
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm thông tin phân công: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2GroupBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
