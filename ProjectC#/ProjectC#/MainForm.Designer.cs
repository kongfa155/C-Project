namespace ProjectC_
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelCalendar = new System.Windows.Forms.Panel();
            this.btnPreviousMonth = new System.Windows.Forms.Button();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblSpend = new System.Windows.Forms.Label();
            this.lblRevenue = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnToggleView = new System.Windows.Forms.Button();
            this.lblDayChoosen = new System.Windows.Forms.Label();
            this.lstBoxDetails = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnAddSpend = new System.Windows.Forms.Button();
            this.btnAddSchedule = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1898, 844);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1322, 838);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelCalendar);
            this.panel1.Controls.Add(this.btnPreviousMonth);
            this.panel1.Controls.Add(this.btnNextMonth);
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1316, 664);
            this.panel1.TabIndex = 0;
            // 
            // panelCalendar
            // 
            this.panelCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCalendar.Location = new System.Drawing.Point(11, 71);
            this.panelCalendar.Name = "panelCalendar";
            this.panelCalendar.Size = new System.Drawing.Size(1302, 590);
            this.panelCalendar.TabIndex = 3;
            // 
            // btnPreviousMonth
            // 
            this.btnPreviousMonth.Location = new System.Drawing.Point(309, 8);
            this.btnPreviousMonth.Name = "btnPreviousMonth";
            this.btnPreviousMonth.Size = new System.Drawing.Size(96, 51);
            this.btnPreviousMonth.TabIndex = 2;
            this.btnPreviousMonth.Text = "<";
            this.btnPreviousMonth.UseVisualStyleBackColor = true;
            this.btnPreviousMonth.Click += new System.EventHandler(this.btnPreviousMonth_Click);
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.Location = new System.Drawing.Point(928, 14);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(96, 51);
            this.btnNextMonth.TabIndex = 1;
            this.btnNextMonth.Text = ">";
            this.btnNextMonth.UseVisualStyleBackColor = true;
            this.btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(602, 14);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(112, 38);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "XX - XX";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cboCategoryFilter);
            this.panel2.Controls.Add(this.lblBalance);
            this.panel2.Controls.Add(this.lblSpend);
            this.panel2.Controls.Add(this.lblRevenue);
            this.panel2.Controls.Add(this.lblMonth);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 673);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1316, 162);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(535, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 38);
            this.label1.TabIndex = 5;
            this.label1.Text = "Hiển thị theo";
            // 
            // cboCategoryFilter
            // 
            this.cboCategoryFilter.FormattingEnabled = true;
            this.cboCategoryFilter.Location = new System.Drawing.Point(542, 52);
            this.cboCategoryFilter.Name = "cboCategoryFilter";
            this.cboCategoryFilter.Size = new System.Drawing.Size(212, 46);
            this.cboCategoryFilter.TabIndex = 4;
            this.cboCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.cboCategoryFilter_SelectedIndexChanged);
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(6, 91);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(88, 38);
            this.lblBalance.TabIndex = 3;
            this.lblBalance.Text = "Số dư";
            // 
            // lblSpend
            // 
            this.lblSpend.AutoSize = true;
            this.lblSpend.Location = new System.Drawing.Point(6, 52);
            this.lblSpend.Name = "lblSpend";
            this.lblSpend.Size = new System.Drawing.Size(124, 38);
            this.lblSpend.TabIndex = 2;
            this.lblSpend.Text = "Tổng chi";
            // 
            // lblRevenue
            // 
            this.lblRevenue.AutoSize = true;
            this.lblRevenue.Location = new System.Drawing.Point(6, 10);
            this.lblRevenue.Name = "lblRevenue";
            this.lblRevenue.Size = new System.Drawing.Size(129, 38);
            this.lblRevenue.TabIndex = 1;
            this.lblRevenue.Text = "Tổng thu";
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Location = new System.Drawing.Point(1059, 10);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(94, 38);
            this.lblMonth.TabIndex = 0;
            this.lblMonth.Text = "Tháng";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lstBoxDetails, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1331, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.131783F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.86822F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(564, 838);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnToggleView);
            this.panel3.Controls.Add(this.lblDayChoosen);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(552, 48);
            this.panel3.TabIndex = 0;
            // 
            // btnToggleView
            // 
            this.btnToggleView.Location = new System.Drawing.Point(189, 1);
            this.btnToggleView.Name = "btnToggleView";
            this.btnToggleView.Size = new System.Drawing.Size(360, 42);
            this.btnToggleView.TabIndex = 1;
            this.btnToggleView.Text = "Chuyển";
            this.btnToggleView.UseVisualStyleBackColor = true;
            this.btnToggleView.Click += new System.EventHandler(this.btnToggleView_Click);
            // 
            // lblDayChoosen
            // 
            this.lblDayChoosen.AutoSize = true;
            this.lblDayChoosen.Location = new System.Drawing.Point(3, 5);
            this.lblDayChoosen.Name = "lblDayChoosen";
            this.lblDayChoosen.Size = new System.Drawing.Size(165, 38);
            this.lblDayChoosen.TabIndex = 0;
            this.lblDayChoosen.Text = "Day Choose";
            // 
            // lstBoxDetails
            // 
            this.lstBoxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBoxDetails.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstBoxDetails.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxDetails.FormattingEnabled = true;
            this.lstBoxDetails.IntegralHeight = false;
            this.lstBoxDetails.ItemHeight = 38;
            this.lstBoxDetails.Location = new System.Drawing.Point(3, 57);
            this.lstBoxDetails.Name = "lstBoxDetails";
            this.lstBoxDetails.Size = new System.Drawing.Size(558, 700);
            this.lstBoxDetails.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnAddSpend);
            this.panel4.Controls.Add(this.btnAddSchedule);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 763);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(558, 72);
            this.panel4.TabIndex = 2;
            // 
            // btnAddSpend
            // 
            this.btnAddSpend.Location = new System.Drawing.Point(258, 6);
            this.btnAddSpend.Name = "btnAddSpend";
            this.btnAddSpend.Size = new System.Drawing.Size(200, 66);
            this.btnAddSpend.TabIndex = 1;
            this.btnAddSpend.Text = "Thêm chi tiêu";
            this.btnAddSpend.UseVisualStyleBackColor = true;
            this.btnAddSpend.Click += new System.EventHandler(this.btnAddSpend_Click);
            // 
            // btnAddSchedule
            // 
            this.btnAddSchedule.Location = new System.Drawing.Point(3, 3);
            this.btnAddSchedule.Name = "btnAddSchedule";
            this.btnAddSchedule.Size = new System.Drawing.Size(249, 66);
            this.btnAddSchedule.TabIndex = 0;
            this.btnAddSchedule.Text = "Thêm lịch trình";
            this.btnAddSchedule.UseVisualStyleBackColor = true;
            this.btnAddSchedule.Click += new System.EventHandler(this.btnAddSchedule_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 38F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1898, 844);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Lịch trình";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPreviousMonth;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblSpend;
        private System.Windows.Forms.Label lblRevenue;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox lstBoxDetails;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAddSpend;
        private System.Windows.Forms.Button btnAddSchedule;
        private System.Windows.Forms.Panel panelCalendar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCategoryFilter;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblDayChoosen;
        private System.Windows.Forms.Button btnToggleView;
    }
}

