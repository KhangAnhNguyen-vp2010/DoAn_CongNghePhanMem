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
using TourDuLich;
namespace WindowsFormsApp1
{
    public partial class DangKy : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        KetNoi kn = new KetNoi();
        DataTable dt;
        public DangKy()
        {
            InitializeComponent();
            conn = kn.conn;
        }
     
        
        private void DangKy_Load(object sender, EventArgs e)
        {
           
        }

        
        private void btn_dangky_Click(object sender, EventArgs e)
        {

            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrEmpty(txt_tk.Text) || string.IsNullOrEmpty(txt_mk.Text) || string.IsNullOrEmpty(txt_retype.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng ký.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_mk.Text != txt_retype.Text)
            {
                MessageBox.Show("Nhập lại mật khẩu không trùng khớp.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = string.Format("select * from tai_khoan where tentaikhoan = N'{0}'", txt_tk.Text);
            adapter = new SqlDataAdapter(sql,conn);
            dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Tài khoản đã tồn tại!!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NhapThongTinKH_KhiDangKy f = new NhapThongTinKH_KhiDangKy(txt_tk.Text, txt_mk.Text);
            f.Show();
            this.Close();
            
        }

        

        

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            DangNhap dn=new DangNhap();
            dn.Show();
            this.Hide();
        }
    }
}
