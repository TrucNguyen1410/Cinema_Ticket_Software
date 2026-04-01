namespace CinemaTicket
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            btnLogout = new Button();
            btnReload = new Button();
            lblStaff = new Label();
            lblDate = new Label();
            pnlRight = new Panel();
            lstOrder = new ListBox();
            lblTotal = new Label();
            pnlCenter = new Panel();
            flpSeats = new FlowLayoutPanel();
            lblSelectedShow = new Label();
            flpShowtimes = new FlowLayoutPanel();
            pnlTop.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlCenter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.WhiteSmoke;
            pnlTop.Controls.Add(btnLogout);
            pnlTop.Controls.Add(btnReload);
            pnlTop.Controls.Add(lblStaff);
            pnlTop.Controls.Add(lblDate);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(3, 4, 3, 4);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(11, 13, 11, 13);
            pnlTop.Size = new Size(1588, 67);
            pnlTop.TabIndex = 3;
            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Right;
            btnLogout.Location = new Point(1371, 13);
            btnLogout.Margin = new Padding(3, 4, 3, 4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(103, 41);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "Đăng xuất";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnReload
            // 
            btnReload.Dock = DockStyle.Right;
            btnReload.Location = new Point(1474, 13);
            btnReload.Margin = new Padding(3, 4, 3, 4);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(103, 41);
            btnReload.TabIndex = 1;
            btnReload.Text = "🔄 Tải lại";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // lblStaff
            // 
            lblStaff.AutoSize = true;
            lblStaff.Font = new Font("Segoe UI", 10F);
            lblStaff.Location = new Point(240, 33);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new Size(94, 23);
            lblStaff.TabIndex = 2;
            lblStaff.Text = "Staff Name";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDate.Location = new Point(25, 33);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(116, 23);
            lblDate.TabIndex = 3;
            lblDate.Text = "dd/MM/yyyy";
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(lstOrder);
            pnlRight.Controls.Add(lblTotal);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Location = new Point(1245, 67);
            pnlRight.Margin = new Padding(3, 4, 3, 4);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(6, 7, 6, 7);
            pnlRight.Size = new Size(343, 733);
            pnlRight.TabIndex = 1;
            // 
            // lstOrder
            // 
            lstOrder.Dock = DockStyle.Fill;
            lstOrder.Font = new Font("Segoe UI", 11F);
            lstOrder.FormattingEnabled = true;
            lstOrder.ItemHeight = 25;
            lstOrder.Location = new Point(6, 7);
            lstOrder.Margin = new Padding(3, 4, 3, 4);
            lstOrder.Name = "lstOrder";
            lstOrder.Size = new Size(331, 652);
            lstOrder.TabIndex = 0;
            // 
            // lblTotal
            // 
            lblTotal.BackColor = Color.MistyRose;
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotal.ForeColor = Color.Red;
            lblTotal.Location = new Point(6, 659);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(331, 67);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "0 đ";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlCenter
            // 
            pnlCenter.Controls.Add(flpSeats);
            pnlCenter.Controls.Add(lblSelectedShow);
            pnlCenter.Dock = DockStyle.Fill;
            pnlCenter.Location = new Point(365, 67);
            pnlCenter.Margin = new Padding(3, 4, 3, 4);
            pnlCenter.Name = "pnlCenter";
            pnlCenter.Padding = new Padding(11, 13, 11, 13);
            pnlCenter.Size = new Size(880, 733);
            pnlCenter.TabIndex = 0;
            // 
            // flpSeats
            // 
            flpSeats.AutoScroll = true;
            flpSeats.BackColor = Color.LightGray;
            flpSeats.BorderStyle = BorderStyle.Fixed3D;
            flpSeats.Dock = DockStyle.Fill;
            flpSeats.Location = new Point(11, 53);
            flpSeats.Margin = new Padding(3, 4, 3, 4);
            flpSeats.Name = "flpSeats";
            flpSeats.Padding = new Padding(23, 27, 23, 27);
            flpSeats.Size = new Size(858, 667);
            flpSeats.TabIndex = 0;
            // 
            // lblSelectedShow
            // 
            lblSelectedShow.Dock = DockStyle.Top;
            lblSelectedShow.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSelectedShow.ForeColor = Color.Navy;
            lblSelectedShow.Location = new Point(11, 13);
            lblSelectedShow.Name = "lblSelectedShow";
            lblSelectedShow.Size = new Size(858, 40);
            lblSelectedShow.TabIndex = 1;
            lblSelectedShow.Text = "Vui lòng chọn suất chiếu";
            lblSelectedShow.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpShowtimes
            // 
            flpShowtimes.AutoScroll = true;
            flpShowtimes.BackColor = Color.White;
            flpShowtimes.BorderStyle = BorderStyle.FixedSingle;
            flpShowtimes.Dock = DockStyle.Left;
            flpShowtimes.Location = new Point(0, 67);
            flpShowtimes.Margin = new Padding(3, 4, 3, 4);
            flpShowtimes.Name = "flpShowtimes";
            flpShowtimes.Size = new Size(365, 733);
            flpShowtimes.TabIndex = 2;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1588, 800);
            Controls.Add(pnlCenter);
            Controls.Add(pnlRight);
            Controls.Add(flpShowtimes);
            Controls.Add(pnlTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Màn hình Bán vé";
            WindowState = FormWindowState.Maximized;
            Load += Form2_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlRight.ResumeLayout(false);
            pnlCenter.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblStaff;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnReload; // <--- Biến mới
        private System.Windows.Forms.FlowLayoutPanel flpShowtimes;
        private System.Windows.Forms.FlowLayoutPanel flpSeats;
        private System.Windows.Forms.ListBox lstOrder;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblSelectedShow;
        private Panel pnlTop;
        private Panel pnlRight;
        private Panel pnlCenter;
    }
}