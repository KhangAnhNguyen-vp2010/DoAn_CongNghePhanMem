using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FastReport;
using FastReport.Export.Pdf;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
using TheArtOfDevHtmlRenderer.Adapters;
namespace TourDuLich
{
    public partial class ThongKe_DoanhThu : Form
    {
        SqlConnection cn;
        KetNoi kn = new KetNoi();
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;

        public ThongKe_DoanhThu()
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
                    
                    guna2HtmlLabel2.Width += 1;

                }
            }
        }

        void loadChart()
        {
            cn.Open();
            string sql = "select month(ngay_lap) as thang, sum(tongtien) as total from hoadon where year(ngay_lap) = '" + guna2ComboBox1.SelectedItem + "' and trangthai = N'Đã Thanh Toán' group by month(ngay_lap) order by thang";
            cmd = new SqlCommand(sql, cn);
            SqlDataReader reader = cmd.ExecuteReader();

            chart1.Series.Clear();
            Series series = new Series("Doanh Thu");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            chart1.Series.Add(series);

            while (reader.Read())
            {
                int month = Convert.ToInt32(reader["thang"]);
                int reveune = Convert.ToInt32(reader["total"]);
                series.Points.AddXY(month, reveune);
            }

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Minimum = 1;
            chart1.ChartAreas[0].AxisX.Maximum = 12.5;
            chart1.ChartAreas[0].AxisX.Title = "Tháng";
            chart1.ChartAreas[0].AxisY.Title = "Doanh Thu (VND)";
            guna2HtmlLabel1.Text = "THỐNG KÊ DOANH THU NĂM " + guna2ComboBox1.SelectedItem;
            cn.Close();
        }

        private void ThongKe_DoanhThu_Load(object sender, EventArgs e)
        {

            loadChart();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadChart();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button_inBC_Click(object sender, EventArgs e)
        {
            cn.Open();
            string sql = "EXEC BAOCAO_DOANHTHU '"+guna2ComboBox1.SelectedItem+"'";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            cn.Close();
            // Tạo đối tượng Report
            Report report = new Report();

            // Tải báo cáo từ file .frx
            report.Load("BaoCaoDoanhThu.frx");

            // Gán dữ liệu cho báo cáo (nếu cần)
            report.RegisterData(dt, "BAOCAO_DOANHTHU");

            DateTime now = DateTime.Today;
            report.SetParameterValue("Parameter_nam", "DOANH THU TRONG NĂM " + guna2ComboBox1.SelectedItem);
            report.SetParameterValue("Parameter_date", "Ngày " + now.ToString("dd-MM-yyyy"));
            report.SetParameterValue("Parameter_sl", "Số Lượng: " + dt.Rows.Count.ToString());
            // Hiển thị báo cáo trong ReportViewer
            report.Prepare();
            report.Show();
        }
    }
}
