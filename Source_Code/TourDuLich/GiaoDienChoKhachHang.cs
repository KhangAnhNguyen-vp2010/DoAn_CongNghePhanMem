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
using Guna.UI2.WinForms.Suite;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.SqlClient;
using TourDuLich;
using WindowsFormsApp1;

namespace QuanLyTourDuLich
{
    public partial class GiaoDienChoKhachHang : Form
    {
        SqlConnection cn;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        SqlCommand cmd;
        int currentPage = 1; // Trang hiện tại
        int pageSize = 6;    // 6 tour/trang
        int totalPages = 0;
        float currentDpi;
        public GiaoDienChoKhachHang()
        {
            InitializeComponent();
            cn = kn.conn;
            AdjustFormSize();
        }

        

        private void AdjustFormSize()
        {
            using (Graphics g = this.CreateGraphics())
            {
                currentDpi = g.DpiX; // DPI hiện tại
                if (currentDpi == 96)
                {
                    this.Width += 50;
                    guna2TabControl1.Width += 50;
                    guna2PictureBox1.Width += 50;
                    guna2HtmlLabel1.Height += 1;
                    guna2ShadowPanel1.Location = new Point(guna2PictureBox1.Width+2, 0);
                    guna2HtmlLabel2.Width += 1;
                    guna2HtmlLabel3.Width += 1;
                    guna2HtmlLabel4.Width += 1;
                    guna2HtmlLabel5.Width += 1;
                    guna2HtmlLabel6.Width += 1;
                    guna2HtmlLabel7.Width += 1;
                    guna2HtmlLabel8.Width += 1;
                    guna2HtmlLabel9.Width += 1;
                    guna2HtmlLabel10.Width += 1;
                    guna2HtmlLabel11.Width += 1;
                    guna2HtmlLabel12.Width += 1;
                    guna2HtmlLabel13.Width += 1;
                    guna2HtmlLabel14.Width += 1;
                    guna2HtmlLabel15.Width += 1;
                    guna2HtmlLabel16.Width += 1;
                    guna2HtmlLabel17.Width += 1;
                    guna2HtmlLabel18.Width += 1;
                    guna2HtmlLabel19.Width += 1;
                    guna2HtmlLabel20.Width += 1;
                    guna2HtmlLabel21.Width += 1;
                    guna2HtmlLabel22.Width += 1;
                    
                }
            }
        }
        Form temp;
        public void hihi()
        {
            temp.Close();
            totalPages = 0;
            currentPage = 1; // Trang hiện tại
            pageSize = 6;    // 6 tour/trang
            load_Tour(1);
            // Không cho phép kéo giãn kích thước Form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            cn.Open();
            string sql1 = "SELECT COUNT(*) FROM tour";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            int totalItems = (int)cmd.ExecuteScalar();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            cn.Close();
        }

        void dongFormCT(Form t)
        {
            temp = t;
        }

        void enableTextBox_KH(int i)
        {
            if (i == 1)
            {
                guna2TextBox_tenkh.Enabled = false;
                guna2TextBox_mail.Enabled = false;
                guna2TextBox_diachi.Enabled = false;
                guna2TextBox_sdt.Enabled = false;
            }
            else
            {
                guna2TextBox_tenkh.Enabled = true;
                guna2TextBox_mail.Enabled = true;
                guna2TextBox_diachi.Enabled = true;
                guna2TextBox_sdt.Enabled = true;
            }

        }

