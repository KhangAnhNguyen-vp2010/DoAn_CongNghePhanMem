using DanhGia_DiemDen;
using DAT_TOUR;
using HOADON;
using KHACHHANG__PC_TOUR;
using QuanLyTourDuLich;
using WindowsFormsApp1;

namespace TourDuLich
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new DangNhap());
        }
    }
}