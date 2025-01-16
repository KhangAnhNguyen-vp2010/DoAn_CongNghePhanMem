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

namespace TourDuLich
{
    public partial class DanhGia_For_KH : Form
    {
        SqlConnection cn;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        SqlCommand cmd;
        GiaoDienChoKhachHang f;
        public DanhGia_For_KH(string madanhgia, GiaoDienChoKhachHang gd)
        {
            InitializeComponent();
            cn = kn.conn;
            maDanhGia = madanhgia;
            f = gd;
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
                }
            }
        }
        private void DanhGia_For_KH_Load(object sender, EventArgs e)
        {
            // Không cho phép kéo giãn kích thước Form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Vô hiệu hoá nút phóng to
            this.MaximizeBox = false;

            // (Tuỳ chọn) Vô hiệu hoá nút thu nhỏ
            this.MinimizeBox = false;
            load_DuLieu();
        }
        string maDanhGia;
        int soDiem;
        void load_DuLieu()
        {
            string sql = string.Format("SELECT * FROM TOUR, DANHGIA WHERE TOUR.MA_TOUR = DANHGIA.MA_TOUR AND MA_DG = '{0}'", maDanhGia);
            adapter = new SqlDataAdapter(sql, cn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                guna2HtmlLabel_matour.Text = row["MA_TOUR"].ToString();
                guna2HtmlLabel_ten.Text = row["TEN_TOUR"].ToString();
                soDiem = int.Parse(row["DIEM_DG"].ToString());
                string img = row["HINHANH"].ToString();
                guna2RatingStar_danhgia.Value = soDiem;
                string relativeImagePath = img; // Đường dẫn tương đối

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
        }

        private void guna2Button_guidanhgia_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                string sql = string.Format("update danhgia set Diem_dg = '{0}' where ma_dg = '{1}'", guna2RatingStar_danhgia.Value, maDanhGia);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("ĐÁNH GIÁ THÀNH CÔNG !!! CẢM ƠN QUÝ KHÁCH", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                f.loadDanhGia();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { cn.Close(); }
            this.Close();
        }
    }
}
