namespace CinemaTicket
{
    partial class FormSeatDesigner
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlSeats;
        private Panel pnlLegend;
        private Label lblTitle;
        private Label lblSelected;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlSeats = new Panel();
            pnlLegend = new Panel();
            lblTitle = new Label();
            lblSelected = new Label();
            SuspendLayout();
            // 
            // pnlSeats
            // 
            pnlSeats.AutoScroll = true;
            pnlSeats.BackColor = Color.Gainsboro;
            pnlSeats.Dock = DockStyle.Fill;
            pnlSeats.Location = new Point(0, 40);
            pnlSeats.Name = "pnlSeats";
            pnlSeats.Size = new Size(901, 324);
            pnlSeats.TabIndex = 0;
            // 
            // pnlLegend
            // 
            pnlLegend.BackColor = Color.WhiteSmoke;
            pnlLegend.Dock = DockStyle.Bottom;
            pnlLegend.Location = new Point(0, 394);
            pnlLegend.Name = "pnlLegend";
            pnlLegend.Size = new Size(901, 60);
            pnlLegend.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(901, 40);
            lblTitle.TabIndex = 3;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSelected
            // 
            lblSelected.Dock = DockStyle.Bottom;
            lblSelected.Location = new Point(0, 364);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(901, 30);
            lblSelected.TabIndex = 1;
            lblSelected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormSeatDesigner
            // 
            ClientSize = new Size(901, 454);
            Controls.Add(pnlSeats);
            Controls.Add(lblSelected);
            Controls.Add(pnlLegend);
            Controls.Add(lblTitle);
            Name = "FormSeatDesigner";
            Text = "Seat Designer";
            Load += FormSeatDesigner_Load;
            ResumeLayout(false);
        }
    }
}
