using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectC_
{
    public partial class MainForm : Form
    {
        private int currentUserId = 0;
        // Chế độ hiển thị: true = Lịch trình, false = Chi tiêu
        private bool isScheduleView = true;
        public MainForm()
        {
            InitializeComponent();
        }
        public MainForm(int userId) : this()
        {
            currentUserId = userId;
            RegisterListBoxEvents();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }
        
        // Thay vì List giả, ta khởi tạo helper
        DatabaseHelper db = new DatabaseHelper();
        DateTime currentDate = DateTime.Now;
        DateTime selectedDate = DateTime.Today; // Biến lưu ngày đang chọn





        // Khai báo thêm ToolTip ở đầu class MainForm
        private ToolTip itemToolTip = new ToolTip();
        private int lastHoverIndex = -1;

        private void RegisterListBoxEvents()
        {
            // Đăng ký sự kiện (nếu bạn chưa đăng ký trong Designer)
            lstBoxDetails.MeasureItem += lstBoxDetails_MeasureItem;
            lstBoxDetails.DrawItem += lstBoxDetails_DrawItem;
            lstBoxDetails.MouseMove += lstBoxDetails_MouseMove;
        }

        // 1. Tính toán độ cao động cho mỗi Item dựa trên độ dài văn bản
        private void lstBoxDetails_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstBoxDetails.Items.Count) return;

            CalendarItem item = lstBoxDetails.Items[e.Index] as CalendarItem;
            string text = item.ToString();

            // Tính toán kích thước cần thiết để hiển thị text với chiều rộng của ListBox
            SizeF size = e.Graphics.MeasureString(text, lstBoxDetails.Font, lstBoxDetails.Width - 10);

            // Cộng thêm khoảng trống (padding) để nhìn không bị dính
            e.ItemHeight = (int)Math.Ceiling(size.Height) + 10;
        }

        // 2. Vẽ nội dung Item tự động xuống dòng
        private void lstBoxDetails_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            e.DrawFocusRectangle();

            CalendarItem item = lstBoxDetails.Items[e.Index] as CalendarItem;
            string text = item.ToString();

            // Xác định màu chữ (nếu đang chọn thì màu trắng, ngược lại màu đen)
            Brush textBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                              ? Brushes.White : Brushes.Black;

            // Vẽ text trong phạm vi hình chữ nhật của Item, tự động xuống dòng
            e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
        }

        // 3. Hiển thị Tooltip khi hover vào Item
        private void lstBoxDetails_MouseMove(object sender, MouseEventArgs e)
        {
            int index = lstBoxDetails.IndexFromPoint(e.Location);

            if (index != lastHoverIndex)
            {
                if (index >= 0 && index < lstBoxDetails.Items.Count)
                {
                    CalendarItem item = lstBoxDetails.Items[index] as CalendarItem;

                    // Hiển thị Content và Description (Note)
                    string tooltipText = $"Nội dung: {item.Content}\nGhi chú: {item.Description}";
                    itemToolTip.SetToolTip(lstBoxDetails, tooltipText);
                }
                else
                {
                    itemToolTip.Hide(lstBoxDetails);
                }
                lastHoverIndex = index;
            }
        }






        private void LoadDataByDate(DateTime date)
        {
            lstBoxDetails.Items.Clear();
            lblDayChoosen.Text = date.ToString("dd/MM/yyyy");

            int selectedCatId = -1;
            if (cboCategoryFilter.SelectedValue != null && cboCategoryFilter.SelectedValue is int)
            {
                selectedCatId = (int)cboCategoryFilter.SelectedValue;
            }

            if (isScheduleView)
            {
                cboCategoryFilter.Visible = false;
                btnToggleView.Text = "Xem Chi Tiêu";

                // Lấy dữ liệu từ DB và chuyển đổi sang CalendarItem
                var schedules = db.GetAllActivitiesByDate(currentUserId, date);
                foreach (var item in schedules.Where(x => x.Type != "Expense"))
                {
                    lstBoxDetails.Items.Add(new CalendarItem
                    {
                        Type = item.Type == "Birthday" ? "Birthday" : "Schedule",
                        Content = item.Content,
                        Description = item.Description // PHẢI lấy từ DB thay vì để ""
                    });
                }
            }
            else
            {
                cboCategoryFilter.Visible = true;
                btnToggleView.Text = "Xem Lịch Trình";

                var expenses = db.GetTransactionsByDate(currentUserId, date, selectedCatId);
                foreach (var item in expenses)
                {
                    lstBoxDetails.Items.Add(new CalendarItem
                    {
                        Type = "Expense",
                        Content = item.Content,
                        Description = ""
                    });
                }
            }

            UpdateFinancialSummary(date.Month, date.Year, selectedCatId);
        }
        private void UpdateFinancialSummary(int month, int year, int categoryId)
        {
            var finance = db.GetFinancesByMonth(currentUserId, month, year, categoryId);

            lblRevenue.Text = $"Tổng thu: {finance.thu:N0} VNĐ";
            lblSpend.Text = $"Tổng chi: {finance.chi:N0} VNĐ";
            lblBalance.Text = $"Số dư: {(finance.thu - finance.chi):N0} VNĐ";
            lblMonth.Text = $"Tháng {month}/{year}";
        }


        private void panelCalendar_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Day_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            selectedDate = (DateTime)btn.Tag; // Gán lại biến selectedDate của class
            LoadDataByDate(selectedDate);
        }

        private void RenderCalendar()
        {
            panelCalendar.Controls.Clear();

            DateTime firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)firstDay.DayOfWeek;

            lblDate.Text = currentDate.ToString("MMMM yyyy");

            // --- PHẦN TÍNH TOÁN KÍCH THƯỚC ĐỘNG ---
            int margin = 5;
            // Chia cho 7 cột và 6 hàng, trừ đi khoảng cách margin
            int cellWidth = (panelCalendar.Width - (margin * 8)) / 7;
            int cellHeight = (panelCalendar.Height - (margin * 7)) / 6;

            int day = 1;

            for (int i = 0; i < 6; i++) // rows
            {
                for (int j = 0; j < 7; j++) // columns
                {
                    if (i == 0 && j < startDay) continue;
                    if (day > daysInMonth) break;

                    Button btn = new Button();
                    btn.Width = cellWidth;   // Sử dụng chiều rộng đã tính
                    btn.Height = cellHeight; // Sử dụng chiều cao đã tính
                    btn.Left = j * (cellWidth + margin) + margin;
                    btn.Top = i * (cellHeight + margin) + margin;

                    btn.Text = day.ToString();
                    btn.Font = new Font(this.Font.FontFamily, cellHeight / 4); // Tự điều chỉnh cỡ chữ theo ô
                    btn.Tag = new DateTime(currentDate.Year, currentDate.Month, day);

                    // Style
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.BackColor = Color.Beige;
                    btn.Click += Day_Click;

                    panelCalendar.Controls.Add(btn);

                    if (((DateTime)btn.Tag).Date == DateTime.Today)
                    {
                        btn.BackColor = Color.LightGreen;
                    }

                    day++;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RenderCalendar();
            lblDayChoosen.Text = currentDate.ToString("dd/MM/yyyy");
            // Load Categories
            DataTable dt = db.GetCategories();
            // Thêm tùy chọn "Tất cả" vào đầu danh sách
            DataRow dr = dt.NewRow();
            dr["Id"] = -1;
            dr["Name"] = "--- Tất cả ---";
            dt.Rows.InsertAt(dr, 0);

            cboCategoryFilter.DataSource = dt;
            cboCategoryFilter.DisplayMember = "Name";
            cboCategoryFilter.ValueMember = "Id";

            LoadDataByDate(selectedDate);
        }

        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            FormAddSchedule f = new FormAddSchedule();
            // Truyền ngày đang chọn sang form thêm (nếu form đó có hiển thị ngày)
            if (f.ShowDialog() == DialogResult.OK)
            {
                // GỌI DB ĐỂ LƯU
                db.InsertSchedule(
                    currentUserId,
                    selectedDate,
                    f.TimeStart.ToString(@"hh\:mm"),
                    f.TimeEnd.ToString(@"hh\:mm"),
                    f.Title,
                    f.Description
                );

                // Load lại dữ liệu tại ngày đó
                LoadDataByDate(selectedDate);
            }
        }

        private void btnAddSpend_Click(object sender, EventArgs e)
        {
            FormAddSpend f = new FormAddSpend();
            if (f.ShowDialog() == DialogResult.OK)
            {
                // Giả sử f.Type trả về "Income" thì type = 1, ngược lại = 0
                int typeBit = (f.Type == "Income") ? 1 : 0;

                // GỌI DB ĐỂ LƯU
                db.InsertTransaction(
                    currentUserId,
                    f.Amount,
                    f.Note,
                    typeBit,
                    f.CategoryId, // Đảm bảo FormAddSpend trả về ID của Category
                    selectedDate
                );

                // Load lại dữ liệu
                LoadDataByDate(selectedDate);
            }
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            // 1. Tăng tháng lên trước
            currentDate = currentDate.AddMonths(1);

            // 2. Cập nhật selectedDate về ngày 1 của tháng mới để tránh lỗi ngày 31
            selectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);

            // 3. Vẽ lại lịch và load dữ liệu
            RenderCalendar();
            LoadDataByDate(selectedDate);
        }

        private void btnPreviousMonth_Click(object sender, EventArgs e)
        {
            // 1. Giảm tháng xuống
            currentDate = currentDate.AddMonths(-1);

            // 2. Cập nhật selectedDate về ngày 1 của tháng mới
            selectedDate = new DateTime(currentDate.Year, currentDate.Month, 1);

            // 3. Vẽ lại lịch và load dữ liệu
            RenderCalendar();
            LoadDataByDate(selectedDate);
        }

        private void btnToggleView_Click(object sender, EventArgs e)
        {
            isScheduleView = !isScheduleView; // Đảo trạng thái
            LoadDataByDate(selectedDate);
        }

        private void cboCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataByDate(selectedDate);
        }
    }
}
