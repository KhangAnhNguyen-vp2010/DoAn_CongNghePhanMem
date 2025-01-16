using DanhGia_DiemDen;
using DAT_TOUR;
using Guna.UI2.WinForms;
using HOADON;
using KHACHHANG__PC_TOUR;
using QuanLyTourDuLich;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace TourDuLich
{
    public partial class GiaoDien_For_Admin : Form
    {
        public GiaoDien_For_Admin()
        {
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
                }
            }
        }

        private void guna2Button_tourVaLT_Click(object sender, EventArgs e)
        {
            QuanLy_Tour_Va_LichTrinh f = new QuanLy_Tour_Va_LichTrinh();
            f.Show();
        }

        private void guna2Button_KH_Click(object sender, EventArgs e)
        {
            KHACHHANG f = new KHACHHANG();
            f.Show();
        }

        private void guna2Button_HDV_Click(object sender, EventArgs e)
        {
            HDV f = new HDV();
            f.Show();
        }

        private void guna2Button_diemdien_Click(object sender, EventArgs e)
        {
            DiemDen f = new DiemDen();
            f.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            TKDanhGia f = new TKDanhGia();
            f.Show();
        }

        private void GiaoDien_For_Admin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void GiaoDien_For_Admin_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            QLHOADON f = new QLHOADON();
            f.Show();
        }

        private void guna2Button_dattour_Click(object sender, EventArgs e)
        {
            QLDatTour f = new QLDatTour();
            f.Show();
        }

        private void guna2Button_qlpchdv_Click(object sender, EventArgs e)
        {
            PhanCongHDV f = new PhanCongHDV();
            f.Show();
        }

        private void guna2Button_qlTK_Click(object sender, EventArgs e)
        {
            QuanLyTaiKhoan f = new QuanLyTaiKhoan();
            f.Show();
        }

        private void guna2Button_thongke_Click(object sender, EventArgs e)
        {
            ThongKe_DoanhThu f = new ThongKe_DoanhThu();
            f.Show();
        }
    }
}
