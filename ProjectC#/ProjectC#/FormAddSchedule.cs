using ProjectC_.Theme;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectC_
{
    public partial class FormAddSchedule : Form
    {
        // Các thuộc tính để truyền dữ liệu về MainForm
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TimeSpan TimeStart { get; private set; }
        public TimeSpan TimeEnd { get; private set; }

        public FormAddSchedule()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề!");
                return;
            }
            // Lấy dữ liệu từ form và gán vào các thuộc tính để truyền về MainForm
            Title = txtTitle.Text;
            Description = txtDescription.Text;
            TimeStart = dtStart.Value.TimeOfDay;
            TimeEnd = dtEnd.Value.TimeOfDay;
            // Kiểm tra thời gian hợp lệ
            if (TimeEnd <= TimeStart)
            {
                MessageBox.Show("Thời gian kết thúc phải sau thời gian bắt đầu!");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormAddSchedule_Load(object sender, EventArgs e)
        {
            // Áp dụng theme cho form
            FormThemeHelper.ApplyInputForm(this);

            // Style thêm cho DateTime
            dtStart.BackColor = ColorTranslator.FromHtml("#FFD166");
            dtEnd.BackColor = ColorTranslator.FromHtml("#FFD166");
        }
    }
}
