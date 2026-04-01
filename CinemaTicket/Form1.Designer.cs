namespace CinemaTicket
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabMovies = new TabPage();
            dgvMovies = new DataGridView();
            panelMovieTop = new Panel();
            label1 = new Label();
            txtId = new TextBox();
            label2 = new Label();
            txtTitle = new TextBox();
            label3 = new Label();
            txtDuration = new TextBox();
            label4 = new Label();
            txtDescription = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            tabShows = new TabPage();
            schedulerControl1 = new DevExpress.XtraScheduler.SchedulerControl();
            dataStorage = new DevExpress.XtraScheduler.SchedulerDataStorage(components);
            panelShowTop = new Panel();
            button1 = new Button();
            label5 = new Label();
            cbShowMovie = new ComboBox();
            label6 = new Label();
            cbRoom = new ComboBox();
            label7 = new Label();
            dtShowDate = new DateTimePicker();
            label8 = new Label();
            dtShowTime = new DateTimePicker();
            label9 = new Label();
            txtShowPrice = new TextBox();
            btnAddShow = new Button();
            tabBookings = new TabPage();
            dgvBookings = new DataGridView();
            panelBookingTop = new Panel();
            lblSearch = new Label();
            txtSearchBooking = new TextBox();
            lblFRoom = new Label();
            cbFilterRoomBooking = new ComboBox();
            lblFMovie = new Label();
            cbFilterMovieBooking = new ComboBox();
            btnExchange = new Button();
            btnRefund = new Button();
            btnFilterBooking = new Button();
            dtBookingFilter = new DateTimePicker();
            label10 = new Label();
            tabStats = new TabPage();
            panelStatTop = new Panel();
            btnExport = new Button();
            btnStat = new Button();
            dtStatDate = new DateTimePicker();
            label12 = new Label();
            cbStatType = new ComboBox();
            label11 = new Label();
            cbFilterMovie = new ComboBox();
            lblFilterMovie = new Label();
            tabControl1.SuspendLayout();
            tabMovies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovies).BeginInit();
            panelMovieTop.SuspendLayout();
            tabShows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)schedulerControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataStorage).BeginInit();
            panelShowTop.SuspendLayout();
            tabBookings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBookings).BeginInit();
            panelBookingTop.SuspendLayout();
            tabStats.SuspendLayout();
            panelStatTop.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabMovies);
            tabControl1.Controls.Add(tabShows);
            tabControl1.Controls.Add(tabBookings);
            tabControl1.Controls.Add(tabStats);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1200, 700);
            tabControl1.TabIndex = 0;
            // 
            // tabMovies
            // 
            tabMovies.Controls.Add(dgvMovies);
            tabMovies.Controls.Add(panelMovieTop);
            tabMovies.Location = new Point(4, 29);
            tabMovies.Name = "tabMovies";
            tabMovies.Padding = new Padding(3);
            tabMovies.Size = new Size(1192, 667);
            tabMovies.TabIndex = 0;
            tabMovies.Text = "Quản lý phim";
            tabMovies.UseVisualStyleBackColor = true;
            // 
            // dgvMovies
            // 
            dgvMovies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMovies.Dock = DockStyle.Fill;
            dgvMovies.Location = new Point(3, 133);
            dgvMovies.Name = "dgvMovies";
            dgvMovies.RowHeadersWidth = 51;
            dgvMovies.Size = new Size(1186, 531);
            dgvMovies.TabIndex = 1;
            dgvMovies.CellClick += dgvMovies_CellClick;
            // 
            // panelMovieTop
            // 
            panelMovieTop.Controls.Add(label1);
            panelMovieTop.Controls.Add(txtId);
            panelMovieTop.Controls.Add(label2);
            panelMovieTop.Controls.Add(txtTitle);
            panelMovieTop.Controls.Add(label3);
            panelMovieTop.Controls.Add(txtDuration);
            panelMovieTop.Controls.Add(label4);
            panelMovieTop.Controls.Add(txtDescription);
            panelMovieTop.Controls.Add(btnAdd);
            panelMovieTop.Controls.Add(btnUpdate);
            panelMovieTop.Controls.Add(btnDelete);
            panelMovieTop.Dock = DockStyle.Top;
            panelMovieTop.Location = new Point(3, 3);
            panelMovieTop.Name = "panelMovieTop";
            panelMovieTop.Size = new Size(1186, 130);
            panelMovieTop.TabIndex = 0;
            // 
            // label1
            // 
            label1.Location = new Point(20, 15);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            label1.Text = "ID";
            // 
            // txtId
            // 
            txtId.Location = new Point(108, 12);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(100, 27);
            txtId.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(20, 45);
            label2.Name = "label2";
            label2.Size = new Size(74, 23);
            label2.TabIndex = 2;
            label2.Text = "Tên phim";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(108, 42);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(242, 27);
            txtTitle.TabIndex = 3;
            // 
            // label3
            // 
            label3.Location = new Point(20, 75);
            label3.Name = "label3";
            label3.Size = new Size(82, 23);
            label3.TabIndex = 4;
            label3.Text = "Thời lượng";
            // 
            // txtDuration
            // 
            txtDuration.Location = new Point(108, 72);
            txtDuration.Name = "txtDuration";
            txtDuration.Size = new Size(242, 27);
            txtDuration.TabIndex = 5;
            // 
            // label4
            // 
            label4.Location = new Point(380, 42);
            label4.Name = "label4";
            label4.Size = new Size(54, 23);
            label4.TabIndex = 6;
            label4.Text = "Mô tả";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(440, 38);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(300, 27);
            txtDescription.TabIndex = 7;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(780, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 29);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(780, 38);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 28);
            btnUpdate.TabIndex = 9;
            btnUpdate.Text = "Sửa";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(780, 72);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 27);
            btnDelete.TabIndex = 10;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // tabShows
            // 
            tabShows.Controls.Add(schedulerControl1);
            tabShows.Controls.Add(panelShowTop);
            tabShows.Location = new Point(4, 29);
            tabShows.Name = "tabShows";
            tabShows.Padding = new Padding(3);
            tabShows.Size = new Size(192, 67);
            tabShows.TabIndex = 1;
            tabShows.Text = "Suất chiếu";
            tabShows.UseVisualStyleBackColor = true;
            // 
            // schedulerControl1
            // 
            schedulerControl1.AllowDrop = false;
            schedulerControl1.DataStorage = dataStorage;
            schedulerControl1.Dock = DockStyle.Fill;
            schedulerControl1.Location = new Point(3, 107);
            schedulerControl1.Name = "schedulerControl1";
            schedulerControl1.Size = new Size(186, 0);
            schedulerControl1.Start = new DateTime(2026, 2, 1, 0, 0, 0, 0);
            schedulerControl1.TabIndex = 1;
            schedulerControl1.Text = "schedulerControl1";
            schedulerControl1.Views.YearView.UseOptimizedScrolling = false;
            // 
            // dataStorage
            // 
            // 
            // 
            // 
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(SystemColors.Window, "None", "&None"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(255, 194, 190), "Important", "&Important"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(168, 213, 255), "Business", "&Business"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(193, 244, 156), "Personal", "&Personal"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(243, 228, 199), "Vacation", "&Vacation"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(244, 206, 147), "Must Attend", "Must &Attend"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(199, 244, 255), "Travel Required", "&Travel Required"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(207, 219, 152), "Needs Preparation", "&Needs Preparation"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(224, 207, 233), "Birthday", "&Birthday"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(141, 233, 223), "Anniversary", "&Anniversary"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(255, 247, 165), "Phone Call", "Phone &Call"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(SystemColors.Window, "None", "&None"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(255, 194, 190), "Important", "&Important"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(168, 213, 255), "Business", "&Business"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(193, 244, 156), "Personal", "&Personal"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(243, 228, 199), "Vacation", "&Vacation"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(244, 206, 147), "Must Attend", "Must &Attend"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(199, 244, 255), "Travel Required", "&Travel Required"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(207, 219, 152), "Needs Preparation", "&Needs Preparation"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(224, 207, 233), "Birthday", "&Birthday"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(141, 233, 223), "Anniversary", "&Anniversary"));
            dataStorage.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(Color.FromArgb(255, 247, 165), "Phone Call", "Phone &Call"));
            // 
            // panelShowTop
            // 
            panelShowTop.Controls.Add(button1);
            panelShowTop.Controls.Add(label5);
            panelShowTop.Controls.Add(cbShowMovie);
            panelShowTop.Controls.Add(label6);
            panelShowTop.Controls.Add(cbRoom);
            panelShowTop.Controls.Add(label7);
            panelShowTop.Controls.Add(dtShowDate);
            panelShowTop.Controls.Add(label8);
            panelShowTop.Controls.Add(dtShowTime);
            panelShowTop.Controls.Add(label9);
            panelShowTop.Controls.Add(txtShowPrice);
            panelShowTop.Controls.Add(btnAddShow);
            panelShowTop.Dock = DockStyle.Top;
            panelShowTop.Location = new Point(3, 3);
            panelShowTop.Name = "panelShowTop";
            panelShowTop.Size = new Size(186, 104);
            panelShowTop.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(848, 6);
            button1.Name = "button1";
            button1.Size = new Size(98, 28);
            button1.TabIndex = 11;
            button1.Text = "Xóa Suất";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.Location = new Point(20, 15);
            label5.Name = "label5";
            label5.Size = new Size(100, 23);
            label5.TabIndex = 0;
            label5.Text = "Tên phim";
            // 
            // cbShowMovie
            // 
            cbShowMovie.DropDownStyle = ComboBoxStyle.DropDownList;
            cbShowMovie.Location = new Point(20, 40);
            cbShowMovie.Name = "cbShowMovie";
            cbShowMovie.Size = new Size(180, 28);
            cbShowMovie.TabIndex = 1;
            // 
            // label6
            // 
            label6.Location = new Point(220, 15);
            label6.Name = "label6";
            label6.Size = new Size(100, 23);
            label6.TabIndex = 2;
            label6.Text = "Rạp";
            // 
            // cbRoom
            // 
            cbRoom.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRoom.Location = new Point(220, 40);
            cbRoom.Name = "cbRoom";
            cbRoom.Size = new Size(120, 28);
            cbRoom.TabIndex = 3;
            // 
            // label7
            // 
            label7.Location = new Point(360, 15);
            label7.Name = "label7";
            label7.Size = new Size(100, 23);
            label7.TabIndex = 4;
            label7.Text = "Ngày chiếu";
            // 
            // dtShowDate
            // 
            dtShowDate.Format = DateTimePickerFormat.Short;
            dtShowDate.Location = new Point(360, 40);
            dtShowDate.Name = "dtShowDate";
            dtShowDate.Size = new Size(134, 27);
            dtShowDate.TabIndex = 5;
            // 
            // label8
            // 
            label8.Location = new Point(520, 15);
            label8.Name = "label8";
            label8.Size = new Size(100, 23);
            label8.TabIndex = 6;
            label8.Text = "Giờ chiếu";
            // 
            // dtShowTime
            // 
            dtShowTime.Format = DateTimePickerFormat.Time;
            dtShowTime.Location = new Point(520, 40);
            dtShowTime.Name = "dtShowTime";
            dtShowTime.ShowUpDown = true;
            dtShowTime.Size = new Size(122, 27);
            dtShowTime.TabIndex = 7;
            // 
            // label9
            // 
            label9.Location = new Point(680, 15);
            label9.Name = "label9";
            label9.Size = new Size(100, 23);
            label9.TabIndex = 8;
            label9.Text = "Giá vé";
            // 
            // txtShowPrice
            // 
            txtShowPrice.Location = new Point(680, 41);
            txtShowPrice.Name = "txtShowPrice";
            txtShowPrice.Size = new Size(100, 27);
            txtShowPrice.TabIndex = 9;
            // 
            // btnAddShow
            // 
            btnAddShow.Location = new Point(848, 40);
            btnAddShow.Name = "btnAddShow";
            btnAddShow.Size = new Size(98, 28);
            btnAddShow.TabIndex = 10;
            btnAddShow.Text = "Thêm";
            btnAddShow.UseVisualStyleBackColor = true;
            btnAddShow.Click += btnAddShow_Click;
            // 
            // tabBookings
            // 
            tabBookings.Controls.Add(dgvBookings);
            tabBookings.Controls.Add(panelBookingTop);
            tabBookings.Location = new Point(4, 29);
            tabBookings.Name = "tabBookings";
            tabBookings.Padding = new Padding(3);
            tabBookings.Size = new Size(1192, 667);
            tabBookings.TabIndex = 2;
            tabBookings.Text = "Bookings";
            tabBookings.UseVisualStyleBackColor = true;
            // 
            // dgvBookings
            // 
            dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBookings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBookings.Dock = DockStyle.Fill;
            dgvBookings.Location = new Point(3, 93);
            dgvBookings.Name = "dgvBookings";
            dgvBookings.RowHeadersWidth = 51;
            dgvBookings.Size = new Size(1186, 571);
            dgvBookings.TabIndex = 1;
            // 
            // panelBookingTop
            // 
            panelBookingTop.Controls.Add(lblSearch);
            panelBookingTop.Controls.Add(txtSearchBooking);
            panelBookingTop.Controls.Add(lblFRoom);
            panelBookingTop.Controls.Add(cbFilterRoomBooking);
            panelBookingTop.Controls.Add(lblFMovie);
            panelBookingTop.Controls.Add(cbFilterMovieBooking);
            panelBookingTop.Controls.Add(btnExchange);
            panelBookingTop.Controls.Add(btnRefund);
            panelBookingTop.Controls.Add(btnFilterBooking);
            panelBookingTop.Controls.Add(dtBookingFilter);
            panelBookingTop.Controls.Add(label10);
            panelBookingTop.Dock = DockStyle.Top;
            panelBookingTop.Location = new Point(3, 3);
            panelBookingTop.Name = "panelBookingTop";
            panelBookingTop.Size = new Size(1186, 90);
            panelBookingTop.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSearch.Location = new Point(20, 55);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(93, 20);
            lblSearch.TabIndex = 5;
            lblSearch.Text = "Tìm vé/ghế:";
            // 
            // txtSearchBooking
            // 
            txtSearchBooking.Location = new Point(120, 52);
            txtSearchBooking.Name = "txtSearchBooking";
            txtSearchBooking.Size = new Size(150, 27);
            txtSearchBooking.TabIndex = 6;
            // 
            // lblFRoom
            // 
            lblFRoom.AutoSize = true;
            lblFRoom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFRoom.Location = new Point(290, 55);
            lblFRoom.Name = "lblFRoom";
            lblFRoom.Size = new Size(68, 20);
            lblFRoom.TabIndex = 7;
            lblFRoom.Text = "Lọc Rạp:";
            // 
            // cbFilterRoomBooking
            // 
            cbFilterRoomBooking.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterRoomBooking.Location = new Point(360, 52);
            cbFilterRoomBooking.Name = "cbFilterRoomBooking";
            cbFilterRoomBooking.Size = new Size(120, 28);
            cbFilterRoomBooking.TabIndex = 8;
            // 
            // lblFMovie
            // 
            lblFMovie.AutoSize = true;
            lblFMovie.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFMovie.Location = new Point(500, 55);
            lblFMovie.Name = "lblFMovie";
            lblFMovie.Size = new Size(77, 20);
            lblFMovie.TabIndex = 9;
            lblFMovie.Text = "Lọc Phim:";
            // 
            // cbFilterMovieBooking
            // 
            cbFilterMovieBooking.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterMovieBooking.Location = new Point(580, 52);
            cbFilterMovieBooking.Name = "cbFilterMovieBooking";
            cbFilterMovieBooking.Size = new Size(150, 28);
            cbFilterMovieBooking.TabIndex = 10;
            // 
            // btnExchange
            // 
            btnExchange.BackColor = Color.LightSkyBlue;
            btnExchange.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExchange.Location = new Point(450, 15);
            btnExchange.Name = "btnExchange";
            btnExchange.Size = new Size(100, 30);
            btnExchange.TabIndex = 3;
            btnExchange.Text = "Đổi Vé";
            btnExchange.UseVisualStyleBackColor = false;
            btnExchange.Click += BtnExchange_Click;
            // 
            // btnRefund
            // 
            btnRefund.BackColor = Color.Orange;
            btnRefund.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefund.Location = new Point(560, 15);
            btnRefund.Name = "btnRefund";
            btnRefund.Size = new Size(100, 30);
            btnRefund.TabIndex = 4;
            btnRefund.Text = "Hoàn Vé";
            btnRefund.UseVisualStyleBackColor = false;
            btnRefund.Click += BtnRefund_Click;
            // 
            // btnFilterBooking
            // 
            btnFilterBooking.Location = new Point(330, 15);
            btnFilterBooking.Name = "btnFilterBooking";
            btnFilterBooking.Size = new Size(94, 29);
            btnFilterBooking.TabIndex = 2;
            btnFilterBooking.Text = "Xem";
            btnFilterBooking.UseVisualStyleBackColor = true;
            btnFilterBooking.Click += btnFilterBooking_Click;
            // 
            // dtBookingFilter
            // 
            dtBookingFilter.Format = DateTimePickerFormat.Short;
            dtBookingFilter.Location = new Point(170, 16);
            dtBookingFilter.Name = "dtBookingFilter";
            dtBookingFilter.Size = new Size(140, 27);
            dtBookingFilter.TabIndex = 1;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label10.Location = new Point(20, 18);
            label10.Name = "label10";
            label10.Size = new Size(147, 23);
            label10.TabIndex = 0;
            label10.Text = "Chọn ngày chiếu:";
            // 
            // tabStats
            // 
            tabStats.Controls.Add(panelStatTop);
            tabStats.Location = new Point(4, 29);
            tabStats.Name = "tabStats";
            tabStats.Padding = new Padding(3);
            tabStats.Size = new Size(192, 67);
            tabStats.TabIndex = 3;
            tabStats.Text = "Thống kê";
            tabStats.UseVisualStyleBackColor = true;
            // 
            // panelStatTop
            // 
            panelStatTop.Controls.Add(btnExport);
            panelStatTop.Controls.Add(btnStat);
            panelStatTop.Controls.Add(dtStatDate);
            panelStatTop.Controls.Add(label12);
            panelStatTop.Controls.Add(cbStatType);
            panelStatTop.Controls.Add(label11);
            panelStatTop.Controls.Add(cbFilterMovie);
            panelStatTop.Controls.Add(lblFilterMovie);
            panelStatTop.Dock = DockStyle.Top;
            panelStatTop.Location = new Point(3, 3);
            panelStatTop.Name = "panelStatTop";
            panelStatTop.Size = new Size(186, 70);
            panelStatTop.TabIndex = 0;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.LightGreen;
            btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExport.Location = new Point(1020, 20);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(150, 30);
            btnExport.TabIndex = 7;
            btnExport.Text = "Xuất Báo Cáo (CSV)";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += BtnExport_Click;
            // 
            // btnStat
            // 
            btnStat.Location = new Point(880, 20);
            btnStat.Name = "btnStat";
            btnStat.Size = new Size(120, 30);
            btnStat.TabIndex = 4;
            btnStat.Text = "Xem Thống kê";
            btnStat.UseVisualStyleBackColor = true;
            btnStat.Click += btnStat_Click;
            // 
            // dtStatDate
            // 
            dtStatDate.Format = DateTimePickerFormat.Short;
            dtStatDate.Location = new Point(740, 21);
            dtStatDate.Name = "dtStatDate";
            dtStatDate.Size = new Size(120, 27);
            dtStatDate.TabIndex = 3;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(660, 24);
            label12.Name = "label12";
            label12.Size = new Size(74, 20);
            label12.TabIndex = 2;
            label12.Text = "Thời gian:";
            // 
            // cbStatType
            // 
            cbStatType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatType.FormattingEnabled = true;
            cbStatType.Location = new Point(130, 21);
            cbStatType.Name = "cbStatType";
            cbStatType.Size = new Size(200, 28);
            cbStatType.TabIndex = 1;
            cbStatType.SelectedIndexChanged += cbStatType_SelectedIndexChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(20, 24);
            label11.Name = "label11";
            label11.Size = new Size(102, 20);
            label11.TabIndex = 0;
            label11.Text = "Loại thống kê:";
            // 
            // cbFilterMovie
            // 
            cbFilterMovie.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterMovie.FormattingEnabled = true;
            cbFilterMovie.Location = new Point(440, 21);
            cbFilterMovie.Name = "cbFilterMovie";
            cbFilterMovie.Size = new Size(200, 28);
            cbFilterMovie.TabIndex = 5;
            cbFilterMovie.Visible = false;
            // 
            // lblFilterMovie
            // 
            lblFilterMovie.AutoSize = true;
            lblFilterMovie.Location = new Point(350, 24);
            lblFilterMovie.Name = "lblFilterMovie";
            lblFilterMovie.Size = new Size(84, 20);
            lblFilterMovie.TabIndex = 6;
            lblFilterMovie.Text = "Chọn phim:";
            lblFilterMovie.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Màn hình QUẢN LÝ (Admin)";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabMovies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMovies).EndInit();
            panelMovieTop.ResumeLayout(false);
            panelMovieTop.PerformLayout();
            tabShows.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)schedulerControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataStorage).EndInit();
            panelShowTop.ResumeLayout(false);
            panelShowTop.PerformLayout();
            tabBookings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBookings).EndInit();
            panelBookingTop.ResumeLayout(false);
            panelBookingTop.PerformLayout();
            tabStats.ResumeLayout(false);
            panelStatTop.ResumeLayout(false);
            panelStatTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMovies;
        private System.Windows.Forms.TabPage tabShows;
        private System.Windows.Forms.Panel panelMovieTop;
        private System.Windows.Forms.DataGridView dgvMovies;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelShowTop;
        private System.Windows.Forms.ComboBox cbShowMovie;
        private System.Windows.Forms.ComboBox cbRoom;
        private System.Windows.Forms.DateTimePicker dtShowDate;
        private System.Windows.Forms.DateTimePicker dtShowTime;
        private System.Windows.Forms.TextBox txtShowPrice;
        private System.Windows.Forms.Button btnAddShow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraScheduler.SchedulerControl schedulerControl1;
        private DevExpress.XtraScheduler.SchedulerDataStorage dataStorage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabBookings;
        private System.Windows.Forms.Panel panelBookingTop;
        private System.Windows.Forms.DataGridView dgvBookings;
        private System.Windows.Forms.Button btnFilterBooking;
        private System.Windows.Forms.DateTimePicker dtBookingFilter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabStats;
        private System.Windows.Forms.Panel panelStatTop;
        private System.Windows.Forms.Button btnStat;
        private System.Windows.Forms.DateTimePicker dtStatDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbStatType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbFilterMovie;
        private System.Windows.Forms.Label lblFilterMovie;

        // --- KHAI BÁO BIẾN CHO CÁC CONTROL MỚI ---
        private System.Windows.Forms.TextBox txtSearchBooking;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cbFilterRoomBooking;
        private System.Windows.Forms.Label lblFRoom;
        private System.Windows.Forms.ComboBox cbFilterMovieBooking;
        private System.Windows.Forms.Label lblFMovie;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.Button btnExport;
    }
}