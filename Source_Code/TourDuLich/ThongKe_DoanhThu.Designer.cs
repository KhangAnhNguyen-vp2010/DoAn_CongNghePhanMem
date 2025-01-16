namespace TourDuLich
{
    partial class ThongKe_DoanhThu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2ComboBox1 = new Guna.UI2.WinForms.Guna2ComboBox();
            guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2Button_inBC = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(20, 109);
            chart1.Margin = new Padding(4, 4, 4, 4);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(1774, 800);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 163);
            guna2HtmlLabel1.ForeColor = Color.Red;
            guna2HtmlLabel1.Location = new Point(590, 31);
            guna2HtmlLabel1.Margin = new Padding(4, 4, 4, 4);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(512, 50);
            guna2HtmlLabel1.TabIndex = 1;
            guna2HtmlLabel1.Text = "THỐNG KÊ DOANH THU NĂM";
            // 
            // guna2ComboBox1
            // 
            guna2ComboBox1.BackColor = Color.Transparent;
            guna2ComboBox1.CustomizableEdges = customizableEdges1;
            guna2ComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            guna2ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            guna2ComboBox1.FocusedColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2ComboBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            guna2ComboBox1.ForeColor = Color.FromArgb(68, 88, 112);
            guna2ComboBox1.ItemHeight = 30;
            guna2ComboBox1.Items.AddRange(new object[] { "2020", "2021", "2022", "2023", "2024", "2025", "2026" });
            guna2ComboBox1.Location = new Point(1562, 295);
            guna2ComboBox1.Margin = new Padding(4, 4, 4, 4);
            guna2ComboBox1.Name = "guna2ComboBox1";
            guna2ComboBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2ComboBox1.Size = new Size(210, 36);
            guna2ComboBox1.StartIndex = 0;
            guna2ComboBox1.TabIndex = 2;
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
            // 
            // guna2HtmlLabel2
            // 
            guna2HtmlLabel2.BackColor = Color.Transparent;
            guna2HtmlLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            guna2HtmlLabel2.Location = new Point(1581, 238);
            guna2HtmlLabel2.Margin = new Padding(4, 4, 4, 4);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(171, 34);
            guna2HtmlLabel2.TabIndex = 3;
            guna2HtmlLabel2.Text = "Lựa Chọn Năm";
            // 
            // guna2Button_inBC
            // 
            guna2Button_inBC.BorderRadius = 10;
            guna2Button_inBC.Cursor = Cursors.Hand;
            guna2Button_inBC.CustomizableEdges = customizableEdges3;
            guna2Button_inBC.DisabledState.BorderColor = Color.DarkGray;
            guna2Button_inBC.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button_inBC.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button_inBC.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button_inBC.FillColor = Color.Gray;
            guna2Button_inBC.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button_inBC.ForeColor = Color.White;
            guna2Button_inBC.Location = new Point(1590, 763);
            guna2Button_inBC.Name = "guna2Button_inBC";
            guna2Button_inBC.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button_inBC.Size = new Size(182, 69);
            guna2Button_inBC.TabIndex = 5;
            guna2Button_inBC.Text = "In Báo Cáo";
            guna2Button_inBC.Click += guna2Button_inBC_Click;
            // 
            // ThongKe_DoanhThu
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1809, 916);
            Controls.Add(guna2Button_inBC);
            Controls.Add(guna2HtmlLabel2);
            Controls.Add(guna2ComboBox1);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(chart1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "ThongKe_DoanhThu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ThongKe_DoanhThu";
            Load += ThongKe_DoanhThu_Load;
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2ComboBox guna2ComboBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Button guna2Button_inBC;
    }
}