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
    public partial class FormAddSpend : Form
    {
        public decimal Amount { get; private set; }
        public string Note { get; private set; }
        public string Type { get; private set; }
        public int CategoryId { get; private set; }

        private DatabaseHelper db = new DatabaseHelper();
        public FormAddSpend()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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

            Amount = amount;
            Note = txtNote.Text;
            Type = cbType.SelectedItem?.ToString() ?? "Chi";
            CategoryId = (int)cbCategory.SelectedValue;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormAddSpend_Load(object sender, EventArgs e)
        {
            // 1. Load danh sách Category từ Database vào Combobox
            DataTable dt = db.GetCategories(); // Bạn cần viết hàm này trong DatabaseHelper
            cbCategory.DataSource = dt;
            cbCategory.DisplayMember = "Name"; // Hiển thị tên loại
            cbCategory.ValueMember = "Id";     // Giá trị ẩn là ID

            // 2. Setup mặc định cho loại giao dịch
            if (cbType.Items.Count > 0) cbType.SelectedIndex = 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                // Nếu người dùng nhập "50" thì tự đổi thành "50000"
                if (long.TryParse(txtAmount.Text, out long val))
                {
                    // Bạn có thể tùy chỉnh: Nếu số nhập vào < 1000 thì mới nhân thêm 1000
                    if (val < 1000 && val > 0)
                    {
                        txtAmount.Text = (val * 1000).ToString();
                    }
                }
            }
        }
    }
}
