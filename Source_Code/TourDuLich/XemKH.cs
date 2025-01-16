using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using TheArtOfDevHtmlRenderer.Adapters;
using TourDuLich;

namespace KHACHHANG__PC_TOUR
{
    
    public partial class XemKH : Form
    {
        KetNoi kn = new KetNoi();
        SqlConnection conn;
        public XemKH()
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
                        DGV_dskh.DataSource = dataTable1;
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
        private void XemKH_Load(object sender, EventArgs e)
        {
            load_dskh();
        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            string tenkh = txt_tenkh.Text;
            string sdt = txt_sdt.Text;
            string email = txt_email.Text;
            string diachi = txt_diachi.Text;
            string cccd = txt_cccd.Text;

            // Cấu trúc SQL để gọi thủ tục
            string sql = "EXEC PROC_TIM_KHACHHANG @TEN_KH, @SDT, @EMAIL, @DIACHI, @CCCD";

            // Tạo command và thêm các tham số
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Thêm tham số cho câu lệnh SQL, nếu textbox trống thì truyền NULL vào

                cmd.Parameters.AddWithValue("@TEN_KH", string.IsNullOrEmpty(tenkh) ? (object)DBNull.Value : "%" + tenkh + "%");
                cmd.Parameters.AddWithValue("@SDT", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
                cmd.Parameters.AddWithValue("@EMAIL", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                cmd.Parameters.AddWithValue("@DIACHI", string.IsNullOrEmpty(diachi) ? (object)DBNull.Value : "%" + diachi + "%");
                cmd.Parameters.AddWithValue("@CCCD", string.IsNullOrEmpty(cccd) ? (object)DBNull.Value : cccd);

                // Mở kết nối và thực thi câu lệnh
                conn.Open();

                // Sử dụng SqlDataAdapter để điền dữ liệu vào DataGridView
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DGV_dskh.DataSource = dt;  // Cập nhật DataGridView
                }

                // Đóng kết nối
                conn.Close();
            }
        }


        private void DGV_dskh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DGV_dskh.Rows[e.RowIndex];

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

       
        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_makh.Clear();
            txt_tenkh.Clear();
            txt_email.Clear();
            txt_sdt.Clear();
            txt_diachi.Clear();
            txt_cccd.Clear();
        }

        private void btn_dstt_Click(object sender, EventArgs e)
        {
            //string sql = "SELECT KH.MA_KH,  KH.TEN_KH,  KH.CCCD,KH.EMAIL FROM KHACHHANG KH WHERE DBO.FUNC_TONGTHANHTOAN_KHACHHANG(KH.MA_KH) IS NULL OR DBO.FUNC_TONGTHANHTOAN_KHACHHANG(KH.MA_KH) = 0;";
            string sql = "SELECT KH.MA_KH,  KH.TEN_KH,  KH.CCCD,KH.EMAIL, trangthai FROM KHACHHANG as KH, HOADON WHERE KH.MA_KH = HOADON.MA_KH AND TRANGTHAI = N'Chưa Thanh Toán' group by KH.MA_KH,  KH.TEN_KH,  KH.CCCD,KH.EMAIL, trangthai";
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
                        DGV_dstt.DataSource = dataTable1;
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

        private void btn_guithongbao_Click(object sender, EventArgs e)
        {
            // Chỉnh con trỏ thành "WaitCursor" khi bắt đầu thao tác
            this.Cursor = Cursors.WaitCursor;

            // Kết nối với cơ sở dữ liệu
            //string sqlQuery = @"
            //    SELECT KH.MA_KH, KH.TEN_KH, KH.CCCD, KH.EMAIL 
            //    FROM KHACHHANG KH 
            //    WHERE DBO.FUNC_TONGTHANHTOAN_KHACHHANG(KH.MA_KH) IS NULL 
            //       OR DBO.FUNC_TONGTHANHTOAN_KHACHHANG(KH.MA_KH) = 0;";

            string sqlQuery = @"SELECT KH.MA_KH,  KH.TEN_KH,  KH.CCCD,KH.EMAIL, trangthai FROM KHACHHANG as KH, HOADON WHERE KH.MA_KH = HOADON.MA_KH AND TRANGTHAI = N'Chưa Thanh Toán' group by KH.MA_KH,  KH.TEN_KH,  KH.CCCD,KH.EMAIL, trangthai";

            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Duyệt qua các dòng dữ liệu và gửi thông báo cho mỗi khách hàng
                    foreach (DataRow row in dt.Rows)
                    {
                        string maKh = row["MA_KH"].ToString();
                        string tenKh = row["TEN_KH"].ToString();
                        string email = row["EMAIL"].ToString();

                        // Gửi email thông báo
                        SendEmail(tenKh, email, maKh);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đặt lại con trỏ về mặc định khi thao tác hoàn thành
                this.Cursor = Cursors.Default;
            }
        }

        private void SendEmail(string customerName, string customerEmail, string makh)
        {
            try
            {

                string sql1 = string.Format("EXEC GuiMail_HOADON '{0}'", makh);
                SqlDataAdapter a2 = new SqlDataAdapter(sql1, conn);
                DataTable dt2 = new DataTable();
                a2.Fill(dt2);

                // Tạo danh sách hóa đơn dưới dạng chuỗi
                StringBuilder invoiceList = new StringBuilder();
                invoiceList.AppendLine("Danh sách hóa đơn chưa thanh toán:\n");
                invoiceList.AppendLine("STT\tMã Hóa Đơn\tTên Tour\tNgày Lập\tSố Lượng\t\t\tGiá Tiền\t\t\tTrạng Thái");

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow row = dt2.Rows[i];
                    string maHD = row["Ma_HD"].ToString();
                    string tenTour = row["ten_tour"].ToString();
                    DateTime ns = DateTime.Parse(row["Ngay_Lap"].ToString());
                    string ngayLap = ns.ToString("dd-MM-yyyy");
                    string sl = row["So_luong"].ToString();
                    string soTien = row["tongTien"].ToString();
                    string tt = row["trangthai"].ToString();

                    // Định dạng danh sách
                    invoiceList.AppendLine($"{i + 1}\t\t{maHD}\t   {tenTour}\t   {ngayLap}\t\t{sl}\t\t\t{soTien} VNĐ\t\t{tt}");
                }


                // Cấu hình thông tin tài khoản email gửi
                var fromEmail = "tourdulichanhchiem@gmail.com";  // Thay bằng email của bạn
                var fromPassword = "ccjc elug qlvf nkqb"; // Mật khẩu ứng dụng Gmail
                var toEmail = customerEmail;
                var subject = "Thông báo thanh toán tour";
                var body = $"Chào {customerName},\n\n" +
                   "Công ty chúng tôi xin gửi bạn danh sách các hóa đơn chưa thanh toán:\n\n" +
                   $"{invoiceList}\n\n" +
                   "Vui lòng thanh toán sớm để tiếp tục sử dụng dịch vụ.\n\nTrân trọng!!!";

                // Tạo đối tượng SmtpClient và cấu hình SMTP server
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                // Tạo email
                MailMessage mail = new MailMessage(fromEmail, toEmail, subject, body);

                // Gửi email
                smtpClient.Send(mail);
                MessageBox.Show($"Email thông báo đã được gửi đến {customerName} ({customerEmail})!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi email: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
