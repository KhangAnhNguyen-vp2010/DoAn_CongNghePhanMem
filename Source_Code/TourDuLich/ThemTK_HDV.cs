using Microsoft.Data.SqlClient;
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
using TheArtOfDevHtmlRenderer.Adapters;
using Microsoft.Data.SqlClient;

namespace TourDuLich
{
    public partial class ThemTK_HDV : Form
    {
        SqlConnection cn;
        SqlDataAdapter adapter;
        DataTable dt;
        KetNoi kn = new KetNoi();
        SqlCommand cmd;
        QuanLyTaiKhoan f;
        public ThemTK_HDV(QuanLyTaiKhoan a)
        {
            InitializeComponent();
            cn = kn.conn;
            f = a;
        }

        void tao_TK()
        {
            if (string.IsNullOrEmpty(guna2TextBox_tentk.Text) || string.IsNullOrEmpty(guna2TextBox_pass.Text))
            {
                MessageBox.Show("Nhập đầy đủ dữ tên tài khoản - mật khẩu!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cn.Open();
                    string sql = string.Format("insert into tai_khoan values(N'{0}', N'{1}', 'HDV', 'HOAT DONG', null, '{2}')", guna2TextBox_tentk.Text, guna2TextBox_pass.Text, guna2ComboBox_hdv.SelectedValue);
                    cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Tạo tài khoản thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    f.load_data();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { cn.Close(); }
            }
            

        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tao_TK();
        }

        void loadCBB()
        {
            string sql = "SELECT ma_hdv from hdv";
            adapter = new SqlDataAdapter(sql, cn);
            dt = new DataTable();
            adapter.Fill(dt);
            guna2ComboBox_hdv.DataSource = dt;
            guna2ComboBox_hdv.DisplayMember = "ten_hdv";
            guna2ComboBox_hdv.ValueMember = "ma_hdv";
            guna2ComboBox_hdv.SelectedIndex = 0;
        }
        private void ThemTK_HDV_Load(object sender, EventArgs e)
        {
            loadCBB();
        }
    }
}
