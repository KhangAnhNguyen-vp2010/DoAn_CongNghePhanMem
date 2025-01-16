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
    public partial class DatTour_ForKH : Form
    {
        SqlConnection cn;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        SqlCommand cmd;
        string maTour, tenTour, gia, sl_ton;
        GiaoDienChoKhachHang parent;
        public DatTour_ForKH(string a, string b, string c, string d, GiaoDienChoKhachHang B)
        {
            InitializeComponent();
            cn = kn.conn;
            maTour = a;
            tenTour = b;
            gia = c;
            sl_ton = d;
            parent = B;
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
                    guna2HtmlLabel7.Width += 1;
                    guna2HtmlLabel8.Width += 1;
                    guna2HtmlLabel9.Width += 1;
                    guna2HtmlLabel10.Width += 1;
                    guna2HtmlLabel11.Width += 1;
                    guna2HtmlLabel12.Width += 1;
                }
            }
        }

        string maDAT()
        {
            string sql = "select top 1 ma_dat from dat_tour order by ma_dat desc";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string old = "D001";
            string newStr;
            if (dt.Rows.Count > 0)
            {
                old = dt.Rows[0][0].ToString();
                int stt = int.Parse(old.Substring(2));
                stt++;
                newStr = "D" + stt.ToString("D3");
                return newStr;
            }
            return old;
        }

        string maHD()
        {
            string sql = "select top 1 ma_hd from hoadon order by ma_hd desc";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string old = "HD001";
            string newStr;
            if (dt.Rows.Count > 0)
            {
                old = dt.Rows[0][0].ToString();
                int stt = int.Parse(old.Substring(2));
                stt++;
                newStr = "HD" + stt.ToString("D3");
                return newStr;
            }
            return old;
        }

        string maDG()
        {
            string sql = "select top 1 ma_dg from danhgia order by ma_dg desc";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            string old = "DG001";
            string newStr;
            if (dt.Rows.Count > 0)
            {
                old = dt.Rows[0][0].ToString();
                int stt = int.Parse(old.Substring(2));
                stt++;
                newStr = "DG" + stt.ToString("D3");
                return newStr;
            }
            return old;
        }
        private void DatTour_ForKH_Load(object sender, EventArgs e)
        {
            guna2Button_guiDatTour.Enabled = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            numericUpDown1.ReadOnly = true;
            string sql = string.Format("EXEC LAY_THONGTIN_USER N'{0}', N'{1}'", StaticUser.tenTK, StaticUser.vaiTro);
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0) // Kiểm tra có dữ liệu trong bảng
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên (index = 0)

                // Lấy dữ liệu từng cột theo tên cột
                guna2HtmlLabel_makh_dattour.Text = row[0].ToString();
                guna2HtmlLabel_tenkh.Text = row[1].ToString();
                guna2HtmlLabel_mail.Text = row[2].ToString();
                guna2HtmlLabel_sdt.Text = row[3].ToString();
                guna2HtmlLabel_dc.Text = row[4].ToString();
            }

            guna2HtmlLabel_matour.Text = maTour;
            guna2HtmlLabel_tentour.Text = tenTour;
            guna2HtmlLabel_sl.Text = sl_ton;
            Decimal de = Decimal.Parse(gia);
            guna2HtmlLabel_gia.Text = ((int)de).ToString() + " VNĐ";


        }

        private void guna2Button_guiDatTour_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > int.Parse(sl_ton))
            {
                MessageBox.Show("Số Lượng Vé Đặt Không Được Vượt Quá Số Lượng Tồn Của Tour!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string maDat, mahd, madg;
                    maDat = maDAT();
                    mahd = maHD();
                    madg = maDG();
                    cn.Open();
                    DateTime now = DateTime.Now;
                    string sql = string.Format("EXEC DATTOUR_FOR_KH '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'", maDat, DatTourStaTic.maTour, guna2HtmlLabel_makh_dattour.Text, numericUpDown1.Value, now.ToString("yyyy-MM-dd"), decimal.Parse(gia), mahd, DatTourStaTic.maLT);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    string sql2 = string.Format("insert into danhgia values('{0}', '{1}', '{2}', '{3}', 1, '{4}', '{5}')", madg, DatTourStaTic.maTour, now.ToString("yyyy-MM-dd"), guna2HtmlLabel_makh_dattour.Text, DatTourStaTic.maLT, mahd);
                    cmd = new SqlCommand(sql2, cn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đặt Tour Thành Công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    parent.hihi();
                    this.Close();
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                guna2Button_guiDatTour.Enabled = true;
            }
            else guna2Button_guiDatTour.Enabled = false;
        }

        private void DatTour_ForKH_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
