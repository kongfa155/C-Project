using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectC_
{
    public partial class MainForm : Form
    {
        //Lưu người dùng hiện tại
        private int currentUserId = 0;
        // Chế độ hiển thị: true = Lịch trình, false = Chi tiêu
        private bool isScheduleView = true;
        // Tooltip để hiển thị nội dung và ghi chú khi hover vào Item
        private ToolTip itemToolTip = new ToolTip();
        private int lastHoverIndex = -1;
        public MainForm()
        {
            InitializeComponent();
        }
        //Hàm khởi tạo có đối số để nhận người dùng
        public MainForm(int userId) : this()
        {
            //Gán người dùng hiện tại
            currentUserId = userId;
            // Đăng ký sự kiện cho ListBox để thực hiện hover, hiển thị tooltip và tự động xuống dòng
            RegisterListBoxEvents();
            // Đặt form ở giữa
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        //Khởi tạo database
        DatabaseHelper db = new DatabaseHelper();
        DateTime currentDate = DateTime.Now;
        DateTime selectedDate = DateTime.Today; // Biến lưu ngày đang chọn

        private void RegisterListBoxEvents()
        {
            // Đăng ký sự kiện cho ListBox để thực hiện hover, hiển thị tooltip và tự động xuống dòng
            lstBoxDetails.MeasureItem += lstBoxDetails_MeasureItem;
            lstBoxDetails.DrawItem += lstBoxDetails_DrawItem;
            lstBoxDetails.MouseMove += lstBoxDetails_MouseMove;

            //Thêm sự kiện double click để xóa item
            lstBoxDetails.DoubleClick += lstBoxDetails_DoubleClick;
        }

        // Tính toán độ cao động cho mỗi Item dựa trên độ dài văn bản
        private void lstBoxDetails_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Kiểm tra chỉ số hợp lệ
            if (e.Index < 0 || e.Index >= lstBoxDetails.Items.Count) return;
            // Lấy Item và văn bản hiển thị
            CalendarItem item = lstBoxDetails.Items[e.Index] as CalendarItem;
            string text = item.ToString();

            // Tính toán kích thước cần thiết để hiển thị text với chiều rộng của ListBox
            SizeF size = e.Graphics.MeasureString(text, lstBoxDetails.Font, lstBoxDetails.Width - 10);

            // Cộng thêm khoảng trống (padding) để nhìn không bị dính
            e.ItemHeight = (int)Math.Ceiling(size.Height) + 10;
        }

        // Vẽ nội dung Item tự động xuống dòng
        private void lstBoxDetails_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Kiểm tra chỉ số hợp lệ
            if (e.Index < 0) return;
            // Vẽ nền và focus rectangle mặc định
            e.DrawBackground();
            e.DrawFocusRectangle();
            // Lấy Item và văn bản hiển thị
            CalendarItem item = lstBoxDetails.Items[e.Index] as CalendarItem;
            string text = item.ToString();

            // Xác định màu chữ (nếu đang chọn thì màu trắng, ngược lại màu đen)
            Brush textBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                              ? Brushes.White : Brushes.Black;

            // Vẽ text trong phạm vi hình chữ nhật của Item, tự động xuống dòng
            e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
        }

        // Hiển thị Tooltip khi hover vào Item
        private void lstBoxDetails_MouseMove(object sender, MouseEventArgs e)
        {
            // Xác định Item đang hover dựa trên vị trí chuột
            int index = lstBoxDetails.IndexFromPoint(e.Location);
            // Chỉ cập nhật khi hover vào item khác, không render lại khi hover vào 1 item đã hiển thị tooltip
            if (index != lastHoverIndex)
            {
                // Kiểm tra chỉ số hợp lệ
                if (index >= 0 && index < lstBoxDetails.Items.Count)
                {
                    // Lấy Item và hiển thị tooltip với nội dung và ghi chú
                    CalendarItem item = lstBoxDetails.Items[index] as CalendarItem;

                    // Hiển thị Content và Description (Note)
                    string tooltipText = $"Nội dung: {item.Content}\nGhi chú: {item.Description}";
                    itemToolTip.SetToolTip(lstBoxDetails, tooltipText);
                }
                else
                {
                    // Nếu không hover vào item nào, ẩn tooltip
                    itemToolTip.Hide(lstBoxDetails);
                }
                lastHoverIndex = index;
            }
        }






        private void LoadDataByDate(DateTime date)
        {
            // Xóa dữ liệu cũ
            lstBoxDetails.Items.Clear();
            // Cập nhật nhãn ngày đã chọn
            lblDayChoosen.Text = date.ToString("dd/MM/yyyy");
            // Lấy ID danh mục đã chọn trong ComboBox (nếu có)
            int selectedCatId = -1;
            if (cboCategoryFilter.SelectedValue != null && cboCategoryFilter.SelectedValue is int)
            {
                selectedCatId = (int)cboCategoryFilter.SelectedValue;
            }
            // Kiểm tra chế độ hiển thị và tải dữ liệu tương ứng
            if (isScheduleView)
            {
                cboCategoryFilter.Visible = false; // Ẩn cboCategoryFilter khi ở chế độ Lịch trình
                lblTotal.Visible = false; // Ẩn lblTotal khi ở chế độ Lịch trình
                btnToggleView.Text = "Xem Chi Tiêu"; // Cập nhật text nút chuyển chế độ

                // Lấy dữ liệu từ DB và chuyển đổi sang CalendarItem
                var schedules = db.GetAllActivitiesByDate(currentUserId, date);
                foreach (var item in schedules.Where(x => x.Type != "Expense"))
                {
                    if (item.Type == "Schedule")
                        lstBoxDetails.Items.Add(new CalendarItem
                        {
                            Id = item.Id,
                            Type = item.Type,
                            Content = $"{item.Content} {item.TimeStart} - {item.TimeEnd}",
                            Description = item.Description
                        });
                }
            }
            else //Hiển thị chi tiêu
            {
                // Hiển thị lblTotal và cboCategoryFilter khi ở chế độ Chi tiêu
                lblTotal.Visible = true;
                cboCategoryFilter.Visible = true;
                btnToggleView.Text = "Xem Lịch Trình";
                // Lấy dữ liệu từ DB và chuyển đổi sang CalendarItem
                var expenses = db.GetTransactionsByDate(currentUserId, date, selectedCatId);
                foreach (var item in expenses.Where(x => x.Type == "Expense"))
                {
                    lstBoxDetails.Items.Add(item);
                }
            }
            // Cập nhật lại thống kê tài chính dựa trên tháng, năm và hạng mục đã chọn
            UpdateFinancialSummary(date.Month, date.Year, selectedCatId);
        }

        // Sự kiện để xóa item khi double click vào ListBox
        private void lstBoxDetails_DoubleClick(object sender, EventArgs e)
        {
            if (lstBoxDetails.SelectedItem == null) return;
            // Lấy Item được chọn
            CalendarItem item = lstBoxDetails.SelectedItem as CalendarItem;

            // Không cho xóa birthday
            if (item.Type == "Birthday")
            {
                MessageBox.Show("Không thể xóa sinh nhật!");
                return;
            }
            // Xác nhận kiểu chi tiêu hay lịch trình để hiển thị thông báo phù hợp
            string typeText = item.Type == "Expense" ? "chi tiêu" : "lịch trình";

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa {typeText} này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            try
            {
                // Gọi hàm xóa tương ứng dựa trên loại item
                if (item.Type == "Expense")
                {
                    db.DeleteTransaction(item.Id, currentUserId);
                }
                else if (item.Type == "Schedule")
                {
                    db.DeleteSchedule(item.Id, currentUserId);
                }
                // Load lại dữ liệu sau khi xóa
                LoadDataByDate(selectedDate);

                MessageBox.Show("Đã xóa thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void UpdateFinancialSummary(int month, int year, int categoryId)
        {
            // Lấy tổng thu/chi trong tháng của hạng mục đó
            var finance = db.GetFinancesByMonth(currentUserId, month, year, categoryId);

            // Lấy số dư hiện tại của người dùng (không phụ thuộc vào bộ lọc ngày/tháng)
            decimal actualBalance = db.GetUserBalance(currentUserId);

            // Tính Tổng thu + Tổng chi = Tổng giao dịch (theo yêu cầu của bộ lọc)
            decimal totalFlow = finance.thu + finance.chi;

            // Hiển thị lên giao diện
            lblRevenue.Text = $"Tổng thu: {finance.thu:N0} VNĐ";
            lblSpend.Text = $"Tổng chi: {finance.chi:N0} VNĐ";

            // lblTotal mới theo yêu cầu của bộ lọc trong tháng đó (Tổng thu + Tổng chi)
            lblTotal.Text = $"Tổng giao dịch: {totalFlow:N0} VNĐ";

            // Số dư này là số dư ví thực tế của người dùng trong DB
            lblBalance.Text = $"Số dư hiện tại: {actualBalance:N0} VNĐ";

            lblMonth.Text = $"Tháng {month}/{year}";
        }
        // Sự kiện khi click vào ngày trên lịch
        private void Day_Click(object sender, EventArgs e)
        {
            // Lấy ngày từ Tag của Button được click
            Button btn = sender as Button;
            selectedDate = (DateTime)btn.Tag; // Gán lại biến selectedDate của class
            // Gọi hàm load dữ liệu theo ngày đã chọn
            LoadDataByDate(selectedDate);
        }

        //Render lịch
        private void RenderCalendar()
        {
            // Xóa lịch cũ
            panelCalendar.Controls.Clear();
            // Tính toán thông tin về tháng hiện tại
            DateTime firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)firstDay.DayOfWeek;
            // Cập nhật nhãn tháng năm
            lblDate.Text = currentDate.ToString("MMMM yyyy").ToUpper();

            //Bảng màu dùng cho lịch
            Color colorPaper = ColorTranslator.FromHtml("#FFF7ED"); // Dùng cho giấy lịch hiện tại và ngày tương lai
            Color colorPassed = ColorTranslator.FromHtml("#E7D3B1"); // Dùng cho ngày đã qua
            Color colorSelected = ColorTranslator.FromHtml("#ffa200"); // Dùng highlight ngày hiện tại
            Color colorText = ColorTranslator.FromHtml("#3F3F46");  // Dùng cho chữ trên lịch
            Color colorGridLine = Color.FromArgb(80, 60, 60, 60);   // Màu xám nhạt cho đường lưới
            //Chia lưới lịch gồm 7 * 7 (1 dòng dùng để ghi tiêu đề)
            int cellWidth = panelCalendar.Width / 7;
            int cellHeight = panelCalendar.Height / 7;

            //Tiêu đề thứ
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            // Vẽ tiêu đề thứ
            for (int i = 0; i < 7; i++)
            {
                Label lblThu = new Label();
                lblThu.Text = dayNames[i];
                lblThu.Size = new Size(cellWidth, cellHeight / 2);
                lblThu.Location = new Point(i * cellWidth, 0);
                lblThu.TextAlign = ContentAlignment.MiddleCenter;
                lblThu.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblThu.ForeColor = colorText;
                panelCalendar.Controls.Add(lblThu);
            }

            // Các ngày trong tháng
            int day = 1;
            // Vẽ các ô ngày, bắt đầu từ startDay và dừng khi hết ngày trong tháng
            for (int i = 0; i < 6; i++)
            {
                // i = 0 là dòng đầu tiên chứa ngày bắt đầu, nếu startDay > 0 thì sẽ bỏ qua các ô trước đó
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j < startDay) continue;
                    if (day > daysInMonth) break;
                    // Tạo đối tượng DateTime cho ngày hiện tại để so sánh với ngày hôm nay
                    DateTime targetDate = new DateTime(currentDate.Year, currentDate.Month, day);

                    //Mỗi ô là một button
                    Button btn = new Button();
                    // Kích thước ô
                    btn.Size = new Size(cellWidth + 1, cellHeight + 1);
                    btn.Left = j * cellWidth;
                    btn.Top = (i * cellHeight) + (cellHeight / 2);
                    //Gán ngày cho nút
                    btn.Text = day.ToString();
                    btn.Font = new Font("MS Sans Serif", 12, FontStyle.Bold);
                    btn.Tag = targetDate;
                    btn.ForeColor = colorText;

                    // Style cơ bản
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.Black; // Màu đường lưới

                    // Logic đổ màu theo trạng thái ngày
                    if (targetDate < DateTime.Today)
                    {
                        // Ngày đã qua
                        btn.BackColor = colorPassed;
                    }
                    else if (targetDate == DateTime.Today)
                    {
                        // Ngày hiện tại
                        btn.BackColor = colorPaper;
                        btn.FlatAppearance.BorderSize = 3; // Viền dày hơn để thấy rõ màu xanh
                        btn.FlatAppearance.BorderColor = colorSelected;
                        btn.BringToFront(); // Đưa lên trên cùng để viền xanh không bị ô khác đè
                    }
                    else
                    {
                        // Ngày tương lai
                        btn.BackColor = colorPaper;
                    }
                    // Đăng ký sự kiện click cho nút ngày
                    btn.Click += Day_Click;
                    panelCalendar.Controls.Add(btn);
                    day++;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Load theme
            AppTheme.ApplyTheme(this);
            // Header (panel1)
            panel1.BackColor = AppTheme.Primary;
            lblDate.ForeColor = AppTheme.White;

            // Panel thống kê
            panel2.BackColor = AppTheme.Panel;

            // Nút quan trọng
            btnAddSpend.BackColor = AppTheme.Accent;
            btnAddSchedule.BackColor = AppTheme.Primary;

            // Label nổi bật
            lblRevenue.ForeColor = AppTheme.PrimaryDark;
            lblSpend.ForeColor = AppTheme.Accent;
            lblBalance.ForeColor = AppTheme.Primary;
            //Load lịch sau cùng để không bị theme ảnh hưởng
            RenderCalendar();
            // Cập nhật nhãn ngày đã chọn ban đầu
            lblDayChoosen.Text = currentDate.ToString("dd/MM/yyyy");
            // Load Categories
            DataTable dt = db.GetCategories();
            // Thêm tùy chọn "Tất cả" vào đầu danh sách
            DataRow dr = dt.NewRow();
            dr["Id"] = -1;
            dr["Name"] = "--- Tất cả ---";
            dt.Rows.InsertAt(dr, 0);
            // Thiết lập DataSource cho ComboBox
            cboCategoryFilter.DataSource = dt;
            cboCategoryFilter.DisplayMember = "Name";
            cboCategoryFilter.ValueMember = "Id";
            // Load dữ liệu cho ngày hiện tại
            LoadDataByDate(selectedDate);
        }

        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            // Tạo Form thêm lịch trình mới
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
            // Tạo form chi tiêu và truyền userId hiện tại để lưu vào DB
            using (FormAddSpend f = new FormAddSpend(currentUserId))
            {
                // Hiển thị form dưới dạng Dialog
                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Chuyển đổi Logic Type "Thu" -> 1, "Chi" -> 0
                        string typeText = f.Type.ToLower();
                        int typeBit = (typeText.Contains("thu") || typeText.Contains("income")) ? 1 : 0;

                        // Gọi hàm để lưu db
                        db.InsertTransaction(
                            currentUserId,
                            f.Amount,
                            f.Note,
                            typeBit,
                            f.CategoryId,
                            selectedDate // Lưu vào đúng ngày đang chọn trên lịch
                        );

                        // Render lại giao diện sau khi lưu thành công
                        // Load lại ListBox và tính toán lại lblRevenue, lblSpend, lblBalance
                        LoadDataByDate(selectedDate);

                        MessageBox.Show("Lưu giao dịch thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
        //Đảo trạng thái hiển thị dữ liệu
        private void btnToggleView_Click(object sender, EventArgs e)
        {
            isScheduleView = !isScheduleView; // Đảo trạng thái
            LoadDataByDate(selectedDate);
        }
        // Sự kiện khi thay đổi lựa chọn trong ComboBox lọc hạng mục chi tiêu
        private void cboCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataByDate(selectedDate);
        }
    }
}
