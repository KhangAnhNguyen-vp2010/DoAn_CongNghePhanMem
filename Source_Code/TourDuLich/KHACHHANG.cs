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
namespace KHACHHANG__PC_TOUR
{
    public partial class KHACHHANG : Form
    {
        KetNoi kn = new KetNoi();
        SqlConnection conn;
        public KHACHHANG()
        {
            conn = kn.conn;
            InitializeComponent();
        }

        void load_dskh()
        {
            string sql = "SELECT * FROM KHACHHANG";
            DataTable dataTable1 = new DataTable();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    conn.Open();
                    adapter.Fill(dataTable1);


                    if (dataTable1.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DGV_Khachhang.DataSource = dataTable1;
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

        private void btn_xem_Click(object sender, EventArgs e)
        {
            XemKH xem = new XemKH();
            xem.Show();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            txt_makh.Text = Tao_MAKH();
            txt_tenkh.Clear();
            txt_email.Clear();
            txt_sdt.Clear();
            txt_diachi.Clear();
            txt_cccd.Clear();

            txt_tenkh.Enabled = true;
            txt_email.Enabled = true;
            txt_sdt.Enabled = true;
            txt_diachi.Enabled = true;
            txt_cccd.Enabled = true;

            btn_them.Enabled = false;
            btn_luu.Enabled = true;
            btn_sua.Enabled = true;
        }

        void themKH()
        {
            try
            {
                if (!kiemtra_rong())
                {

                    string makh = Tao_MAKH();
                    string cccd = txt_cccd.Text;
                    string tenkh = txt_tenkh.Text;
                    string email = txt_email.Text.Trim();
                    string diachi = txt_diachi.Text;
                    string sdt = txt_sdt.Text;

                    // Kiểm tra nếu makh đã tồn tại trong cơ sở dữ liệu
                    string kiemtra_makh = "SELECT COUNT(*) FROM KHACHHANG WHERE MA_KH = @makh";

                    using (SqlCommand checkCommand = new SqlCommand(kiemtra_makh, conn))
                    {
                        conn.Open();
                        checkCommand.Parameters.AddWithValue("@makh", txt_makh.Text);
                        int count = (int)checkCommand.ExecuteScalar();
                        conn.Close();
                        if (count > 0)
                        {
                            // Nếu makh đã tồn tại, thông báo lỗi
                            MessageBox.Show("Mã khách hàng đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }




                    // Tạo một đối tượng SqlCommand để thực thi câu lệnh INSERT
                    SqlCommand command = new SqlCommand("EXEC PROC_THEM_KHACHHANG @MAKH, @TENKH, @EMAIL, @SDT, @DIACHI, @CCCD;", conn);

                    // Gán giá trị từ các control vào các tham số của command
                    command.Parameters.AddWithValue("@MAKH", makh);
                    command.Parameters.AddWithValue("@TENKH", tenkh);
                    command.Parameters.AddWithValue("@EMAIL", email);
                    command.Parameters.AddWithValue("@SDT", sdt);
                    command.Parameters.AddWithValue("@DIACHI", diachi);
                    command.Parameters.AddWithValue("@CCCD", cccd);


                    // Mở kết nối và thực thi câu lệnh
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();

                    btn_xoa.Enabled = true;
                    btn_them.Enabled = true;
                    // Load lại dữ liệu lên DataGridView
                    load_dskh();

                    // Thông báo thêm thành công
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Cần điển đầy đủ thông tin bắt buộc !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                conn.Close();
            }
        }
        bool kiemtra_rong()
        {
            if (string.IsNullOrEmpty(txt_makh.Text)
                || string.IsNullOrEmpty(txt_tenkh.Text)
                || string.IsNullOrEmpty(txt_email.Text)
                || string.IsNullOrEmpty(txt_sdt.Text)
                || string.IsNullOrEmpty(txt_diachi.Text)
                || string.IsNullOrEmpty(txt_cccd.Text)
                )
            {

                return true; // Dừng việc thực hiện các lệnh bên dưới
            }
            return false;
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            xoa_KH();
        }
        void xoa_KH()
        {
            string makh = txt_makh.Text;

            // Xác nhận xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng có mã " + makh + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra nếu makh đã tồn tại trong cơ sở dữ liệu
            string kiemtra_makh = "SELECT COUNT(*) FROM KHACHHANG WHERE MA_KH = @makh";
            using (SqlCommand checkCommand = new SqlCommand(kiemtra_makh, conn))
            {
                checkCommand.Parameters.AddWithValue("@makh", txt_makh.Text);
                conn.Open();
                int count = (int)checkCommand.ExecuteScalar();
                conn.Close();

                if (count == 0)
                {
                    // Nếu makh không tồn tại, thông báo lỗi
                    MessageBox.Show("Mã khách hàng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (result == DialogResult.Yes)
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

                    string sql = "EXEC PROC_XOA_KHACHHANG @MAKH";
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@MAKH", makh);
                        // Đăng ký sự kiện InfoMessage trước khi mở kết nối
                        conn.InfoMessage += infoMessageHandler;
                        conn.Open();
                        command.ExecuteNonQuery();
                        // Hủy đăng ký sự kiện InfoMessage và đóng kết nối
                        conn.InfoMessage -= infoMessageHandler;
                        conn.Close();
                    }
                    btn_xoa.Enabled = true;
                    btn_them.Enabled = true;
                    load_dskh(); // Hàm để load lại dữ liệu vào DataGridView

                    //MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng : " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        private void DGV_Khachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_them.Enabled = true;
            btn_sua.Enabled = true;
            txt_makh.Enabled = false;
            txt_tenkh.Enabled = false;
            txt_email.Enabled = false;
            txt_sdt.Enabled = false;
            txt_diachi.Enabled = false;
            txt_cccd.Enabled = false;
            if (e.RowIndex >= 0)
            {

                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DGV_Khachhang.Rows[e.RowIndex];

                string makh = row.Cells["MA_KH"].Value.ToString();
                string tenkh = row.Cells["TEN_KH"].Value.ToString();
                string email = row.Cells["EMAIL"].Value.ToString();
                string sdt = row.Cells["SDT"].Value.ToString();
                string diachi = row.Cells["DIA_CHI"].Value.ToString();
                string cccd = row.Cells["CCCD"].Value.ToString();
                // Đổ dữ liệu vào các TextBox
                txt_makh.Text = makh;
                txt_tenkh.Text = tenkh;
                txt_email.Text = email;
                txt_sdt.Text = sdt;
                txt_diachi.Text = diachi;
                txt_cccd.Text = cccd;
            }
        }

        private void KHACHHANG_Load(object sender, EventArgs e)
        {

            load_dskh();
            btn_luu.Enabled = false;
            txt_makh.Enabled = false;
            txt_tenkh.Enabled = false;
            txt_email.Enabled = false;
            txt_sdt.Enabled = false;
            txt_diachi.Enabled = false;
            txt_cccd.Enabled = false;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_makh.Clear();
            txt_tenkh.Clear();
            txt_email.Clear();
            txt_sdt.Clear();
            txt_diachi.Clear();
            txt_cccd.Clear();
        }

        private string Tao_MAKH()
        {
            string mamoi = "KH001";
            conn.Open();

            // Lấy mã khách hàng lớn nhất
            string query = "SELECT TOP 1 MA_KH FROM KHACHHANG ORDER BY MA_KH DESC";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    // Lấy mã hiện tại và tăng lên 1
                    string malonnhat = result.ToString(); // Ví dụ: "KH001"
                    int phanso = int.Parse(malonnhat.Substring(2)); // Lấy phần số: 001
                    phanso++; // Tăng lên 1
                    mamoi = "KH" + phanso.ToString("D3"); // Tạo mã mới: "KH002"
                }
                conn.Close();
            }
            conn.Close();


            return mamoi;
        }


        private void btn_luu_Click(object sender, EventArgs e)
        {

            if (btn_them.Enabled == false)
            {
                themKH();

            }
            if (btn_sua.Enabled == false)
            {
                suaKH();
            }




        }

        void suaKH()
        {
            string cccd = txt_cccd.Text;
            string makh = txt_makh.Text;
            string tenkh = txt_tenkh.Text;
            string email = txt_email.Text.Trim();
            string diachi = txt_diachi.Text;
            string sdt = txt_sdt.Text;


            // Xác nhận sửa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn sửa thông tin khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            string kiemtra_makh = "SELECT COUNT(*) FROM KHACHHANG WHERE MA_KH = @makh";
            using (SqlCommand checkCommand = new SqlCommand(kiemtra_makh, conn))
            {
                try
                {


                    checkCommand.Parameters.AddWithValue("@makh", txt_makh.Text);
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    conn.Close();

                    if (count == 0)
                    {
                        // Nếu makh không tồn tại, thông báo lỗi
                        MessageBox.Show("Mã khách hàng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Lỗi sửa thông tin:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            if (result == DialogResult.Yes)
            {
                try
                {
                    if (!kiemtra_rong())
                    {


                        // Câu lệnh SQL để cập nhật thông tin khách hàng
                        string sql = "UPDATE KHACHHANG SET TEN_KH = @TENKH, EMAIL = @EMAIL, SDT = @SDT, DIA_CHI = @DIACHI, CCCD = @CCCD WHERE MA_KH = @MAKH";
                        using (SqlCommand command = new SqlCommand(sql, conn))
                        {
                            command.Parameters.AddWithValue("@MAKH", makh);
                            command.Parameters.AddWithValue("@TENKH", tenkh);
                            command.Parameters.AddWithValue("@EMAIL", email);
                            command.Parameters.AddWithValue("@SDT", sdt);
                            command.Parameters.AddWithValue("@DIACHI", diachi);
                            command.Parameters.AddWithValue("@CCCD", cccd);


                            conn.Open();
                            command.ExecuteNonQuery();
                            conn.Close();

                        }


                        load_dskh(); ;

                        MessageBox.Show("Sửa thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cần điển đầy đủ thông tin bắt buộc !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Lỗi khi sửa thông tin khách hàng: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btn_sua_Click(object sender, EventArgs e)
        {
            btn_sua.Enabled = false;
            btn_luu.Enabled = true;
            btn_them.Enabled = true;

            txt_tenkh.Enabled = true;
            txt_email.Enabled = true;
            txt_sdt.Enabled = true;
            txt_diachi.Enabled = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string sql = "select * from khachhang";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            DGV_Khachhang.DataSource = dt;
        }
    }
}
