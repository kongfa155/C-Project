using ProjectC_.Theme;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectC_
{
    public partial class FormAddSpend : Form
    {
        // Các thuộc tính để truyền dữ liệu về MainForm
        public decimal Amount { get; private set; }
        public string Note { get; private set; }
        public string Type { get; private set; }
        public int CategoryId { get; private set; }
        // Tạo liên kết database để lấy danh mục
        private DatabaseHelper db = new DatabaseHelper();
        public FormAddSpend(int currentUserId)
        {
            InitializeComponent();
            // Thiết lập form để không bị thay đổi kích thước
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate dữ liệu đầu vào
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ!");
                return;
            }

            if (cbCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục!");
                return;
            }
            //Lấy dữ liệu từ form và gán vào các thuộc tính để truyền về MainForm
            Amount = amount;
            Note = txtNote.Text;
            Type = cbType.SelectedItem?.ToString() ?? "Chi";
            CategoryId = (int)cbCategory.SelectedValue;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormAddSpend_Load(object sender, EventArgs e)
        {
            //Áp dụng theme khi load form
            FormThemeHelper.ApplyInputForm(this);

            // Custom riêng
            cbType.SelectedIndex = 1; // mặc định Chi tiêu

            // Làm nổi input tiền
            txtAmount.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            txtAmount.ForeColor = ColorTranslator.FromHtml("#E67E22");
            // Load danh sách Category từ Database vào Combobox
            DataTable dt = db.GetCategories();
            cbCategory.DataSource = dt;
            cbCategory.DisplayMember = "Name"; // Hiển thị tên loại
            cbCategory.ValueMember = "Id";     // Giá trị ẩn là ID

            // Setup mặc định cho loại giao dịch là tiêu
            if (cbType.Items.Count > 0) cbType.SelectedIndex = 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}