        void load_Tour(int pageIndex)
        {
            string sql = $@"
            SELECT * 
            FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY Ma_Tour) AS RowNum, * 
                FROM (SELECT * 
                      FROM TOUR 
                      WHERE ( TEN_TOUR LIKE '%' + N'{guna2TextBox_search.Text}' + '%' 
	                    OR TG_TOUR LIKE '%' + N'{guna2TextBox_search.Text}' + '%' 
	                    OR GIA_TOUR LIKE '%' + N'{guna2TextBox_search.Text}' + '%')) AS T
            ) AS Result
            WHERE (RowNum BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize})";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.BackColor = Color.FromArgb(255, 224, 192);
            foreach (DataRow row in dt.Rows)
            {
                string name = row["TEN_TOUR"].ToString();
                string tg = row["TG_TOUR"].ToString();
                DateTime date = DateTime.Parse(tg);
                string tg2 = date.ToString("dd-MM-yyyy");
                int gia = Convert.ToInt32(row["GIA_TOUR"]);
                string img = row["HINHANH"].ToString();

                string relativeImagePath = img; // Đường dẫn tương đối

                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
                string imageDirectory = Path.Combine(Application.StartupPath, "image", "tour"); // Thư mục chứa hình ảnh

                // Kết hợp thư mục gốc với đường dẫn tương đối để có đường dẫn tuyệt đối
                string imagePath = Path.Combine(imageDirectory, relativeImagePath);


                // Tạo Panel làm thẻ card
                Guna.UI2.WinForms.Guna2Panel card = new Guna.UI2.WinForms.Guna2Panel
                {
                    Width = 400,
                    Height = 480,
                    Margin = new Padding(14),
                    Padding = new Padding(10),
                    BorderColor = Color.Gray,
                    BorderThickness = 1,
                    BorderRadius = 15,

                };

                Guna.UI2.WinForms.Guna2PictureBox picture = new Guna.UI2.WinForms.Guna2PictureBox
                {

                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 200,
                    Height = 350,
                    Dock = DockStyle.Bottom,
                    BorderRadius = 5,

                };

                if (File.Exists(imagePath)) // Kiểm tra nếu file tồn tại
                {
                    picture.Image = Image.FromFile(imagePath); // Chuyển đổi đường dẫn thành hình ảnh
                }
                else
                {
                    picture.Image = null; // Nếu không tồn tại, đặt giá trị là null
                }


                Guna.UI2.WinForms.Guna2HtmlLabel lblName = new Guna.UI2.WinForms.Guna2HtmlLabel
                {
                    Text = name,
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    TextAlignment = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Dock = DockStyle.Top,
                    Height = 40
                };

                //Guna.UI2.WinForms.Guna2HtmlLabel lbltg = new Guna.UI2.WinForms.Guna2HtmlLabel
                //{
                //    Text = "Ngày Tổ Chức: " + tg2,
                //    //Font = new Font("Arial", 14, FontStyle.Italic),
                //    ForeColor = Color.Black,
                //    AutoSize = false,
                //    Dock = DockStyle.Bottom,
                //    Height = 30,
                //    Width = 200,
                //    TextAlignment = ContentAlignment.MiddleCenter
                //};


                Guna.UI2.WinForms.Guna2HtmlLabel lblPrice = new Guna.UI2.WinForms.Guna2HtmlLabel
                {
                    
                    Font = new Font("Arial", 14, FontStyle.Italic),
                    ForeColor = Color.Green,
                    Dock = DockStyle.Bottom,
                    AutoSize = true,
                    TextAlignment = ContentAlignment.MiddleCenter,
                };

                if (currentDpi == 96)
                {
                    lblPrice.Text = "===========Giá: " + gia.ToString() + "Đ===========";
                }
                else
                {
                    lblPrice.Text = "======Giá: " + gia.ToString() + "Đ======";
                }

                Guna.UI2.WinForms.Guna2Button btnDetails = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = "Xem chi tiết",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    BorderRadius = 10,
                    FillColor = Color.DodgerBlue,
                    ForeColor = Color.White,
                    Margin = new Padding(30),
                    Font = new Font("Arial", 14, FontStyle.Italic),
                    Cursor = Cursors.Hand
                };
                btnDetails.Click += new EventHandler(btnDetails_Click);

                // Lưu trữ thông tin tour vào Tag của nút
                btnDetails.Tag = row;  // Lưu cả row dữ liệu vào Tag

                card.Controls.Add(lblName);
                card.Controls.Add(picture);
                //card.Controls.Add(lbltg);
                card.Controls.Add(lblPrice);
                card.Controls.Add(btnDetails);

                flowLayoutPanel1.Controls.Add(card);
            }


        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;  // Lấy button hiện tại
            DataRow tour = btn.Tag as DataRow;  // Lấy dữ liệu của tour từ Tag của button
            cn.Open();
            string sql = string.Format("select ten_diem_den from diemden where ma_dia_diem = '{0}'", tour["MA_DIA_DIEM"].ToString());
            cmd = new SqlCommand(sql, cn);
            string tenDD = cmd.ExecuteScalar().ToString();
            string sql1 = string.Format("select dia_chi from diemden where ma_dia_diem = '{0}'", tour["MA_DIA_DIEM"].ToString());
            cmd = new SqlCommand(sql1, cn);
            string dia_chi = cmd.ExecuteScalar().ToString();
            string sql2 = string.Format("select * from LICHTRINH where MA_TOUR = '{0}'", tour["MA_tour"].ToString());
            adapter = new SqlDataAdapter(sql2, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            cn.Close();
            DatTourStaTic.maTour = tour["ma_tour"].ToString();
            string ngaybd = null, ngaykt = null;
            DateTime temp = new DateTime();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)
                DateTime date1 = DateTime.Parse(row[2].ToString());
                DateTime date2 = DateTime.Parse(row[3].ToString());
                // Lấy dữ liệu từng cột theo tên cột
                temp = date1;
                ngaybd = date1.ToString("dd-MM-yyyy");
                ngaykt = date2.ToString("dd-MM-yyyy");


                DatTourStaTic.maLT = row[1].ToString();

            }
            if (!string.IsNullOrEmpty(ngaybd))
            {
                dk_nagyBD = temp;

            }


            // Tạo form chi tiết
            Form detailsForm = new Form
            {
                Text = "Chi Tiết Tour",
                Size = new Size(1050, 600), // Đặt kích thước form
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(255, 192, 255)
            };

            int formWidth = detailsForm.Width;

            // Các thông tin hiển thị bên dưới hình ảnh
            Guna2HtmlLabel lblMaTour = new Guna2HtmlLabel
            {
                Text = "Mã Tour: " + tour["MA_TOUR"].ToString(),
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 20), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            Guna2HtmlLabel lblTourName = new Guna2HtmlLabel
            {
                Text = "Tên Tour: " + tour["Ten_Tour"].ToString(),
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 60), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            // Hiển thị hình ảnh
            Guna2PictureBox pictureBox = new Guna2PictureBox
            {
                Size = new Size(500, 500),
                Location = new Point(10, 20), // Căn giữa hình ảnh theo chiều ngang
                BorderRadius = 10,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            string relativeImagePath = tour["HINHANH"].ToString();
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            string imageDirectory = Path.Combine(Application.StartupPath, "image", "tour");
            string imagePath = Path.Combine(imageDirectory, relativeImagePath);
            if (File.Exists(imagePath))
            {
                pictureBox.Image = Image.FromFile(imagePath);
            }
            else
            {
                pictureBox.Image = null;
            }

            DateTime date = DateTime.Parse(tour["TG_TOUR"].ToString());
            Guna2HtmlLabel lblTime = new Guna2HtmlLabel
            {
                Text = "Ngày Tổ Chức: " + date.ToString("dd-MM-yyyy"),
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 100), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            Guna2HtmlLabel lblDestination = new Guna2HtmlLabel
            {
                Text = "Điểm đến: " + (string.IsNullOrEmpty(tenDD) ? "Không xác định" : tenDD),
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 140), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            Guna2HtmlLabel lbldiachi = new Guna2HtmlLabel
            {
                Text = "Địa Chỉ: " + dia_chi,
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 180), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            Guna2HtmlLabel lbllichtrinh = new Guna2HtmlLabel
            {
                Text = "Lịch Trình: " + (string.IsNullOrEmpty(ngaybd) ? "Đang Cập Nhật" : (ngaybd.ToString() + " -> " + ngaykt)),
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 220), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            flags_lt = lbllichtrinh.Text;

            Guna2HtmlLabel lblsl_ton = new Guna2HtmlLabel
            {
                Text = "Số Lượng Vé Còn Lại: " + ((int)tour["SL"] == 0 ? "Đã Hết Vé" : tour["SL"].ToString()),
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 260), // Căn giữa theo chiều ngang
                ForeColor = Color.Black,
                MaximumSize = new Size(500, 0)
            };

            flags_sl_ve = (int)tour["SL"];

            Guna2HtmlLabel lblPrice = new Guna2HtmlLabel
            {
                Text = "Giá Tour: " + Convert.ToInt32(tour["GIA_TOUR"]).ToString("N0") + " VND",
                Font = new Font("Arial", 14, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(520, 300), // Căn giữa theo chiều ngang
                ForeColor = Color.Green,
                MaximumSize = new Size(500, 0)
            };

            // Nút Đặt Tour
            Guna2Button btnBookTour = new Guna2Button
            {
                Text = "Đặt Tour",
                Size = new Size(150, 40),
                Location = new Point(700, 400), // Căn giữa theo chiều ngang
                BorderRadius = 10,
                FillColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Height = 80,
                Cursor = Cursors.Hand
            };

            btnBookTour.Click += new EventHandler(btnBookTour_Click);
            dongFormCT(detailsForm);
            btnBookTour.Tag = tour;

            a = tour["ma_tour"].ToString();
            b = tour["ten_tour"].ToString();
            c = tour["gia_tour"].ToString();
            d = tour["sl"].ToString();

            // Thêm các control vào form
            detailsForm.Controls.Add(lblMaTour);
            detailsForm.Controls.Add(lblTourName);
            detailsForm.Controls.Add(pictureBox);
            detailsForm.Controls.Add(lblTime);
            detailsForm.Controls.Add(lblDestination);
            detailsForm.Controls.Add(lbldiachi);
            detailsForm.Controls.Add(lbllichtrinh);
            detailsForm.Controls.Add(lblsl_ton);
            detailsForm.Controls.Add(lblPrice);
            detailsForm.Controls.Add(btnBookTour);

            detailsForm.ShowDialog();  // Hiển thị form chi tiết


        }
        string a, b, c, d;
        string flags_lt = null;
        int flags_sl_ve;
        DateTime dk_nagyBD;
        void btnBookTour_Click(object sender, EventArgs e)
        {
            if (flags_lt == "Lịch Trình: Đang Cập Nhật")
            {
                MessageBox.Show("Hiện Tại Chưa Có Lịch Trình Cho Tour Này!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime now = DateTime.Today;
            if (now >= dk_nagyBD)
            {
                MessageBox.Show("Tour Này Hiện Tại Đã Tới Ngày Khởi Hành! Không Thể Đặt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (flags_sl_ve == 0)
            {
                MessageBox.Show("Rất Tiêc Vé Đã Hết!!! Không Thể Đặt Tour.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DatTour_ForKH f = new DatTour_ForKH(a, b, c, d, this);
            f.Show();

        }


        private void GiaoDienChoKhachHang_Load(object sender, EventArgs e)
        {
            guna2TextBox_search.Hide();
            guna2Button_search.Hide();
            guna2Button_cancel.Hide();
            guna2Button_submit.Hide();
            enableTextBox_KH(1);
            Load_DuLieu_KH();
            load_Tour(1);
            // Không cho phép kéo giãn kích thước Form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            cn.Open();
            string sql1 = "SELECT COUNT(*) FROM tour";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            int totalItems = (int)cmd.ExecuteScalar();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            cn.Close();
        }

        private void guna2Button_Next_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                load_Tour(currentPage);
            }
        }

        private void guna2Button_Prev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                load_Tour(currentPage);
            }
        }

        void tim_TourDuLich()
        {
            totalPages = 0;
            currentPage = 1; // Trang hiện tại
            pageSize = 6;    // 6 tour/trang
            load_Tour(1);
            cn.Open();
            string sql1 = $@"SELECT COUNT(*) FROM tour WHERE (TEN_TOUR LIKE '%' + N'{guna2TextBox_search.Text}' + '%' OR TG_TOUR LIKE '%' + N'{guna2TextBox_search.Text}' + '%' OR GIA_TOUR LIKE '%' + N'{guna2TextBox_search.Text}' + '%')";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            int totalItems = (int)cmd.ExecuteScalar();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            cn.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tim_TourDuLich();
        }

        private void guna2Button_load_Click(object sender, EventArgs e)
        {
            totalPages = 0;
            currentPage = 1; // Trang hiện tại
            pageSize = 6;    // 6 tour/trang
            guna2TextBox_search.Text = null;
            load_Tour(1);
            cn.Open();
            string sql1 = "SELECT COUNT(*) FROM tour";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            int totalItems = (int)cmd.ExecuteScalar();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            cn.Close();
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2TabControl1.SelectedIndex == 1 || guna2TabControl1.SelectedIndex == 2)
            {
                guna2TextBox_search.Show();
                guna2Button_search.Show();
                load_HoaDon();
            }
            else
            {
                guna2TextBox_search.Hide();
                guna2Button_search.Hide();
            }

            if (guna2TabControl1.SelectedIndex == 0)
            {
                guna2Button_cancel.Hide();
                guna2Button_submit.Hide();
                Load_DuLieu_KH();
            }

            if (guna2TabControl1.SelectedIndex == 3)
            {
                loadDanhGia();
            }
        }

        private void guna2Button_suaThongTinKH_Click(object sender, EventArgs e)
        {
            guna2Button_cancel.Show();
            guna2Button_submit.Show();
            enableTextBox_KH(0);
            guna2TextBox_cccd.Enabled = true;
        }

        private void guna2Button_cancel_Click(object sender, EventArgs e)
        {
            guna2Button_cancel.Hide();
            guna2Button_submit.Hide();
            enableTextBox_KH(1);
            Load_DuLieu_KH();
            guna2TextBox_cccd.Enabled = false;
        }

        void Load_DuLieu_KH()
        {
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                guna2TextBox_makh.Text = row[0].ToString();
                guna2TextBox_tenkh.Text = row[1].ToString();
                guna2TextBox_mail.Text = row[2].ToString();
                guna2TextBox_sdt.Text = row[3].ToString();
                guna2TextBox_diachi.Text = row[4].ToString();
                guna2TextBox_cccd.Text = row[5].ToString();
            }
        }

        private void guna2Button_submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_tenkh.Text) || string.IsNullOrEmpty(guna2TextBox_mail.Text) || string.IsNullOrEmpty(guna2TextBox_sdt.Text) || string.IsNullOrEmpty(guna2TextBox_diachi.Text))
            {
                MessageBox.Show("Nhập Đầy Đủ Dữ Liệu");
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(guna2TextBox_sdt.Text, @"^\d+$") || guna2TextBox_sdt.Text.Length != 10)
                {
                    MessageBox.Show("Số điện thoại là toàn là kí tự số và bao gôm 10 số!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    cn.Open();
                    string sql = string.Format("EXEC CHINHSUA_PROFILE_KH '{0}', N'{1}', N'{2}', N'{3}', N'{4}', '{5}'", guna2TextBox_makh.Text, guna2TextBox_tenkh.Text, guna2TextBox_mail.Text, guna2TextBox_sdt.Text, guna2TextBox_diachi.Text, guna2TextBox_cccd.Text);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    Load_DuLieu_KH();
                    enableTextBox_KH(1);
                    guna2TextBox_cccd.Enabled = false;
                    guna2Button_cancel.Hide();
                    guna2Button_submit.Hide();
                    MessageBox.Show("Chỉnh Sửa Thành Công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    cn.Close();
                }

            }
        }

        public void load_HoaDon()
        {
            guna2DataGridView_hoadon.RowTemplate.Height = 80;
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string makh = null;
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                makh = row[0].ToString();

            }
            string sql1 = "EXEC SHOW_HOADON '" + makh + "'";
            adapter = new SqlDataAdapter(sql1, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView_hoadon.DataSource = dt;

            // Kiểm tra xem DataGridView có dữ liệu hay không
            if (guna2DataGridView_hoadon.Rows.Count > 0)
            {
                guna2HtmlLabel_sumhd.Text = guna2DataGridView_hoadon.RowCount.ToString();
                // Chọn dòng đầu tiên
                guna2DataGridView_hoadon.ClearSelection();
                guna2DataGridView_hoadon.Rows[0].Selected = true;
                guna2DataGridView_hoadon.CurrentCell = guna2DataGridView_hoadon.Rows[0].Cells[0];

                // Gọi sự kiện CellClick cho dòng đầu tiên
                guna2DataGridView_hoadon_CellClick(
                    guna2DataGridView_hoadon,
                    new DataGridViewCellEventArgs(0, 0) // Cột 0, dòng 0
                );
            }
            else
            {
                guna2Button_thantoan.Hide();
                guna2Button_huy.Hide();
                guna2HtmlLabel_sumhd.Text = (0).ToString();
            }
        }

        string maDat;
        private void guna2DataGridView_hoadon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    maDat = guna2DataGridView_hoadon.Rows[index].Cells[0].Value.ToString().Trim();
                    guna2HtmlLabel_mahd_thanhtoan.Text = guna2DataGridView_hoadon.Rows[index].Cells[1].Value.ToString().Trim();
                    DateTime ns = DateTime.Parse(guna2DataGridView_hoadon.Rows[index].Cells[3].Value.ToString());
                    guna2HtmlLabel_ngaylapHD.Text = ns.ToString("dd-MM-yyyy");
                    sl = guna2DataGridView_hoadon.Rows[index].Cells[4].Value.ToString();
                    guna2HtmlLabel_trangthai_thanhtoan.Text = guna2DataGridView_hoadon.Rows[index].Cells[6].Value.ToString().Trim();
                    if (guna2HtmlLabel_trangthai_thanhtoan.Text == "Đã Thanh Toán" || guna2HtmlLabel_trangthai_thanhtoan.Text == "Đã Bị Huỷ")
                    {
                        guna2Button_huy.Hide();
                        guna2Button_thantoan.Hide();
                    }
                    else
                    {
                        guna2Button_huy.Show();
                        guna2Button_thantoan.Show();
                    }
                    StaticThanhToan.maHD = guna2DataGridView_hoadon.Rows[index].Cells[1].Value.ToString().Trim();
                    StaticThanhToan.tongTien = guna2DataGridView_hoadon.Rows[index].Cells[5].Value.ToString().Trim();
                }
                catch (Exception)
                {

                }
            }
        }

        void search_HoaDon()
        {
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string makh = null;
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                makh = row[0].ToString();

            }
            string sql1 = string.Format("EXEC TIMKIEM_HOADON_FOR_KH N'{0}', '{1}'", guna2TextBox_searchforHD.Text, makh);
            adapter = new SqlDataAdapter(sql1, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView_hoadon.DataSource = dt;
        }

        private void guna2Button_thantoan_Click(object sender, EventArgs e)
        {
            if (guna2HtmlLabel_mahd_thanhtoan.Text == "." || guna2HtmlLabel_trangthai_thanhtoan.Text == ".")
            {
                MessageBox.Show("Hãy chọn hoá đơn cần thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (guna2HtmlLabel_trangthai_thanhtoan.Text == "Đã Thanh Toán")
            {
                MessageBox.Show("Đơn hàng này đã được thanh toán vui lòng chọn đơn hàng khác!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (guna2HtmlLabel_trangthai_thanhtoan.Text == "Đã Bị Huỷ")
            {
                MessageBox.Show("Đơn hàng này đã bị huỷ. Không thể thanh toán!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ThanhToanHoaDon tt = new ThanhToanHoaDon(this);
                tt.Show();
            }
        }

        string sl;
        private void guna2Button_huy_Click(object sender, EventArgs e)
        {
            if (guna2HtmlLabel_mahd_thanhtoan.Text == "." || guna2HtmlLabel_trangthai_thanhtoan.Text == ".")
            {
                MessageBox.Show("Hãy chọn hoá đơn cần huỷ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (guna2HtmlLabel_trangthai_thanhtoan.Text == "Đã Thanh Toán")
            {
                MessageBox.Show("Bạn chỉ có thể huỷ những đơn hàng chưa thanh toán!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (guna2HtmlLabel_trangthai_thanhtoan.Text == "Đã Bị Huỷ")
            {
                MessageBox.Show("Đơn hàng này đã bị huỷ. Không thể huỷ tiếp!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                cn.Open();
                string sql = string.Format("EXEC HUY_HOADON '{0}', '{1}', '{2}', '{3}'", guna2HtmlLabel_mahd_thanhtoan.Text, maDat, a, sl);
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                totalPages = 0;
                currentPage = 1; // Trang hiện tại
                pageSize = 6;    // 6 tour/trang
                load_Tour(1);
                // Không cho phép kéo giãn kích thước Form
                this.FormBorderStyle = FormBorderStyle.FixedSingle;

                string sql1 = "SELECT COUNT(*) FROM tour";
                SqlCommand cmd1 = new SqlCommand(sql1, cn);
                int totalItems = (int)cmd1.ExecuteScalar();
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                cn.Close();
                MessageBox.Show("Đã huỷ đơn hàng!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_HoaDon();
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            load_HoaDon();
        }

        public void loadDanhGia()
        {
            guna2DataGridView_danhgia.RowTemplate.Height = 80;
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string makh = null;
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                makh = row[0].ToString();

            }
            string sql1 = string.Format("EXEC SHOW_BANGDANHGIA '{0}'", makh);
            adapter = new SqlDataAdapter(sql1, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView_danhgia.DataSource = dt;
            // Kiểm tra xem DataGridView có dữ liệu hay không
            if (guna2DataGridView_danhgia.Rows.Count > 0)
            {
                // Chọn dòng đầu tiên
                guna2DataGridView_danhgia.ClearSelection();
                guna2DataGridView_danhgia.Rows[0].Selected = true;
                guna2DataGridView_danhgia.CurrentCell = guna2DataGridView_danhgia.Rows[0].Cells[0];

                // Gọi sự kiện CellClick cho dòng đầu tiên
                guna2DataGridView_danhgia_CellClick(
                    guna2DataGridView_danhgia,
                    new DataGridViewCellEventArgs(0, 0) // Cột 0, dòng 0
                );
                guna2Button_danhgia.Show();
            }
            else guna2Button_danhgia.Hide();
        }

        private void guna2Button_danhgia_Click(object sender, EventArgs e)
        {
            if (guna2HtmlLabel_madg.Text == "." || guna2HtmlLabel_diemdg.Text == ".")
            {
                MessageBox.Show("Hãy chọn tour cần đánh giá!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DanhGia_For_KH dg = new DanhGia_For_KH(guna2HtmlLabel_madg.Text, this);
            dg.Show();
        }

        private void guna2DataGridView_danhgia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int index = e.RowIndex;
                    guna2HtmlLabel_madg.Text = guna2DataGridView_danhgia.Rows[index].Cells[0].Value.ToString();
                    guna2HtmlLabel_tentour_dg.Text = guna2DataGridView_danhgia.Rows[index].Cells[2].Value.ToString();
                    guna2HtmlLabel_diemdg.Text = guna2DataGridView_danhgia.Rows[index].Cells[4].Value.ToString();
                    guna2HtmlLabel_tt_fordg.Text = guna2DataGridView_danhgia.Rows[index].Cells[5].Value.ToString();

                }
                catch (Exception)
                {

                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            loadDanhGia();
        }

        private void GiaoDienChoKhachHang_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Nếu chọn "No", hủy sự kiện đóng Form
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy sự kiện đóng Form
            }
            else
            {
                DangNhap f = new DangNhap();
                f.Show();
            }

        }

        private void GiaoDienChoKhachHang_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void guna2TextBox_mail_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button_searchForHD_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button_searchForHD_Click_1(object sender, EventArgs e)
        {
            search_HoaDon();
        }

        private void guna2HtmlLabel18_Click(object sender, EventArgs e)
        {

        }



        private void guna2Button_search_dg_Click(object sender, EventArgs e)
        {
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string makh = null;
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                makh = row[0].ToString();

            }
            string sql1 = string.Format("EXEC TIMKIEM_BANGDANHGIA_FOR_KH N'{0}', '{1}'", guna2TextBox_search_dg.Text, makh);
            adapter = new SqlDataAdapter(sql1, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2DataGridView_danhgia.DataSource = dt;
        }

        private void guna2HtmlLabel21_Click(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
