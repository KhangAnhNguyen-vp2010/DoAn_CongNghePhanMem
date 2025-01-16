using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyTourDuLich;
namespace WindowsFormsApp1
{
    public partial class LichSuDanTour : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        public LichSuDanTour()
        {
            InitializeComponent();
            conn = kn.conn;
        }
        public void LoadLS(string manv)
        {
            string sql = "Select MA_TOUR, MA_LT, NGAY_BD, NGAY_KT from LICHTRINH where MA_HDV=@manv";
            adapter = new SqlDataAdapter(sql, conn);
            dt = new DataTable();
            adapter.SelectCommand.Parameters.AddWithValue("@manv", manv);
            adapter.Fill(dt);
            dgv_lichsu.DataSource = dt;
            dgv_lichsu.Columns["MA_TOUR"].HeaderText = "Mã Tour";
            dgv_lichsu.Columns["MA_LT"].HeaderText = "Mã Lịch trình";
            dgv_lichsu.Columns["NGAY_BD"].HeaderText = "Ngày bắt đầu";
            dgv_lichsu.Columns["NGAY_KT"].HeaderText = "Ngày kết thúc";

        }

        private void LichSuDanTour_Load(object sender, EventArgs e)
        {

        }
    }
}
